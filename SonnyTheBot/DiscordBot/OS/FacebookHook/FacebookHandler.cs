using Discord;
using Discord.WebSocket;
using DiscordBot.OS.Extensions;
using DiscordBot.OS.FacebookHook.Data;
using DiscordBot.OS.System.Json;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBot.OS.FacebookHook
{
    /// <summary>
    /// Represents a Facebook connection handler
    /// </summary>
    public class FacebookHandler
    {
        #region Singleton Instance
        /// <summary>
        /// THe instance of the FacebookHandler class. Ensures that only one instance of this object is present at any given time
        /// </summary>
        public static FacebookHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    ////  The path to the FacebookConfig.Json file
                    //string path = Path.GetDirectoryName ( Assembly.GetExecutingAssembly ().Location ) + @"\Data\FacebookConfig.Json";

                    ////  THe file content
                    //string JsonFile;
                    //using (StreamReader reader = new StreamReader ( path ))
                    //{
                    //    JsonFile = reader.ReadToEnd ();
                    //}

                    ////  Deserialize the Json object response into a C# class object.
                    //instance = JsonConvert.DeserializeObject<FacebookHandler> ( JsonFile, new JsonSerializerSettings
                    //{
                    //    //  Do not try to Deserialize fields that are not present in the Json Object response
                    //    MissingMemberHandling = MissingMemberHandling.Ignore
                    //} );

                    instance = JDecoder.DecodeFromFile<FacebookHandler> ( "FacebookConfig.Json", new JsonSerializerSettings
                    {
                        //  DO not try to decode fields that are not present in the Json file
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    } );

                    Debug.Log.Message ( $"FacebookHandler - Configurating API: {instance.GraphAPI}" );
                    Debug.Log.Message ( $"FacebookHandler - Configurating GroupID: {instance.GroupID}" );
                    Debug.Log.Message ( $"FacebookHandler - Configurating Token: {(instance.Token != null ? "Loaded" : "null")}" );
                    Debug.Log.Message ( $"FacebookHandler - Configurating Fields: {instance.Fields}" );
                    Debug.Log.Message ( $"FacebookHandler - Configurating FacebookFeedChannel: {instance.FacebookFeedChannelID}" );

                    //  The Facebook Hook
                    instance.facebookClient.BaseAddress = new Uri ( instance.GraphAPI + instance.GroupID );
                }

                return instance;
            }
        }
        private static FacebookHandler instance;
        #endregion

        /// <summary>
        /// Facebook Data stream
        /// </summary>
        public FacebookData Facebook { get; private set; }

        /// <summary>
        /// The last pulled post from Facebook that was posted on Discord
        /// </summary>
        public DateTime LastPost { get; set; }

        /// <summary>
        /// The URL to the Facebook GraphAPI
        /// </summary>
        public string GraphAPI { get; set; } = null;
        /// <summary>
        /// The ID of the Facebook-Group to pull posts from
        /// </summary>
        public string GroupID { get; set; } = null;
        /// <summary>
        /// The Fields to inlude in the Json Response Object
        /// </summary>
        public string Fields { get; set; } = null;
        /// <summary>
        /// The Facebook token to grant access to the Facebook API
        /// </summary>
        public string Token { get; set; } = null;

        public string FacebookFeedChannelID { get; set; } = null;

        /// <summary>
        /// The HTTP Client used to request data from Facebook
        /// </summary>
        private readonly HttpClient facebookClient = new HttpClient ();

        /// <summary>
        /// Send an HTTP request to facebook
        /// </summary>
        /// <returns></returns>
        public async Task SendHTTPRequest ()
        {

            //  Get the Json object response from the Facebook Graph API
            HttpResponseMessage message = this.facebookClient.GetAsync ( $"{Fields}&access_token={Token}" ).Result;

            message.EnsureSuccessStatusCode ();

            string result = await message.Content.ReadAsStringAsync ();

            //  Deserialize the Json object response into a C# class object.
            Facebook = JsonConvert.DeserializeObject<FacebookData> ( result, new JsonSerializerSettings
            {
                //  Do not try to Deserialize fields that are not present in the Json Object response
                MissingMemberHandling = MissingMemberHandling.Ignore
            } );

            //await DebugFeed ();
        }

        /// <summary>
        /// Print ot the entire feed in the console
        /// </summary>
        /// <returns></returns>
        public Task DebugFeed ()
        {
            int postIndex = 0;
            int commentIndex = 0;
            int subCommentIndex = 0;

            //  Loop trough each post
            foreach (var post in Facebook.feed.data)
            {
                Console.WriteLine ( $"[{postIndex}]Post - [{post.id}]\n    {post.message}" );

                //  ONly try to collect comments to a post if there is any
                if (post.comments != null)
                {
                    //  Loop trough each comment
                    foreach (var comment in post.comments.data)
                    {
                        Console.WriteLine ( $"        [{commentIndex}]Comment - [{comment.id}]\n            {comment.message}" );

                        //  Only try to collect subComments to a comment if there is any
                        if (comment.comments != null)
                        {
                            //  Loop trough each subComment
                            foreach (var subComment in comment.comments.data)
                            {
                                Console.WriteLine ( $"                [{subCommentIndex}]SubComment - [{subComment.id}]\n                    {subComment.message}" );
                                subCommentIndex++;
                            }
                        }

                        commentIndex++;
                    }
                }

                postIndex++;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Empty constructer
        /// </summary>
        private FacebookHandler ()
        {

        }

        /// <summary>
        /// Pull and post the last updated post on facebook
        /// </summary>
        /// <param name="_discordClient"></param>
        /// <returns></returns>
        public async Task PostLastFacebookPost ( DiscordSocketClient _discordClient )
        {
            #region Debug ID's
            /*Facebook feed channel ID
                619919379148046356
            */

            /*Test-Chamber Channel ID
                615970271827853312
            */
            #endregion

            IMessageChannel channel = await _discordClient.GetFacebookFeedChannel () as IMessageChannel;

            //  Iterrate trough all posts in the feed
            foreach (var post in Instance.Facebook.feed.data)
            {
                //  Check if a post is new
                if (post.GetPostTime ().ToLocalTime () > LastPost)
                {
                    //  Set the newest post date
                    LastPost = post.GetPostTime ().ToLocalTime ();
                    await channel.SendMessageAsync ( $"**<Post Feed Updated>**{Environment.NewLine}{post.message}{Environment.NewLine}{post.full_picture ?? ""}" );

                    //  Only try to collect comments to a post if there is any
                    if (post.comments != null)
                    {
                        //  Loop trough each comment
                        foreach (var comment in post.comments.data)
                        {
                            await channel.SendMessageAsync ( $"----**Comment:** <{post.id}>{Environment.NewLine}----: _{comment.message}_" );

                            //  Only try to collect subComments to a comment if there is any
                            if (comment.comments != null)
                            {
                                //  Loop trough each subComment
                                foreach (var subComment in comment.comments.data)
                                {
                                    await channel.SendMessageAsync ( $"--------**SubComment**:{Environment.NewLine}--------:_{subComment.message}_" );
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}