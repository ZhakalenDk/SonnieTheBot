using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscordBot.OS.Extensions
{
    public static class DiscordCommandServiceExtensions
    {
        /// <summary>
        /// Get all the available commands as string representations
        /// </summary>
        /// <param name="_service"></param>
        /// <returns></returns>
        public static List<string> GetCommandsAsString ( this CommandService _service )
        {
            List<string> commandsList = new List<string> ();

            //  The list of commands
            List<CommandInfo> commandInfo = _service.Commands.ToList ();

            //  Loop trough each commands and build a line with the information
            foreach ( CommandInfo command in commandInfo )
            {
                commandsList.Add ( $"```{command.Name} - {command.Summary}```" );
            }

            return commandsList;
        }

        /// <summary>
        /// Get all commands
        /// </summary>
        /// <param name="_service"></param>
        /// <returns></returns>
        public static List<CommandInfo> GetAllCommands ( this CommandService _service )
        {
            return _service.Commands.ToList ();
        }
    }
}
