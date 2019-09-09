using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Debug
{
    /// <summary>
    /// Represents a log with date and time
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Print a message to the console with a date and time stamp
        /// </summary>
        /// <param name="_message"></param>
        public static void Message ( string _message )
        {
            Console.WriteLine ( $"({DateTime.Now}): {_message}" );
        }
    }
}
