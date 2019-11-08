using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Data.Users;
using DiscordBot.OS.Extensions;
using DiscordBot.OS.System;
using DiscordBot.Data.Events;
using DiscordBot.OS.System.Time;
using DiscordBot.OS.System.Time.Extensions;
using System;
using System.Threading.Tasks;

namespace DiscordBot.OS.Discord.CommandPipe.Commands
{
    /// <summary>
    /// All commands that require admin priviliges
    /// </summary>
    public class AdminCommands : ModuleBase
    {
        [Command ( "Save" )]
        [Name ( "Save" )]
        [Summary ( "Gennemtving en 'Gem' handling på alle nuværende buffered brugere og events. NOTE: Kan kun udføres af en admin.\nExample: ;save" )]
        public async Task Save ()
        {
            //  THh user to chekc for priviliges
            IGuildUser user = Context.User as IGuildUser;

            //If the user is an admin save all usrs to the STB.UP file
            if ( user.IsAdmin () )
            {
                Debug.Log.Message ( "AdminCommands - Force-Saving User and Event Profiles" );
                DataScanner<User> scanner = new DataScanner<User> ( @"\Data\Users\STB.UP" );
                DataScanner<Event> eventScanner = new DataScanner<Event> ( @"\Data\Events\STB.EP" );
                await scanner.WriteToFile ( UserManager.GetUserList () );
                await eventScanner.WriteToFile ( EventManager.Events );
                await Context.Channel.SendMessageAsync ( $"Jeg har skrevet alle navne og events ned, {Context.User.Mention}" );
                return;
            }

            await Context.Channel.SendMessageAsync ( "Ups... Du er vidst ikke admin" );
        }

        [Command ( "Kill" )]
        [Name ( "Kill" )]
        [Summary ( "Gennemtving lukning a Sonnie. NOTE: Kan kun udføres af en admin.\nExample: ;kill" )]
        public async Task Kill ()
        {
            //  THe user to check for priviliges
            IGuildUser user = Context.User as IGuildUser;

            //  If the user is an admin Log Sonnie off and kill the program
            if ( user.IsAdmin () )
            {

                Debug.Log.Message ( "AdminCommands - Trying to log off" );

                //  Get the application client
                DiscordSocketClient client = Context.Client as DiscordSocketClient;
                await Context.Channel.SendMessageAsync ( "Jamen vi ses så!" );

                //  Save the list of users to the STB.UP file
                DataScanner<User> scanner = new DataScanner<User> ( @"\Data\Users\STB.UP" );
                await scanner.WriteToFile ( UserManager.GetUserList () );

                await client.LogoutAsync ();

                //  Kill the program
                Environment.Exit ( -1 );
            }

            await Context.Channel.SendMessageAsync ( "Ups... Du er vidst ikke admin" );
        }

        [Command ( "Ferie" )]
        [Name ( "Ferie [D/M/Å/T.M-D/M/Å/T.M], Ferie [?], Ferie [Annuller]" )]
        [Summary ( "Sæt Sonnie til ferie-mode. Det vil gøre at Sonnie ikke checker for porno. NOTE: Kan kun udføres af en admin.\nExample: ;ferie [2/3/2019/12.30-5/3/2019/12.30]\nEksemplet vil sætte Sonnie i feriemode fra perioden D. 2.Marts.2019 kl.12.30 til D. 5.Marts.2019 Kl. 12.30\n\nSpørg Sonnie om han er på ferie, og i så fald, hvilken periode.\nExample: ;ferie ?\n\nAnnuller den nuværende ferie.\nExample: ;ferie annuller" )]
        public async Task VacationMode ( string _value )
        {
            //  The user to check for priviliges
            IGuildUser user = Context.User as IGuildUser;
            // Scanner for reading and writing from/to file
            DataScanner<Vacation> scanner = new DataScanner<Vacation> ( @"\Data\Events\STB.VP" );

            //  If the user is an admin set Sonnie in vacation mode
            if ( user.IsAdmin () )
            {
                if ( _value == "?" )
                {
                    string vacString = ( ( DiscordHandler.Instance.Vacation.ToString () == null ) ? ( "Jeg er ikke på ferie!" ) : ( $"Jeg er på ferie fra: {DiscordHandler.Instance.Vacation.ToString ()}" ) );
                    await Context.Channel.SendMessageAsync ( vacString );
                    return;
                }

                if ( _value.ToLower () == "annuller" )
                {
                    await Context.Channel.SendMessageAsync ( "Ferien er annulleret" );
                    DiscordHandler.Instance.Vacation = Vacation.SetInvalidVacation ();
                    await scanner.WriteToFile ( string.Empty );

                    return;
                }

                DiscordHandler.Instance.Vacation = Vacation.Parse ( _value );
                await Context.Channel.SendMessageAsync ( $"Jeg går på ferie fra: {DiscordHandler.Instance.Vacation.ToString ()}" );
                await scanner.WriteToFile ( _value );
                return;

            }

            await Context.Channel.SendMessageAsync ( "Ups... Du er vidst ikke admin" );

        }

