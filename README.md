# Sonnie The Bot
_Sonnie is a Danish Bot, so all his commands and his responses are in Danish._

## History
Sonnie is a bot, written to connect Facebook and Discord with eachtoher.
He's based on a teacher from a Danish Education; Data. & Communication, GF2.

Because he's based on a teacher he has some features directly linked to the teacher (_In a way his behavior is related to the teachers_).
This means that Sonnie has some hidden "_commands_" that triggers when writing specific words or sentences.
In the beginning he was supposed to only link Facebook and Discord, as we had some people in the class, who was not on Facebook. Now he has a lot more features than that.

## Features

###### Facebook Link
Sonnie can collect posts from a facebook group page and post them in a channel on discord. The supported data includes comments and pictures. (_As of now Sonnie only supports single pictures. Not Picture arrays_), along with embedded links. However, he can not post to Facebook or collect the names of the members in the group.

###### User Storage
Sonnie can store the names, ID and nicknames and usernames of all the users in the associated Discord Server.
This feature was ment to identify the class members on the Discord.
You can therefor ask Sonnie who a specific user is by tagging them and Sonnie will look trough his database and tell you the real name of that user.

In order for Sonnie to do this he needs to know the user. So if a user is not in the database they will be prompted to add themselves into the database, everytime they write somehing in a channel.

###### Check For Porno
Porno is a term used to describe anything that is not related to the current teaching in class.
If a user is online on Discord and the users activity status is changed to _Gaming_ or _Watching_, Sonnie will tell the user to stop wacthing porn.

###### Vacation Mode (See [Commands](https://github.com/ZhakalenDk/SonnieTheBot/blob/master/README.md#commands))
Sonnie can be commanded to go into Vacation mode.
While in vacation mode, Sonnie want check for porno.

###### Commands
Commands are executed in channel chat inside the Client application.
Sonnie identifies a command by looking for the prefix defined by _"[Prefix](https://github.com/ZhakalenDk/SonnieTheBot/blob/master/SonnyTheBot/DiscordBot/bin/Debug/netcoreapp2.1/config.json)": ";"_.

Here's a list of the curretly implemented commands:
- **Jeg er [Dit navn] - Fortæl Sonnie hvem du er. _Example: ;Jeg er "Sonnie Hansen"_**

- **Hvem er [Tag en bruger] - Spørg Sonnie hvem en bruger er. _Example: ;Hvem er "@Sonnie"_**

- **Foreslå [Dit forslag] - Foreslå noget til Sonnie. _Example: ;Foreslå "Ku' det ikke være sjovt, hvis Sonnie ku' ændre folks navne?"_**

- **Ikke Tilstede [Tag en Bruger] - Få Sonnie til at sende en besked til en bruger der ikke er tilstede. _Example: ;ikke tilstede @Sonnie_**

- **Save - Gennemtving en 'Gem' handling på alle nuværende buffered brugere. NOTE: Kan kun udføres af en admin. _Example: ;save_**

- **Kill - Gennemtving lukning a Sonnie. NOTE: Kan kun udføres af en admin. _Example: ;kill_**

- **Ferie [D/M/Å/T.M-D/M/Å/T.M], Ferie [?], Ferie [Annuller]**
  - **Sæt Sonnie til ferie-mode. Det vil gøre at Sonnie ikke checker for porno.NOTE: Kan kun udføres af en admin. _Example: ;ferie [2/3/2019/12.30-5/3/2019/12.30]_ Eksemplet vil sætte Sonnie i feriemode fra perioden D. 2.Marts.2019 kl.12.30 til D. 5.Marts.2019 Kl. 12.30**
  - **Spørg Sonnie om han er på ferie, og i så fald, hvilken periode. _Example: ;ferie ?_**
  - **Annuller den nuværende ferie. _Example: ;ferie annuller_**

- **Bruger er [ID] [Navn] - Gem en bruger i Sonnies database. NOTE: Kan kun udføres af en admin. _Example: ;bruger er [615574635110465547] "Sonnie Hansen"_**

- **Hjælp - Sender dig en liste med all kommandoer. _Example: ;hjælp_**

- **Credits - Giver en liste over dem der har hjulpet med at lave Sonnie. _Example: ;credits_**

- **Version - Fortæller hvilket version af Sonnie der kører. _Example: ;version_**

## How To

