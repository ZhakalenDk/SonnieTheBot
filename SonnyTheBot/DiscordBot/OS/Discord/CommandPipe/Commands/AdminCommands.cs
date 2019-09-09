using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Data.Users;
using DiscordBot.OS.Extensions;
using DiscordBot.OS.System;
using DiscordBot.OS.System.Time;
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
        [Summary ( "Gennemtving en 'Gem' handling på alle nuværende buffered brugere. NOTE: Kan kun udføres af en admin.\nExample: ;save" )]
        public async Task SaveUsers ()
        {
            //  THh user to chekc for priviliges
            IGuildUser user = Context.User as IGuildUser;

            //If the user is an admin save all usrs to the STB.UP file
            if (user.IsAdmin ())
            {
                Debug.Log.Message ( "AdminCommands - Force-Saving User Profiles" );
                await DataScanner.WriteToFile ( UserManager.GetUserList () );
                await Context.Channel.SendMessageAsync ( $"Jeg har skrevet alle navne ned, {Context.User.Mention}" );
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
            if (user.IsAdmin ())
            {

                Debug.Log.Message ( "AdminCommands - Trying to log off" );

                //  Get the application client
                DiscordSocketClient client = Context.Client as DiscordSocketClient;
                await Context.Channel.SendMessageAsync ( "Jamen vi ses så!" );

                //  Save the list of users to the STB.UP file
                await DataScanner.WriteToFile ( UserManager.GetUserList () );

                await client.LogoutAsync ();

                //  Kill the program
                Environment.Exit ( -1 );
            }

            await Context.Channel.SendMessageAsync ( "Ups... Du er vidst ikke admin" );
        }

        [Command ( "Ferie" )]
        [Name ( "Ferie [D/M/Å/T-D/M/Å/T]" )]
        [Summary ( "Sæt Sonnie til ferie-mode. Det vil gøre at Sonnie ikke checker for porno.NOTE: Kan kun udføres af en admin.\nExample: ;ferie [2/3/2019/12-5/3/2019/12]\nEksemplet vil sætte Sonnie i feriemode fra perioden D. 2.Marts.2019 kl.12 til D. 5.Marts.2019 Kl. 12" )]
        public async Task VacationMode ( string _value )
        {
            //  The user to check for priviliges
            IGuildUser user = Context.User as IGuildUser;

            //  If the user is an admin set Sonnie in vacation mode
            if (user.IsAdmin ())
            {
                DiscordHandler.Instance.Vacation = Vacation.Parse ( _value );
                await Context.Channel.SendMessageAsync ( $"Jeg går på ferie fra: {DiscordHandler.Instance.Vacation.ToString ()}" );
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

            if (user.IsAdmin ())
            {
                //  Check if the user is already in the list
                User dataUser = UserManager.SearchUserByID ( ulong.Parse ( _ID ) );

                //  If the user is not in the list, create a new user object and add the user to the list
                if (dataUser == null)
                {
                    //  Get the server
                    IGuild server = Context.Guild as IGuild;

                    //  Get the user based on the ID input
                    IGuildUser entryUser = await server.GetUserAsync ( ulong.Parse ( _ID ) ) as IGuildUser;

                    //  Create the user object based on the input value
                    User newEntry = new User ( entryUser.Id, _name, entryUser.Mention, entryUser.Nickname );

                    //  Add the user to the list
                    UserManager.AddUser ( newEntry );

                    //  Save all users to the STB.UP file
                    await DataScanner.WriteToFile ( UserManager.GetUserList () );

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
    }
}
