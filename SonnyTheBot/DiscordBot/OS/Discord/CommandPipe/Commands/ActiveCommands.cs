using System;
using Discord.WebSocket;
using Discord.Commands;
using System.Threading.Tasks;
using DiscordBot.Data.Users;
using DiscordBot.OS.System;
using DiscordBot.OS.Extensions;
using Discord;

namespace DiscordBot.OS.Discord.CommandPipe.Commands
{
    /// <summary>
    /// All commands that require input form the user
    /// </summary>
    public class ActiveCommands : ModuleBase
    {
        [Command ( "Jeg er" )]
        [Name ( "Jeg er [Dit navn]" )]
        [Summary ( "Fortæl Sonnie hvem du er\nExample: ;Jeg er \"Sonnie Eis\"" )]
        public async Task AddMe ( [Summary ( "Your first name" )]string _name )
        {
            //  Search the list of users
            User user = UserManager.SearchUserByID ( Context.Message.Author.Id );

            //  If no user was found, create a new User object and add it to the list
            if ( user == null )
            {
                user = new User ( Context.User.Id, _name, Context.User.Mention );
                UserManager.AddUser ( user );
                Debug.Log.Message ( $"ActiveCommands[Jeg er] - Added {user.ToString ()}" );

            }
            else    //  If a user was found just replace the name
            {
                user.SetName ( _name );
            }

            //  Save all users to file
            DataScanner<User> scanner = new DataScanner<User> ( @"\Data\Users\STB.UP" );
            await scanner.WriteToFile ( UserManager.GetUserList () );
            await ReplyAsync ( $"Jeg tilføjer {_name}, som dit navn." );
        }

        [Command ( "Hvem er" )]
        [Name ( "Hvem er [Tag en bruger]" )]
        [Summary ( "Spørg Sonnie hvem en bruger er\nExample: ;Hvem er \"@Sonnie\"" )]
        public async Task WhoIs ( [Summary ( "The username of the user" )] string _mention )
        {
            // Search
            User user = UserManager.SearchByMention ( _mention );

            //  If the no user is found
            if ( user == null )
            {
                await ReplyAsync ( $"Jeg kender ikke \"{_mention}\"!" );
            }

            await ReplyAsync ( $"{user.Nickname} er: {user.Name}." );
        }

        [Command ( "Foreslå" )]
        [Name ( "Foreslå [Dit forslag]" )]
        [Summary ( "Foreslå noget til Sonnie\nExample: ;Foreslå \"Ku' det ikke være sjovt, hvis Sonnie ku' ændre folks navne?\"" )]
        public async Task Suggestion ( [Summary ( "Your suggestion" )] string _suggestion )
        {
            //  The Suggestion channel
            ISocketMessageChannel channel = await Context.Client.GetSuggestionChannel () as ISocketMessageChannel;

            await channel.SendMessageAsync ( $"Nyt forslag fra: {Context.User.Mention}{Environment.NewLine}```{_suggestion}```" );

            await ReplyAsync ( $"Tak for forslaet, {Context.Message.Author.Mention}" );
        }

        [Command ( "Ikke Tilstede" )]
        [Name ( "Ikke Tilstede [Tag en Bruger]" )]
        [Summary ( "Få Sonnie til at sende en besked til en bruger der ikke er tilstede\nExample: ;ikke tilstede @Sonnie" )]
        public async Task NotPresent ( [Summary ( "The username of the user" )] string _mention )
        {
            //  The user to send the message to
            IGuildUser user = await Context.Guild.GetUserAsync ( User.GetIDFromMention ( _mention ) );
            await Context.Channel.SendMessageAsync ( "Hmm. Det tjekker jeg lige op på!" );

            await user.SendMessageAsync ( "Hvorfor er du ikke i skole?" );
        }
    }
}
