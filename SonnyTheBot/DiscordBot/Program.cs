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
using System.Collections.Generic;
using DiscordBot.Data.Events;

namespace DiscordBot
{
    class Program
    {
        private readonly IConfiguration config;
        /// <summary>
        /// Version Control ([Major feaures].[Minor features].[Major fixes].[Minor fixes])
        /// </summary>
        public static string VERSION = "3.9.8.1";

        static void Main ( string [] args )
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

            #region Read users from file
            //  The path to the file to read from
            DataScanner<User> scanner = new DataScanner<User> ( @"\Data\Users\STB.UP" );
            List<User> users = new List<User> ();

            //  Add each user to the list of users
            foreach ( DataContainer item in scanner.ReadFromFile ( ':' ) )
            {
                users.Add ( new User ( ulong.Parse ( item [ 0 ] ), item [ 1 ], item [ 2 ], item [ 3 ], item [ 4 ] ) );
            }

            //  Set list of users
            UserManager.SetUserList ( users );
            #endregion

            #region Read events from file
            //  The path to the file to read from
            DataScanner<Event> eventScanner = new DataScanner<Event> ( @"\Data\Events\STB.EP" );
            List<Event> events = new List<Event> ();

            //  Add each event to the list of events
            foreach ( DataContainer item in eventScanner.ReadFromFile ( ':' ) )
            {
                events.Add ( new Event ( item [ 0 ], new DateTime ().Parse ( item [ 3 ] ), new DateTime ().Parse ( item [ 4 ] ), item [ 1 ], item [ 2 ], new DateTime ().Parse ( item [ 5 ] ), item [ 6 ] ) );
            }

            //  Set event list
            EventManager.SetEventsList ( events );
            #endregion

            #region Read Vacation
            DataScanner<Vacation> vacScanner = new DataScanner<Vacation> ( @"\Data\Events\STB.VP" );
            DataContainer? container = vacScanner.ReadFromFile ( 0 );
            DiscordHandler.Instance.Vacation = ( container != null ? Vacation.Parse ( container.Value [ 0 ] ) : Vacation.SetInvalidVacation () );
            #endregion
        }

        /// <summary>
        /// THe starting point of the bo execution
        /// </summary>
        /// <returns></returns>
        private async Task MainAsync ()
        {
            using ( var services = ConfigureServices () )
            {
                var rSClient = services.GetRequiredService<DiscordSocketClient> ();
                DiscordHandler.Instance.Client = rSClient;

                DiscordHandler.Instance.Client.Log += DiscordHandler.Instance.LogAsync;
                DiscordHandler.Instance.Client.Ready += DiscordHandler.Instance.ReadyAsync;
                DiscordHandler.Instance.Client.UserIsTyping += DiscordHandler.Instance.UserIsTyping;
                DiscordHandler.Instance.Client.LatencyUpdated += DiscordHandler.Instance.ClientLatencyUpdated;
                services.GetRequiredService<CommandService> ().Log += DiscordHandler.Instance.LogAsync;

                await DiscordHandler.Instance.Client.LoginAsync ( TokenType.Bot, this.config [ "Token" ] );
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