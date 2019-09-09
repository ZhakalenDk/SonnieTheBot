using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.OS.System.Time.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Check if the current date is in a weekend
        /// </summary>
        /// <param name="_dateTime">The date to check</param>
        /// <returns></returns>
        public static bool IsWeekend ( this DateTime _dateTime )
        {
            if (_dateTime.DayOfWeek == DayOfWeek.Saturday || _dateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the date is within a given timeperiod
        /// </summary>
        /// <param name="_dateTime">The current time</param>
        /// <param name="_from">The date that represents the beginning of the period</param>
        /// <param name="_to">THe date that represents the end of the period</param>
        /// <returns></returns>
        public static bool IsWithin ( this DateTime _dateTime, DateTime _from, DateTime _to )
        {
            //Debug.Log.Message ( $"({currentTime.ToString ()}) >= ({_from.ToString ()}) == ({(currentTime >= _from).ToString ()})" );
            //Debug.Log.Message ( $"({currentTime.ToString ()}) <= ({_from.ToString ()}) == ({(currentTime <= _to).ToString ()})" );

            if (_dateTime <= _to && _dateTime >= _from)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Add a given time to the DateTime Object
        /// </summary>
        /// <param name="_dateTime">The DateTime object</param>
        /// <param name="_time">The time to add</param>
        /// <returns></returns>
        public static DateTime AddTime ( this DateTime _dateTime, Time _time )
        {
            _dateTime = _dateTime.AddHours ( _time.hours );
            _dateTime = _dateTime.AddMinutes ( _time.minutes );
            _dateTime = _dateTime.AddSeconds ( _time.seconds );

            return _dateTime;
        }
    }
}
