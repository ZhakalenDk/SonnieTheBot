using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.OS.Extensions
{
    public static class DiscordGuildUserExtensions
    {
        /// <summary>
        /// Returns true if the user is an admin
        /// </summary>
        /// <param name="_user"></param>
        /// <returns></returns>
        public static bool IsAdmin ( this IGuildUser _user )
        {
            //  Loop trough each role
            foreach ( ulong roleID in _user.RoleIds )
            {
                //  Return true if user is admin
                if ( roleID == 614097428756299784 )
                {
                    return true;
                }
            }

            return false;
        }
    }
}