###### Dependencies
Sonnie needs 2 different tokens in order to operate. More specifically he needs a [Discord Bot Token](https://github.com/reactiflux/discord-irc/wiki/Creating-a-discord-bot-&-getting-a-token) and [Facebook Token](https://www.youtube.com/watch?v=_hF099c0A9M).
These Tokens are loaded in via a .Json File.
The Discord Token should be located in the [config.Json](https://github.com/ZhakalenDk/SonnieTheBot/blob/master/SonnyTheBot/DiscordBot/bin/Debug/netcoreapp2.1/config.json), along with the prefix to execute commands (_See [Commands](https://github.com/ZhakalenDk/SonnieTheBot/blob/master/README.md#commands)_).
The Facebook Token is written in the [FacebookConfig.Json](https://github.com/ZhakalenDk/SonnieTheBot/blob/master/SonnyTheBot/DiscordBot/Data/FacebookConfig.Json), along with other values that will be explained later under "_Config Files_".

Sonnie also needs to be [linked to a Discord Server](https://github.com/jagrosh/MusicBot/wiki/Adding-Your-Bot-To-Your-Server).
The server details should be written in the [DiscordConfig.Json](https://github.com/ZhakalenDk/SonnieTheBot/blob/master/SonnyTheBot/DiscordBot/Data/DiscordConfig.Json) file.

###### Config Files
Sonnie has 3 config files that are loaded on upstart.

1. **[Config.Json](https://github.com/ZhakalenDk/SonnieTheBot/blob/master/SonnyTheBot/DiscordBot/bin/Debug/netcoreapp2.1/config.json)**
  - **Description:** This configuration file is for the command handling.
  - **Fields**:
    - **Token:** The token to the Bot Client. (See [How To Get Discord Token](https://github.com/reactiflux/discord-irc/wiki/Creating-a-discord-bot-&-getting-a-token))
    - **Prefix:** This is the symbol that will tell Sonnie that the chat message just written is a command.
2. **[FacebookConfig.Json](https://github.com/ZhakalenDk/SonnieTheBot/blob/master/SonnyTheBot/DiscordBot/Data/FacebookConfig.Json)**
  - **Description:** This is the configuration file that handles the Facebook hook values. It contain informationen about how Sonnie should contact the [Facebook Graph API](https://developers.facebook.com/docs/graph-api/).
  - **Fields:**
    - **GraphAPI:** This is the link to Facebook Graph API. This field will be set as default. Unless you know exactly what you're doing don't modify this.
    - **GroupID:** This is the ID for the Facebook group that Soonie should pull posts from. (See [How to Get Group ID]())
    - **Fields:** This defines which values to pull from facebook. This field is also set as default and unless you're 100% sure that you know exactly how to handle these values, don't modify this. If these fields are modified without setting Sonnie up for handling the new fields, he will crash with no exception.
    - **Token:** This is the Facebook Token that Sonnie will use to acces Facebooks API. (See [How to get Facebook Token](https://www.youtube.com/watch?v=_hF099c0A9M))
    - **FacebookFeedChannelID:** This is the ID for the Facebook Feed channel on the Discord Server. (See [How To Get Discord Channel ID]())
3. **[DiscordConfig.Json](https://github.com/ZhakalenDk/SonnieTheBot/blob/master/SonnyTheBot/DiscordBot/Data/DiscordConfig.Json)**
  - **Description:** This is the configuration file for setting up Sonnies link to the Discord server.
  - **Fields:**
    - **SuggestionChannelID:** This is the channel that handles users suggestions for Sonnie (See [How To Get Discord Channel ID]())
	  - **GeneralChannelID:** This is the general channel on the server. Sonnies main channel to communicate in. (See [How To Get Discord Channel ID]())
  
  ## How to get Facebook Group ID
  To get your Facebook Groups ID, go to Facebook and go to your group. In the Adress bar of your browser you will see the path to the group. In the adress you will find somehting simular to _Facebook/groups/[THIS IS YOUR GROUPS ID]/?ref=bookmarks_.
  
  ## How to get Dicord Text Channel ID
  Open Discord and go into your settings -> Appearance -> Enable Developer Mode.
  This will enable the developer mode, as you might have expected. However, now you can right click on almost anything in discord and in the bottom of the PopUp menu you should find "Copy ID".
  Go to the Text Channel you would like to get the ID for, and as stated above, right click on the channel and in the PopUp menu go to bottom and click on "_Copy ID_".
