using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace DiscordBot.OS.System.Json
{
    /// <summary>
    /// Decode Json files into C# class objects
    /// </summary>
    public static class JDecoder
    {
        /// <summary>
        /// Decode a Json file into a C# class object
        /// </summary>
        /// <typeparam name="T">The C# object</typeparam>
        /// <param name="_filename">The name of the file including the filetype</param>
        /// <param name="_settings">How the Decoder should behave when decoding the Json file</param>
        /// <returns></returns>
        public static T DecodeFromFile<T> ( string _filename, JsonSerializerSettings _settings )
        {
            //  The path to the FacebookConfig.Json file
            string path = Path.GetDirectoryName ( Assembly.GetExecutingAssembly ().Location ) + @"\Data\" + _filename;

            //  THe file content
            string JsonFile;
            using (StreamReader reader = new StreamReader ( path ))
            {
                JsonFile = reader.ReadToEnd ();
            }

            //  Deserialize the Json object response into a C# class object.
            T obj = JsonConvert.DeserializeObject<T> ( JsonFile, _settings );

            return obj;
        }
    }
}