        [Command ( "Bruger er" )]
        [Name ( "Bruger er [ID] [Navn]" )]
        [Summary ( "Gem en bruger i Sonnies database. NOTE: Kan kun udføres af en admin.\nExample: ;bruger er [615574635110465547] \"Sonnie Eis\"" )]
        public async Task UserIs ( string _ID, string _name )
        {
            //  The user to chekc for priviliges
            IGuildUser user = Context.User as IGuildUser;

            if ( user.IsAdmin () )
            {
                //  Check if the user is already in the list
                User dataUser = UserManager.SearchUserByID ( ulong.Parse ( _ID ) );

                //  If the user is not in the list, create a new user object and add the user to the list
                if ( dataUser == null )
                {
                    //  Get the server
                    IGuild server = Context.Guild as IGuild;

                    //  Get the user based on the ID input
                    IGuildUser entryUser = await server.GetUserAsync ( ulong.Parse ( _ID ) ) as IGuildUser;

                    //  Create the user object based on the input value
                    User newEntry = new User ( entryUser.Id, _name, entryUser.Mention, entryUser.Nickname, "false" );

                    //  Add the user to the list
                    UserManager.AddUser ( newEntry );

                    //  Save all users to the STB.UP file
                    DataScanner<User> scanner = new DataScanner<User> ( @"\Data\Users\STB.UP" );
                    await scanner.WriteToFile ( UserManager.GetUserList () );

                    await Context.Channel.SendMessageAsync ( $"Jeg tilføjer {_name}, som {entryUser.Mention}." );
                    return;
                }
                //  If the user is in the list, just replace the name
                dataUser.SetName ( _name );
                await Context.Channel.SendMessageAsync ( $"Jeg tilføjer {_name}, som {dataUser.Mention}." );
                return;

            }
            await Context.Channel.SendMessageAsync ( "Ups... Du er vidst ikke admin" );
        }

        [Command ( "Event" )]
        [Name ( "Event [Navn], [D/M/Å/T-D/M/Å/T], [Beskrivelse], [Andre Informationer], [Påmind (Optional) D/M/Å/T], Event [?], Event [Annuller [Event Navn]]" )]
        [Summary ( "Få Sonnie til at holde styr på et event. NOTE: Kan kun udføres af en admin.\nExample: ;event \"Mit Event\" 09/10/2019/18.30-09/10/2019/20.30 \"Beskrivelse, bla bla. Det her forklare mit event.\" \"Mere info om mit event. Det finder sted på adressen Bla bla blabla. Og medbring Blabla bla\" 09/10/2019/18.15\n\nFå vist en besked med alle nuværende events.\nExample: ;event ?\n\nAnnuller et event\nExample: ;event annuller \"Mit Event\"" )]
        public async Task PlanEvent ( string _name, string _timeSpan = null, string _description = null, string _otherInformation = null, string _prompt = null )
        {
            //  Return a list with all currently stored events
            if ( _name == "?" )
            {
                if ( EventManager.Events.Count != 0 && EventManager.Events != null )
                {
                    string eventList = string.Empty;
                    foreach ( Event item in EventManager.Events )
                    {
                        eventList += item.Print ();
                    }

                    await Context.Message.Author.SendMessageAsync ( eventList );

                    return;
                }
                await Context.Channel.SendMessageAsync ( "Der er ingen events!" );
                return;
            }

            IGuildUser user = Context.User as IGuildUser;

            if ( user.IsAdmin () )
            {
                //  Cancel an event
                if ( _name.ToLower () == "annuller" )
                {
                    string eventName = _timeSpan.ToLower ();
                    EventManager.Events.Remove ( EventManager.Events.Find ( _event => _event.Name.ToLower () == eventName ) );
                    DataScanner<Event> scanner2 = new DataScanner<Event> ( @"\Data\Events\STB.EP" );
                    await scanner2.WriteToFile ( EventManager.Events );
                    await Context.Channel.SendMessageAsync ( "Oh... Det event er annulleret" );
                    return;
                }

                //  Extract the requered data from the passed string
                string [] rawDates = _timeSpan.Split ( '-' );
                DateTime startDate = new DateTime ().Parse ( rawDates [ 0 ] );
                DateTime endDate = new DateTime ().Parse ( rawDates [ 1 ] );
                DateTime prompt = new DateTime ().Parse ( _prompt );

                //  Search for the auther of this event in the stored users and use his/her real name. If not found use the discord username instead
                string auther = ( UserManager.SearchUserByID ( Context.Message.Author.Id ).Name ?? Context.Message.Author.Username );
                Event newEvent = new Event ( _name, startDate, endDate, _description, _otherInformation, prompt, auther );
                EventManager.Events.Add ( newEvent );

                //  Save data to file
                DataScanner<Event> scanner = new DataScanner<Event> ( @"\Data\Events\STB.EP" );
                await scanner.WriteToFile ( EventManager.Events );

                await Context.Channel.SendMessageAsync ( $"Nyt event. Spændende!" );
            }
        }

        [Command ( "Har fri" )]
        [Name ( "Har fri [Navn]" )]
        [Summary ( "Fortæl Sonnie at en bruger har fri. NOTE: Kan kun udføres af en admin.\nExample: ;har fri \"Sonnie Eis\"" )]
        public async Task SetStatus ( string _name, string _status )
        {
            //  The user to set the status for
            User userToSet = UserManager.SearchByMention ( _name );

            //  Set the status to either true or false
            userToSet.Free = ( ( _status.ToLower () == "true" ) ? ( true ) : ( false ) );

            await Context.Channel.SendMessageAsync ( $"Det er modtaget, {Context.User.Mention}" );
        }
    }
}
