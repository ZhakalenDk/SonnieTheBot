using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Data.Events
{
    public static class EventManager
    {
        public static List<Event> Events { get; private set; } = new List<Event> ();

        internal static void SetEventsList ( List<Event> _events )
        {
            Events = _events;
        }
    }
}
