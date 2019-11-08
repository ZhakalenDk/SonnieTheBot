using Discord;
using Discord.WebSocket;
using DiscordBot.Data.Events;
using DiscordBot.Data.Users;
using DiscordBot.OS.FacebookHook;
using DiscordBot.OS.System.Json;
using DiscordBot.OS.System.Time;
using DiscordBot.OS.System.Time.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.OS.Discord
{
    /// <summary>
    /// Represents a Discord configuration handler
    /// </summary>
    public class DiscordHandler
    {
        #region Singleton Instance
        /// <summary>
        /// THe instance of the DiscordHandler class. Ensures that only one instance of this object is present at any given time
        /// </summary>
        public static DiscordHandler Instance
        {
            get
            {
                if ( instance == null )
                {
                    instance = JDecoder.DecodeFromFile<DiscordHandler> ( "DiscordConfig.Json", new Newtonsoft.Json.JsonSerializerSettings
                    {
                        MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore
                    } );

                    Debug.Log.Message ( $"DiscordHandler - Configurating Suggestion Channel: {instance.SuggestionChannelID}" );
                    Debug.Log.Message ( $"DiscordHandler - Configurating General Channel: {instance.GeneralChannelID}" );

                }

                return instance;
            }
        }
        private static DiscordHandler instance;
        #endregion

        /// <summary>
        /// Indicates if the Discord client is ready or not
        /// </summary>
        public bool Ready { get; private set; } = false;
        public DiscordSocketClient Client { get; set; }
        /// <summary>
        /// The Discord servers general channel
        /// </summary>
        public string GeneralChannelID { get; set; }
        /// <summary>
        /// The Discord Application client channel attached to the Suggestions command response
        /// </summary>
        public string SuggestionChannelID { get; set; } = null;
        /// <summary>
        /// The last user to type in chat
        /// </summary>
        public string LastUserToType { get; private set; }
        /// <summary>
        /// Sonnies Vacation indicator
        /// </summary>
        public Vacation Vacation { get; set; } = Vacation.SetInvalidVacation ();
        /// <summary>
        /// How often the Facebook feed should be updated
        /// </summary>
        private readonly UpdateTimer facebookUpdateTimer = new UpdateTimer ( "Facbook", new Time ( _minutes: 1 ) );
        /// <summary>
        /// How often the Sonnie should check for porno
        /// </summary>
        private readonly UpdateTimer checkForPornoUpdateTimer = new UpdateTimer ( "CheckForPorno", new Time ( _minutes: 11, _seconds: 15 ) );
        /// <summary>
        /// How often Sonnie should delete his messages
        /// </summary>
        private readonly UpdateTimer deleteOldMessages = new UpdateTimer ( "DeleteMessages", new Time ( _hour: 1 ) );

        /// <summary>
        /// Empty Constructer
        /// </summary>
        private DiscordHandler ()
        {

        }

        /// <summary>
        /// An ection that represents a Discrod Latency check (Heartbeat)
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        public async Task ClientLatencyUpdated ( int arg1, int arg2 )
        {
            //  If the client is ready and the current time exceeds the update timer
            if ( Instance.Ready )
            {
                #region Delete commands messages and reponses
                //  THe channel to write a message in
                ISocketMessageChannel channel = Client.GetChannel ( ulong.Parse ( GeneralChannelID ) ) as ISocketMessageChannel;

                if ( this.deleteOldMessages.ReadyToUpdate () )
                {
                    Debug.Log.Message ( "DiscordHandler - Deleting messages" );

                    //  Loop trough the last 100 cached messages and delete them if it was a command or if Sonnie wrote them
                    foreach ( var message in await channel.GetMessagesAsync ().FlattenAsync () )
                    {
                        if ( message.Author.Username == Client.CurrentUser.Username || message.Content [ 0 ] == ';' )
                        {
                            //Debug.Log.Message ( $"Auther: {message.Author.Username} | Content: {message.Content}" );
                            await message.DeleteAsync ();
                        }
                    }
                }
                #endregion

                #region Checking vacation state
                //  Tell people in the discord that Sonnie is no longer in vacation mode.
                if ( !Vacation.OnVecation () && Vacation.WasOnVacation )
                {
                    Debug.Log.Message ( "Returned from vac" );
                    await channel.SendMessageAsync ( "Jeg er tilbage fra ferie!" );
                    Vacation = Vacation.SetInvalidVacation ();
                    Vacation.WasOnVacation = false;
                    System.DataScanner<Vacation> vacScanner = new System.DataScanner<Vacation> ( @"\Data\Events\STB.VP" );
                    await vacScanner.WriteToFile ( string.Empty );
                }

                //  To track if Sonnie was on vacation mode recently.
                if ( Vacation.OnVecation () && !Vacation.WasOnVacation )
                {
                    Debug.Log.Message ( "Going into vac" );
                    await channel.SendMessageAsync ( "Jeg går på ferie nu!" );
                    Vacation.WasOnVacation = true;
                }

                if ( !Vacation.OnVecation () && Instance.checkForPornoUpdateTimer.ReadyToUpdate () )
                {
                    foreach ( var guildUser in Client.GetGuild ( 614042459957362698 ).Users )
                    {
                        await CheckForPorno ( guildUser, channel );
                    }
                }
                #endregion

                #region Checking if there's an event that should be prompted
                //  Check if there's any upcomming events
                foreach ( Event item in EventManager.Events )
                {
                    DateTime today = new DateTime ( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0 );
                    Debug.Log.Message ( $"DiscordHandler - Comparing {item.Prompt.TimeOfDay} to {today.TimeOfDay}" );

                    //  If an events prompt time is now, remind the users about the event
                    if ( item.Prompt.Date == today.Date && item.Prompt.TimeOfDay == today.TimeOfDay )
                    {
                        Debug.Log.Message ( "DiscordHandler - Duo time: Promping users" );
                        //  Send a message to each user in the channel
                        foreach ( IGuildUser user in await channel.GetUsersAsync ().FlattenAsync () )
                        {
                            if ( !user.IsBot )
                            {
                                IDMChannel dmChannel = await user.GetOrCreateDMChannelAsync ();

                                // Only try to send if a DM channel can be established 
                                if ( dmChannel != null )
                                {
                                    Debug.Log.Message ( $"DiscordHandler - Sending to: {user.Username}" );
                                    _ = dmChannel.SendMessageAsync ( $"Påmindelse omkring event:{Environment.NewLine}{item.Print ()}" );
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Update Facebook log
                //  Check for new Feacebook posts
                if ( Instance.facebookUpdateTimer.ReadyToUpdate () )
                {
                    Debug.Log.Message ( "DiscordHandler - Updating Facebook-Feed" );

                    FacebookHandler.Instance.SendHTTPRequest ().GetAwaiter ().GetResult ();

                    await FacebookHandler.Instance.PostLastFacebookPost ( Client );
                }
                #endregion
            }
        }

        /// <summary>
        /// Accours when a user is typing a message
        /// </summary>
        /// <param name="_user">The user, who is typing</param>
        /// <param name="_channel">THe channel the user is typing the message in</param>
        /// <returns></returns>
        public async Task UserIsTyping ( SocketUser _user, ISocketMessageChannel _channel )
        {
            await Client.SetGameAsync ( $"{_user.Username}", "", ActivityType.Watching );

            if ( UserManager.SearchByMention ( _user.Mention ) == null && LastUserToType != _user.Mention )
            {
                LastUserToType = _user.Mention;
                await _channel.SendMessageAsync ( $"Ehm.. Hvem er du, {_user.Mention}?{Environment.NewLine}Skriv ```;Jeg er [Dit fulde navn]{Environment.NewLine}EXAMPLE: ;jeg er \"Sonnie Eis\"```" );
            }
        }

        /// <summary>
        /// Accours when the client is ready and connected
        /// </summary>
        /// <returns></returns>
        public Task ReadyAsync ()
        {
            Debug.Log.Message ( $"Connected as {Client.CurrentUser} v{Program.VERSION}" );

            Instance.Ready = true;

            return Task.CompletedTask;
        }

        /// <summary>
        /// Accours everytime a handler is activivated and a message is sent back to the bot client
        /// </summary>
        /// <param name="_log"></param>
        /// <returns></returns>
        public Task LogAsync ( LogMessage _log )
        {
            Debug.Log.Message ( $"{_log.Source} - {_log.Message}" );
            return Task.CompletedTask;
        }

        /// <summary>
        /// Check of a user is watching "Porn" during school hours
        /// </summary>
        /// <param name="_user">The user, who is watching "Porn"</param>
        /// <param name="_channel">The channel to post a repsonse in</param>
        /// <returns></returns>
        private async Task CheckForPorno ( SocketGuildUser _user, ISocketMessageChannel _channel )
        {
            #region School Time
            DateTime schoolStart = new DateTime ( DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 8, 10, 0 );
            #region Breaks
            DateTime firstBreakStart = new DateTime ( DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 9, 50, 0 );
            DateTime firstBreakEnd = new DateTime ( DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10, 10, 0 );

            DateTime secondBreakStart = new DateTime ( DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 11, 50, 0 );
            DateTime secondBreakEnd = new DateTime ( DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12, 20, 0 );

            DateTime thirdBreakStart = new DateTime ( DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 14, 0, 0 );
            DateTime thirdBreakEnd = new DateTime ( DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 14, 20, 0 );
            #endregion
            DateTime schoolEnd = new DateTime ( DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 15, 10, 0 );
            #endregion

            //Console.WriteLine ( $"Activity Value: {(_user.Activity == null ? "Null" : _user.Activity.Type.ToString ())}" );

            //  If a user is updated and they're playing games or watching something within school hours
            if ( _user.Activity != null && DateTime.Now.IsWithin ( schoolStart, schoolEnd ) && !DateTime.Now.IsWithin ( firstBreakStart, firstBreakEnd ) && !DateTime.Now.IsWithin ( secondBreakStart, secondBreakEnd ) && !DateTime.Now.IsWithin ( thirdBreakStart, thirdBreakEnd ) && !DateTime.Now.IsWeekend () )
            {
                if ( _user.Activity.Type == ActivityType.Playing || _user.Activity.Type == ActivityType.Watching )
                {
                    Debug.Log.Message ( $"Program - User \"{_user.Username}\" is {_user.Activity.Type}: {_user.Activity.Name}" );

                    await _channel.SendMessageAsync ( $"Hold op med det porno, {_user.Mention}!" );
                }
            }
        }
    }
}