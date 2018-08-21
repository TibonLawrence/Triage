module MappingHelperV2

open JsonV2
open Models
open System

let mapEventsNotes (eventsNotes : (Event list * Note list)) = 
    {
        Events =
            fst eventsNotes |> List.map(
                fun dbEvent -> 
                                let split = dbEvent.User.DisplayName.Split ","
                                {
                                    Id = dbEvent.Id
                                    MatterId = dbEvent.MatterId
                                    Category = Enum.GetName(typeof<Enums.Category>, dbEvent.Category)
                                    Subject = dbEvent.Subject
                                    User = 
                                        {
                                                Id = dbEvent.User.Id
                                                LastName = split.[0]
                                                FirstName = split.[1]
                                                Email = dbEvent.User.Email
                                        }
                                    Action = dbEvent.Action
                                    Timestamp = dbEvent.Timestamp
                                } : JsonV2.Event)
        Notes =
            snd eventsNotes |> List.map(
                fun dbNote -> 
                                let split = dbNote.User.DisplayName.Split ","
                                {
                                    Id = dbNote.Id
                                    MatterId = dbNote.MatterId
                                    Subject = dbNote.Subject
                                    Body = dbNote.Body
                                    User = 
                                        {
                                            Id = dbNote.User.Id
                                            LastName = split.[0]
                                            FirstName = split.[1]
                                            Email = dbNote.User.Email
                                        }
                                    Timestamp = dbNote.Timestamp
                                } : JsonV2.Note)
    }

