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
    public class DataScanner<T>
    {
        /// <summary>
        /// The path to the specific file
        /// </summary>
        private readonly string path;

        /// <summary>
        /// Write a range of data objects to file
        /// </summary>
        /// <param name="_list">The list of objects to write</param>
        /// <returns></returns>
        public async Task WriteToFile ( List<T> _list )
        {
            Debug.Log.Message ( "DataScanner - Writing data to file" );
            string fileContent = string.Empty;

            //  Loop trough the list of users
            foreach ( T item in _list )
            {
                //  Build the content string to write to the file
                fileContent += $"{item.ToString ()}\n";
            }

            //  Write all data to file
            await File.WriteAllTextAsync ( this.path, fileContent );

        }

        /// <summary>
        /// Write a single string value to a file
        /// </summary>
        /// <param name="_value">The stirng value to write</param>
        /// <returns></returns>
        public async Task WriteToFile ( string _value )
        {
            Debug.Log.Message ( "DataScanner - Writing data to file" );
            string fileContent = string.Empty;
            //  Build the content string to write to the file
            fileContent += $"{_value}\n";

            //  Write all data to file
            await File.WriteAllTextAsync ( this.path, fileContent );
        }

        /// <summary>
        /// Collect data from file and seperate it by ':'
        /// </summary>
        /// <returns></returns>
        public List<DataContainer> ReadFromFile ( char _breakBy )
        {
            //  THe list to return
            string [] lineValues = null;
            List<DataContainer> data = new List<DataContainer> ();

            //  File content
            string file;

            Debug.Log.Message ( "DataScanner - Reading items from file" );

            //  Read all data from the STB.UP file
            using ( StreamReader reader = new StreamReader ( this.path ) )
            {
                file = reader.ReadToEndAsync ().Result;
            }

            //  Split up the content string by lines
            string [] fileLines = file.Split ( "\n" );

            //  Loop trough the content lines
            foreach ( string line in fileLines )
            {
                //  If a line is empty break out of the loop
                if ( line.Length == 0 )
                {
                    break;
                }

                Debug.Log.Message ( $"DataScanner - Reading ({line}) breaking by ({_breakBy})" );

                //  Split the string up into the appropriate values
                lineValues = line.Split ( _breakBy );
                data.Add ( new DataContainer ( lineValues ) );
            }

            return data;
        }

        /// <summary>
        /// Read a single line from a file
        /// </summary>
        /// <param name="_LineNumber">Which line to read from file</param>
        /// <returns></returns>
        public DataContainer? ReadFromFile ( int _LineNumber )
        {

            //  The value to return
            DataContainer? data = null;

            //  File content
            string file;

            Debug.Log.Message ( "DataScanner - Reading item from file" );

            //  Read all data from the STB file
            using ( StreamReader reader = new StreamReader ( this.path ) )
            {
                file = reader.ReadToEndAsync ().Result;
            }

            string [] fileLines = file.Split ( '\n' );

            if ( fileLines [ 0 ].Length > 0 )
            {
                data = new DataContainer ( fileLines [ _LineNumber ] );
                Debug.Log.Message ( $"DataScanner - Reading ({data.Value [ 0 ]})" );
            }

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_path">The path to the file that contains the data</param>
        public DataScanner ( string _path )
        {
            this.path = Path.GetDirectoryName ( Assembly.GetExecutingAssembly ().Location ) + _path;
        }
    }
}
