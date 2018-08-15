module MappingHelperV1

open JsonV1
open System

let mapEventsNotes (eventsNotes: (Data.Event list * Data.Note list)) = 
    {
        Events =
            fst eventsNotes |> List.map(
                fun dbEvent -> 
                                {
                                    MatterId = dbEvent.MatterId
                                    Category = Enum.GetName(typeof<Enums.Category>, dbEvent.Category)
                                    Subject = dbEvent.Subject
                                    UserId = dbEvent.User.Id
                                    Action = dbEvent.Action
                                    Timestamp = dbEvent.Timestamp
                                } : JsonV1.Event)
        Notes =
            snd eventsNotes |> List.map(
                fun dbNote -> 
                                {
                                    MatterId = dbNote.MatterId
                                    Subject = dbNote.Subject
                                    Body = dbNote.Body
                                    UserId = dbNote.User.Id
                                    Timestamp = dbNote.Timestamp
                                } : JsonV1.Note)
    }