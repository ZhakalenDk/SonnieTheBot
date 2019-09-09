using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscordBot.OS.Extensions
{
    public static class DiscordCommandServiceExtensions
    {
        /// <summary>
        /// Get all the available commands
        /// </summary>
        /// <param name="_service"></param>
        /// <returns></returns>
        public static string GetAllCommands ( this CommandService _service )
        {
            StringBuilder sb = new StringBuilder ();

            //  The list of commands
            List<CommandInfo> commandInfo = _service.Commands.ToList ();

            //  Loop trough each commands and build a line with the information
            foreach (CommandInfo command in commandInfo)
            {
                sb.AppendLine ( $"```{command.Name} - {command.Summary}```" );
            }

            return sb.ToString ();
        }
    }
}
