using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.OS.System.Time
{
    /// <summary>
    /// Represents a time stamp
    /// </summary>
    public struct Time
    {
        /// <summary>
        /// The hour component of this instance
        /// </summary>
        public readonly int hours;
        /// <summary>
        /// THe minute component of this instance
        /// </summary>
        public readonly int minutes;
        /// <summary>
        /// THe second component of this instance
        /// </summary>
        public readonly int seconds;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_hour">Set the hour stamp</param>
        /// <param name="_minutes">Set the minute stamp</param>
        /// <param name="_seconds">Set the second stamp</param>
        public Time ( int _hour = 0, int _minutes = 0, int _seconds = 0 )
        {
            this.hours = _hour;
            this.minutes = _minutes;
            this.seconds = _seconds;
        }

        public override string ToString ()
        {
            return $"{this.hours}, {this.minutes}, {this.seconds}";
        }
    }
}
