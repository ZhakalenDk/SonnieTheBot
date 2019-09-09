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
            //  Get the list of commands

            var sb = new StringBuilder ();

            IUser user = Context.User;

            sb.AppendLine ( $"Det her kan jeg gøre, {user.Mention}{Environment.NewLine}Start alle kommandoer med ';'" );
            sb.AppendLine ( CommandHandler.cService.GetAllCommands () );

            await user.SendMessageAsync ( sb.ToString () );
            await ReplyAsync ( "Jeg sender dig en liste!" );
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