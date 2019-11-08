using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.OS.System
{
    /// <summary>
    /// Represents an array of string values
    /// </summary>
    public struct DataContainer
    {
        /// <summary>
        /// The values stored in the container
        /// </summary>
        string [] Values { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_values">The array of strings to store</param>
        public DataContainer ( string [] _values )
        {
            Values = _values;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_value">The string to store in the container</param>
        public DataContainer ( string _value )
        {
            Values = new string [ 1 ];
            Values [ 0 ] = _value;
        }

        /// <summary>
        /// Returns a stored container value
        /// </summary>
        /// <param name="index">The index of the value in the container</param>
        /// <returns></returns>
        public string this [ int index ]
        {
            get { return Values [ index ]; }
        }

    }
}
