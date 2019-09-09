using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Data.Users
{
    /// <summary>
    /// Represents a Discord application user
    /// </summary>
    public class User
    {
        /// <summary>
        /// The ID of the user
        /// </summary>
        public ulong ID { get; private set; }
        /// <summary>
        /// THe custom name of the user
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The string that represents the users Mention tag in discord
        /// </summary>
        public string Mention { get; set; }
        /// <summary>
        /// THe nickname of the user, defined by discord
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ID">The ID of the user</param>
        /// <param name="_name">THe custom name for the user</param>
        public User ( ulong _ID, string _name )
        {
            ID = _ID;
            Name = _name;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ID">The ID of the user</param>
        /// <param name="_name">THe custom name for the user</param>
        /// <param name="_mention">THe Mention tag for the user. Defined by discord</param>
        public User ( ulong _ID, string _name, string _mention )
        {
            ID = _ID;
            Name = _name;
            Mention = _mention;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ID">The ID of the user</param>
        /// <param name="_name">THe custom name for the user</param>
        /// <param name="_mention">THe Mention tag for the user. Defined by discord</param>
        /// <param name="_Nickname">The nickname of the user, defined by discord</param>
        public User ( ulong _ID, string _name, string _mention, string _Nickname )
        {
            ID = _ID;
            Name = _name;
            Mention = _mention;
            Nickname = _Nickname;
        }
        /// <summary>
        /// Set a new custom name
        /// </summary>
        /// <param name="_name">THe new name ot use</param>
        public void SetName ( string _name )
        {
            this.Name = _name;
        }

        /// <summary>
        /// Extract the ID from a mention tag
        /// </summary>
        /// <param name="_mention">THe mention tag</param>
        /// <returns></returns>
        public static ulong GetIDFromMention ( string _mention )
        {
            //  Check which prefix to split by
            char prefix = ((_mention.Contains ( "!" )) ? ('!') : ('@'));

            ulong ID = ulong.Parse ( _mention.Split ( prefix )[1].Split ( ">" )[0] );

            return ID;
        }

        public override string ToString ()
        {
            return $"{Name}, [{ID}], {Mention}, {Nickname}";
        }
    }
}