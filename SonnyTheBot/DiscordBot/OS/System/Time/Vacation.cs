using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.OS.System.Time
{
    /// <summary>
    /// A specified period of time
    /// </summary>
    public class Vacation
    {
        /// <summary>
        /// The day the vacation will begin
        /// </summary>
        DateTime From { get; }
        /// <summary>
        /// The day the vacation will end
        /// </summary>
        DateTime Until { get; }

        /// <summary>
        /// Should only be set outside the class scope
        /// </summary>
        public bool WasOnVacation { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_from">The day the vacation will begin</param>
        /// <param name="_until">The day th vacation will end</param>
        public Vacation ( DateTime _from, DateTime _until )
        {
            From = _from;
            Until = _until;
        }

        /// <summary>
        /// Parse a formated string into a Vacation object (Format must be "D/M/Y/H.M-D/M/Y/H.M"
        ///  Example: "3/12/2019/12.30-7/12/2019/12.30")
        /// </summary>
        /// <param name="_value">The formated string value to parse</param>
        /// <returns></returns>
        public static Vacation Parse ( string _value )
        {
            /*2/3/2019/12.30-5/3/2019/12.30*/
            string [] period = _value.Split ( '-' );
            //Console.WriteLine ( $"[0]={period[0]} : [1]={period[1]}" );
            string [] startValues = period [ 0 ].Split ( '/' );
            string [] endValues = period [ 1 ].Split ( '/' );

            //  Extract the clock values
            string [] startClock = startValues [ 3 ].Split ( '.' );
            string [] endClock = endValues [ 3 ].Split ( '.' );

            DateTime start = new DateTime ( int.Parse ( startValues [ 2 ] ), int.Parse ( startValues [ 1 ] ), int.Parse ( startValues [ 0 ] ), int.Parse ( startClock [ 0 ] ), ( int.Parse ( startClock [ 1 ] ) ), 0 );
            DateTime end = new DateTime ( int.Parse ( endValues [ 2 ] ), int.Parse ( endValues [ 1 ] ), int.Parse ( endValues [ 0 ] ), int.Parse ( endClock [ 0 ] ), ( int.Parse ( endClock [ 1 ] ) ), 0 );

            return new Vacation ( start, end );
        }

        /// <summary>
        /// Sets the Vacation object to not being in vecation mode
        /// </summary>
        /// <returns></returns>
        public static Vacation SetInvalidVacation ()
        {
            return new Vacation ( DateTime.Now, new DateTime ( DateTime.Now.Day - 1 ) );
        }

        /// <summary>
        /// Returns true if the current date is within the vacation period
        /// </summary>
        /// <returns></returns>
        public bool OnVecation ()
        {
            if ( DateTime.Now >= From && DateTime.Now < Until )
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the string representation of this object. If the Vacation object i set to be invalid this will return null
        /// </summary>
        /// <returns></returns>
        public override string ToString ()
        {
            if ( From > Until )
            {
                return null;
            }
            return $"{From.ToString ()} - {Until.ToString ()}";
        }
    }
}
