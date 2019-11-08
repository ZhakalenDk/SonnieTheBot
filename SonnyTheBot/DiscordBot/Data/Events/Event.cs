using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Data.Events
{
    /// <summary>
    /// Represents an custom event
    /// </summary>
    public class Event
    {
        /// <summary>
        /// THe name of the event
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// THe description of the event
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Other ipmortant information about the event. For example where it will take place
        /// </summary>
        public string OtherInfo { get; set; }
        /// <summary>
        /// When the event will begin
        /// </summary>
        public DateTime From { get; set; }
        /// <summary>
        /// When the event will end
        /// </summary>
        public DateTime Until { get; set; }
        /// <summary>
        /// When should the users be notified about the event
        /// </summary>
        public DateTime Prompt { get; set; }

        /// <summary>
        /// THe name of the user, who raised the event
        /// </summary>
        public string Auther { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_name">Event Name</param>
        /// <param name="_from">When the event should begin</param>
        /// <param name="_until">When the event should end</param>
        /// <param name="_description">And explanation on the event</param>
        /// <param name="_otherInformation">IMportant information, such as where th event will take place</param>
        /// <param name="_prompt">When should user be notified of the event?</param>
        /// <param name="_auther">The user, who raised the event</param>
        public Event ( string _name, DateTime _from, DateTime _until, string _description, string _otherInformation, DateTime _prompt, string _auther )
        {
            Name = _name;
            From = _from;
            Until = _until;
            Description = _description;
            OtherInfo = _otherInformation;
            Prompt = _prompt;

            Auther = _auther;
        }

        /// <summary>
        /// Print a formatted string representation of this object
        /// </summary>
        /// <returns></returns>
        public string Print ()
        {
            return $"```Name: { Name}{ Environment.NewLine}Description: {Description}{ Environment.NewLine}From: {From.ToString ()}{ Environment.NewLine}Until: { Until.ToString ()}{ Environment.NewLine}Other Info: { OtherInfo}{ Environment.NewLine}Prompt: { Prompt.ToString ()}{ Environment.NewLine}Raised by: { Auther}```";
        }

        /// <summary>
        /// Returns a formatted string where each value of this instance is seperated by ':'
        /// </summary>
        /// <returns></returns>
        public override string ToString ()
        {
            return $"{Name}:{Description}:{OtherInfo}:{From.Day}/{From.Month}/{From.Year}/{From.Hour}.{From.Minute}:{Until.Day}/{Until.Month}/{Until.Year}/{Until.Hour}.{Until.Minute}:{Prompt.Day}/{Prompt.Month}/{Prompt.Year}/{Prompt.Hour}.{Prompt.Minute}:{Auther}";
        }
    }
}
