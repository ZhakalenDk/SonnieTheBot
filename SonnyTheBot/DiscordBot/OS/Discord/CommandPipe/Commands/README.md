# Curretly implemented commands:

## User Commands
- **Jeg er [Dit navn] - Fortæl Sonnie hvem du er. _Example: ;Jeg er "Sonnie Hansen"_**

- **Hvem er [Tag en bruger] - Spørg Sonnie hvem en bruger er. _Example: ;Hvem er "@Sonnie"_**

- **Foreslå [Dit forslag] - Foreslå noget til Sonnie. _Example: ;Foreslå "Ku' det ikke være sjovt, hvis Sonnie ku' ændre folks navne?"_**

- **Ikke Tilstede [Tag en Bruger] - Få Sonnie til at sende en besked til en bruger der ikke er tilstede. _Example: ;ikke tilstede @Sonnie_**

- **Hjælp - Sender dig en liste med all kommandoer. _Example: ;hjælp_**

- **Credits - Giver en liste over dem der har hjulpet med at lave Sonnie. _Example: ;credits_**

- **Version - Fortæller hvilket version af Sonnie der kører. _Example: ;version_**

## Admin Commands
- **Save - Gennemtving en 'Gem' handling på alle nuværende buffered brugere. NOTE: Kan kun udføres af en admin. _Example: ;save_**

- **Kill - Gennemtving lukning a Sonnie. NOTE: Kan kun udføres af en admin. _Example: ;kill_**

- **Ferie [D/M/Å/T.M-D/M/Å/T.M], Ferie [?], Ferie [Annuller]**
  - **Sæt Sonnie til ferie-mode. Det vil gøre at Sonnie ikke checker for porno. NOTE: Kan kun udføres af en admin. _Example: ;ferie [2/3/2019/12.30-5/3/2019/12.30]_ Eksemplet vil sætte Sonnie i feriemode fra perioden D. 2.Marts.2019 kl.12.30 til D. 5.Marts.2019 Kl. 12.30**
  - **Spørg Sonnie om han er på ferie, og i så fald, hvilken periode. _Example: ;ferie ?_**
  - **Annuller den nuværende ferie. _Example: ;ferie annuller_**

- **Bruger er [ID] [Navn] - Gem en bruger i Sonnies database. NOTE: Kan kun udføres af en admin. _Example: ;bruger er [615574635110465547] "Sonnie Hansen"_**

- **Event [Navn], [D/M/Å/T-D/M/Å/T], [Beskrivelse], [Andre Informationer], [Påmind (Optional) D/M/Å/T], Event [?], Event [Annuller [Event Navn]] - Få Sonnie til at holde styr på et event. NOTE: Kan kun udføres af en admin. Example: ;event "Mit Event" 09/10/2019/18.30-09/10/2019/20.30 "Beskrivelse, bla bla. Det her forklare mit event." "Mere info om mit event. Det finder sted på adressen Bla bla blabla. Og medbring Blabla bla" 09/10/2019/18.15**

  - **Få vist en besked med alle nuværende events. Example: ;event ?**

  - **Annuller et event Example: ;event annuller "Mit Event"**

- **Har fri [Navn] - Fortæl Sonnie at en bruger har fri. NOTE: Kan kun udføres af en admin. Example: ;har fri "Sonnie Eis"**
