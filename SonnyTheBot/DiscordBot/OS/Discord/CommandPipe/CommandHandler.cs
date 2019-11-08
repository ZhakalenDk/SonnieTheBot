using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using DiscordBot.Data.Users;
using DiscordBot.OS.System;
using DiscordBot.OS.System.Time;

namespace DiscordBot.OS.Discord.CommandPipe
{
    /// <summary>
    /// Represents a Discord commandservice handler
    /// </summary>
    class CommandHandler
    {
        /// <summary>
        /// A global entry of the Discord WebAPI command service
        /// </summary>
        public static CommandService cService;
        public readonly IConfiguration config;
        /// <summary>
        /// The Discord WebAPI command service
        /// </summary>
        public readonly CommandService command;
        /// <summary>
        /// THe Discord Application Client
        /// </summary>
        public readonly DiscordSocketClient client;
        public readonly IServiceProvider services;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_provider"></param>
        public CommandHandler ( IServiceProvider _provider )
        {
            this.config = _provider.GetRequiredService<IConfiguration> ();
            this.command = _provider.GetRequiredService<CommandService> ();
            this.client = _provider.GetRequiredService<DiscordSocketClient> ();
            this.services = _provider;

            this.command.CommandExecuted += ExecuteCommandAsync;
            this.client.MessageReceived += MessageRecievedAsync;
        }

        /// <summary>
        /// Register modules that are public and inherit ModuleBase<T>
        /// </summary>
        /// <returns></returns>
        public async Task InitAsync ()
        {
            await this.command.AddModulesAsync ( Assembly.GetEntryAssembly (), this.services );
            cService = this.command;
        }
        public async Task MessageRecievedAsync ( SocketMessage _rawMessage )
        {
            //Debug.Log.Message ( _rawMessage.ToString () );

            //  Make sure the message is of the right type and the message was from a user and not a bot
            if ( !( _rawMessage is SocketUserMessage message ) || message.Source != MessageSource.User )
            {
                return;
            }

            //  The default Activity for the bot
            await this.client.SetGameAsync ( "Rumkyllinger I Rummet", "", ActivityType.Listening );

            //  If the word network or netværk is in a message
            #region Network is for Losers check
            if ( message.Content.ToLower ().Contains ( "network" ) || message.Content.ToLower ().Contains ( "netværk" ) )
            {
                await message.Channel.SendMessageAsync ( $"Netværk er for tabere, {message.Author.Mention}!" );

                //  The server the bot is connected to
                var server = this.client.GetGuild ( 614042459957362698 );
                Debug.Log.Message ( server.Name.ToString () );

                #region If user is not in collection add the user
                var loser = server.GetUser ( message.Author.Id );
                User rawUser = UserManager.SearchUserByID ( loser.Id );
                if ( rawUser == null )
                {
                    User user = new User ( loser.Id, "Ivan", loser.Mention, loser.Nickname );
                    DataScanner<User> scanner = new DataScanner<User> ( @"\Data\Users\STB.UP" );
                    await scanner.WriteToFile ( UserManager.GetUserList () );
                    UserManager.AddUser ( user );
                }
                else
                {
                    rawUser.Nickname = loser.Nickname;
                }
                #endregion
                #region If user is not an admin chance nickname to "Taber"
                foreach ( var role in loser.Roles )
                {
                    if ( role == server.GetRole ( 614097428756299784 ) )
                    {
                        return;
                    }
                }

                await loser.ModifyAsync ( user => user.Nickname = "Taber" );
                #endregion
                return;
            }
            #endregion

            #region Programming is the way forward check
            if ( message.Content.ToLower ().Contains ( "programmering" ) || message.Content.ToLower ().Contains ( "code" ) )
            {
                await message.Channel.SendMessageAsync ( $"Programmering er vejen frem, {message.Author.Mention}!" );
                return;
            }
            #endregion

            #region Say Sorry to the Bot
            if ( message.Content.ToLower ().Contains ( ( $"undskyld sonnie" ) ) )
            {
                var server = this.client.GetGuild ( 614042459957362698 );
                Debug.Log.Message ( server.Name.ToString () );
                var loser = server.GetUser ( message.Author.Id );
                User rawUser = UserManager.SearchUserByID ( loser.Id );

                #region If the User is not an admin
                foreach ( var role in loser.Roles )
                {
                    if ( role == server.GetRole ( 614097428756299784 ) )
                    {
                        await message.Channel.SendMessageAsync ( $"Du skal da ikke undskylde for noget, {message.Author.Mention}" );
                        return;
                    }
                }
                #endregion

                #region Give the user his Nickname back
                if ( rawUser != null )
                {
                    await loser.ModifyAsync ( user => user.Nickname = rawUser.Nickname );
                    await message.Channel.SendMessageAsync ( $"Det er så okay, {message.Author.Mention}!" );
                }
                #endregion
            }
            #endregion

            #region Citater

            #region Diktatur
            if ( message.Content.ToLower () == "hvad er det her?" )
            {
                await message.Channel.SendMessageAsync ( $"Det her er et diktatur, {message.Author.Mention}. Og diktatoren må skide i hjørnerne!" );
            }
            #endregion

            #region FuckDeAndreNiveau
            if ( message.Content.ToLower () == "hvad arbejder vi ud fra?" )
            {
                await message.Channel.SendMessageAsync ( $"Vi arbejder ud fra \"Fuck De Andre Niveau\", {message.Author.Mention}! " );
            }
            #endregion

            #endregion

            var argPos = 0;
            char prefix = char.Parse ( this.config [ "Prefix" ] );

            if ( !( message.HasMentionPrefix ( this.client.CurrentUser, ref argPos ) || message.HasCharPrefix ( prefix, ref argPos ) ) )
            {
                return;
            }

            var context = new SocketCommandContext ( this.client, message );

            await this.command.ExecuteAsync ( context, argPos, this.services );
        }

        /// <summary>
        /// Execute a given command
        /// </summary>
        /// <param name="_command">The command to execute</param>
        /// <param name="_context">The context in which the command has been raised</param>
        /// <param name="_result">The result of the commad execution</param>
        /// <returns></returns>
        public async Task ExecuteCommandAsync ( Optional<CommandInfo> _command, ICommandContext _context, IResult _result )
        {
            if ( !_command.IsSpecified )
            {
                Debug.Log.Message ( $"CommandHandler - Failed to execute for [{_command.Value.Name}] <-> [{_context.User}]!" );
                return;
            }

            if ( _result.IsSuccess )
            {
                Debug.Log.Message ( $"CommandHandler - Executed [{_command.Value.Name}] for -> [{_context.User}]" );
                return;
            }

            await _context.Channel.SendMessageAsync ( $"Ups... Noget gik galt... -> [{_context.User.Mention}]!" );
        }
    }
}
