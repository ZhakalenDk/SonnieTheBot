# SonnieTheBot
_Sonnie is a Danish Bot, so all his commands and his respones are in Danish._

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

###### Commands
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
Sonnie needs 2 different tokens in order to operate.
More specifically he needs a [Discord Bot Token](https://github.com/reactiflux/discord-irc/wiki/Creating-a-discord-bot-&-getting-a-token) and [Facebook Token](https://www.youtube.com/watch?v=_hF099c0A9M).
These Tokens are loaded in via a .Json File.
The Discord Token should be located in the [config.Json](https://github.com/ZhakalenDk/SonnieTheBot/blob/master/SonnyTheBot/DiscordBot/bin/Debug/netcoreapp2.1/config.json), along with the prefix to execute commands (_I'll explain commands further down_).
The Facebook Token is written in the [FacebookConfig.Json](https://github.com/ZhakalenDk/SonnieTheBot/blob/master/SonnyTheBot/DiscordBot/Data/FacebookConfig.Json), along with other values that will be explained later under "_Config Files_".

###### Config Files
