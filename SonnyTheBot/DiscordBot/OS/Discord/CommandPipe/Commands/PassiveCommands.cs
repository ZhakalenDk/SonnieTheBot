using System;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using DiscordBot.OS.Extensions;

namespace DiscordBot.OS.Discord.CommandPipe.Commands
{
    /// <summary>
    /// All command that does not requere any input or admin priviliges
    /// </summary>
    public class PassiveCommands : ModuleBase
    {
        [Command ( "hjælp" )]
        [Name ( "Hjælp" )]
        [Summary ( "Sender dig en liste med all kommandoer\nExample: ;hjælp" )]
        public async Task Help ()
        {
            await ReplyAsync ( "Jeg sender dig en liste!" );

            var sb = new StringBuilder ();
            IGuildUser user = Context.User as IGuildUser;
            await user.SendMessageAsync ( $"Det her kan jeg gøre:" );

            foreach ( string command in CommandHandler.cService.GetCommandsAsString () )
            {
                string cmdString = command;
                //  If the command is an adming-command but the user is not an admin. Don't include the command
                if ( command.ToLower ().Contains ( "admin" ) && !user.IsAdmin () )
                {
                    cmdString = string.Empty;
                }

                /*
                    If the current commands character length does not exceed the capacity of 2,000 characters.
                    If it exceeds the capacity, post the current build string and clear the string builder.
                    The proceed to build a new string with the remaining commands
                */
                if ( sb.Length + cmdString.Length <= 2000 )
                {
                    sb.Append ( cmdString );
                }
                else
                {
                    await user.SendMessageAsync ( sb.ToString () );
                    sb.Clear ();
                    sb.Append ( cmdString );
                }
            }

            await user.SendMessageAsync ( sb.ToString () );
        }

        [Command ( "Credits" )]
        [Name ( "Credits" )]
        [Summary ( "Giver en liste over dem der har hjulpet med at lave Sonnie\nExample: ;credits" )]
        public async Task Credits ()
        {
            var sb = new StringBuilder ();

            sb.AppendLine ( $"```{Environment.NewLine}Mike{Environment.NewLine}Christian{Environment.NewLine}Jannik```" );

            await ReplyAsync ( sb.ToString () );
        }

        [Command ( "Version" )]
        [Name ( "Version" )]
        [Summary ( "Fortæller hvilket version af Sonnie der kører\nExample: ;version" )]
        public async Task VControl ()
        {
            await Context.Channel.SendMessageAsync ( $"Sonnie kører: v{Program.VERSION}" );
        }
    }
}