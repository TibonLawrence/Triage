module JsonV1

open System
open Json
open System.ComponentModel.DataAnnotations
    

[<CLIMutable>]
type Event =
    {
        MatterId: int
        Category: string
        Subject: string
        UserId: Guid
        Action: string
        Timestamp: DateTime
    }


[<CLIMutable>]
type Note =
    {
        MatterId: int
        Subject: string
        Body: string
        UserId: Guid
        Timestamp: DateTime
    }

[<CLIMutable>]
type EventRequest =
    {
        DateStart: DateTime
    }
    
[<CLIMutable>]
type NotesAndEvents =
    {
        Notes: Note list
        Events: Event list
    }