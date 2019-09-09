using Discord;
using Discord.WebSocket;
using DiscordBot.OS.Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.OS.Extensions
{
    public static class DiscordSocketUserExtensions
    {
        /// <summary>
        /// Get the discord channel that is attached to the Facebook WebHook
        /// </summary>
        /// <param name="_discordClient">THe discord client</param>
        /// <returns></returns>
        public static async Task<IMessageChannel> GetFacebookFeedChannel ( this IDiscordClient _discordClient )
        {
            //  THe channel to post facebook-posts in
            IMessageChannel channel = await _discordClient.GetChannelAsync ( ulong.Parse ( FacebookHook.FacebookHandler.Instance.FacebookFeedChannelID ) ) as IMessageChannel;

            return channel;
        }

        /// <summary>
        /// Get the Discord channel that is attached to the Suggestion command
        /// </summary>
        /// <param name="_discordClient">THe Discord application client</param>
        /// <returns></returns>
        public static async Task<IMessageChannel> GetSuggestionChannel ( this IDiscordClient _discordClient )
        {
            ISocketMessageChannel channel = await _discordClient.GetChannelAsync ( ulong.Parse ( DiscordHandler.Instance.SuggestionChannelID ) ) as ISocketMessageChannel;
            return channel;
        }
    }
}
