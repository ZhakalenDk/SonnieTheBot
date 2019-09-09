using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using DiscordBot.OS.Discord.CommandPipe;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DiscordBot.Data.Users;
using DiscordBot.OS.System;
using DiscordBot.OS.FacebookHook;
using DiscordBot.OS.System.Time;
using DiscordBot.OS.System.Time.Extensions;
using DiscordBot.OS.Discord;

namespace DiscordBot
{
    class Program
    {
        private readonly IConfiguration config;
        /// <summary>
        /// Version Control ([Major feaures].[Minor features].[Major fixes].[Minor fixes])
        /// </summary>
        public static string VERSION = "2.4.4.1";

        static void Main ( string[] args )
        {
            FacebookHandler.Instance.LastPost = DateTime.Now;
            new Program ().MainAsync ().GetAwaiter ().GetResult ();

            //  Make sure the Console does not close
            Console.Read ();
        }
        public Program ()
        {
            var builder = new ConfigurationBuilder ().SetBasePath ( AppContext.BaseDirectory ).AddJsonFile ( path: "config.json" );

            this.config = builder.Build ();

            UserManager.SetUserList ( DataScanner.ReadFromFile () );
        }

        /// <summary>
        /// THe starting point of the bo execution
        /// </summary>
        /// <returns></returns>
        private async Task MainAsync ()
        {
            using (var services = ConfigureServices ())
            {
                var rSClient = services.GetRequiredService<DiscordSocketClient> ();
                DiscordHandler.Instance.Client = rSClient;

                DiscordHandler.Instance.Client.Log += DiscordHandler.Instance.LogAsync;
                DiscordHandler.Instance.Client.Ready += DiscordHandler.Instance.ReadyAsync;
                DiscordHandler.Instance.Client.UserIsTyping += DiscordHandler.Instance.UserIsTyping;
                DiscordHandler.Instance.Client.LatencyUpdated += DiscordHandler.Instance.ClientLatencyUpdated;
                services.GetRequiredService<CommandService> ().Log += DiscordHandler.Instance.LogAsync;

                await DiscordHandler.Instance.Client.LoginAsync ( TokenType.Bot, this.config["Token"] );
                await DiscordHandler.Instance.Client.StartAsync ();

                await services.GetRequiredService<CommandHandler> ().InitAsync ();
                await DiscordHandler.Instance.Client.SetGameAsync ( "Rumkyllinger I Rummet", "", ActivityType.Listening );

                await Task.Delay ( -1 );
            }
        }

        private ServiceProvider ConfigureServices ()
        {
            return new ServiceCollection ().AddSingleton ( this.config )
                .AddSingleton<DiscordSocketClient> ()
                .AddSingleton<CommandService> ()
                .AddSingleton<CommandHandler> ()
                .BuildServiceProvider ();

        }
    }
}