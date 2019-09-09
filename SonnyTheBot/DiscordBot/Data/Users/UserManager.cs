using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Data.Users
{
    public static class UserManager
    {
        private static List<User> users;

        public static User SearchUserByID ( ulong _ID )
        {
            User u = users.Find ( user => user.ID == _ID );

            if (u != null)
            {
                return u;
            }

            return null;
        }

        public static User SearchByName ( string _name )
        {
            User u = users.Find ( user => user.Name == _name );

            if (u != null)
            {
                return u;
            }

            return null;
        }

        public static User SearchByMention ( string _mention )
        {
            User u = users.Find ( user => user.Mention == _mention );
            if (u == null)
            {
                string[] newString = _mention.Split ( "@" );
                return users.Find ( user => user.Mention == $"{newString[0]}@!{newString[1]}" );
            }

            return u;
        }

        [System.Obsolete ( "Method should be updated to AddUser (User)", true )]
        public static void AddUser ( string _name, ulong _ID, string _nickname = null )
        {
            Debug.Log.Message ( $"UserManager - Adding User ({_name}, [{_ID}], {_nickname})" );
            users.Add ( new User ( _ID, _name, _nickname ) );
        }

        public static void AddUser ( User _user )
        {
            users.Add ( _user );
        }

        public static void SetUserList ( List<User> _userList )
        {
            users = _userList;
        }

        public static List<User> GetUserList ()
        {
            return users;
        }

    }
}
