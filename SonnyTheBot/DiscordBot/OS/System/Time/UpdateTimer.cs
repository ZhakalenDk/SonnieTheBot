using DiscordBot.OS.System.Time.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.OS.System.Time
{
    /// <summary>
    /// Represents a timer that resets itself everytime itøs triggered
    /// </summary>
    public class UpdateTimer
    {
        /// <summary>
        /// The name of this timer
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// The time between updates
        /// </summary>
        public Time Interval { get; set; }
        /// <summary>
        /// The current time
        /// </summary>
        private DateTime current;
        /// <summary>
        /// When the next trigger event should happen
        /// </summary>
        public DateTime UpdateTime { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_interval">HOw long, in hours, the timer should wait</param>
        public UpdateTimer ( string _name, Time _interval )
        {
            Name = _name;
            Interval = _interval;
            SetNewUpdate ();
        }

        /// <summary>
        /// Returns true if the current time exceeds the timer maxValue
        /// </summary>
        /// <returns></returns>
        public bool ReadyToUpdate ()
        {
            this.current = DateTime.Now;

            if (this.current >= this.UpdateTime)
            {
                SetNewUpdate ();
                return true;
            }

            return false;
        }

        /// <summary>
        /// >Set when the update should happen next
        /// </summary>
        private void SetNewUpdate ()
        {
            /*int hours = ((DateTime.Now.Hour + Interval.hours > 23) ? ((DateTime.Now.Hour + Interval.hours) - 23) : (DateTime.Now.Hour + Interval.hours));

            int minutes = ((DateTime.Now.Minute + Interval.minutes > 59) ? ((DateTime.Now.Minute + Interval.minutes) - 59) : (DateTime.Now.Minute + Interval.minutes));
            int seconds = ((DateTime.Now.Second + Interval.seconds > 59) ? ((DateTime.Now.Second + Interval.seconds) - 59) : (DateTime.Now.Second + Interval.hours));*/
            this.UpdateTime = DateTime.Now.AddTime ( Interval );
            Debug.Log.Message ( $"{Name} - Setting new Update schedule: {this.UpdateTime.ToString ()}" );
        }
    }
}
