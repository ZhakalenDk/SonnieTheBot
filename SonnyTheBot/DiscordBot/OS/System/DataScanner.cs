using DiscordBot.Data.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.OS.System
{
    /// <summary>
    /// Represents an object to Write/Read from an UTP.UP file
    /// </summary>
    public static class DataScanner
    {
        /// <summary>
        /// The path to the STB.UP file
        /// </summary>
        private static readonly string path = Path.GetDirectoryName ( Assembly.GetExecutingAssembly ().Location ) + @"\Data\Users\STB.UP";

        /// <summary>
        /// Save all users to the STB.UP file
        /// </summary>
        /// <param name="_users">The list of users</param>
        /// <returns></returns>
        public static async Task WriteToFile ( List<User> _users )
        {
            Debug.Log.Message ( "DataScanner - Writing user data to file" );
            string fileContent = string.Empty;

            //  Loop trough the list of users
            foreach (User user in _users)
            {
                //  Build the content string to write to the file
                fileContent += $"{user.ID}:{user.Name}:{user.Mention}:{user.Nickname}\n";
            }

            //  Write all data to file
            await File.WriteAllTextAsync ( path, fileContent );

        }

        /// <summary>
        /// Collect every user from the STB.UP file
        /// </summary>
        /// <returns></returns>
        public static List<User> ReadFromFile ()
        {
            //  File content
            string file;

            //  The list of users to return
            List<User> users = new List<User> ();
            Debug.Log.Message ( "DataScanner - Reading Users from file" );

            //  Read all data from the STB.UP file
            using (StreamReader reader = new StreamReader ( path ))
            {
                file = reader.ReadToEndAsync ().Result;
            }

            //  Split up the content string by lines
            string[] fileLines = file.Split ( "\n" );

            //  Loop trough the content lines
            foreach (string line in fileLines)
            {
                //  If a line is empty break out of the loop
                if (line.Length == 0)
                {
                    break;
                }

                //  Split the string up into the appropriate values
                string[] userValues = line.Split ( ':' );

                //  Build the user object
                User user = new User ( ulong.Parse ( userValues[0] ), userValues[1], userValues[2], userValues[3] );

                Debug.Log.Message ( $"DataScanner - Reading ({user})" );

                //  Add the user to the user list
                users.Add ( user );
            }

            return users;
        }
    }
}
