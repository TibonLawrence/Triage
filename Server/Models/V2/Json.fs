module JsonV2

open System
open Json
open System.ComponentModel.DataAnnotations
    

[<CLIMutable>]
type Event =
    {
        Id: int
        MatterId: int
        Category: string
        Subject: string
        User: User
        Action: string
        Timestamp: DateTime
    }

[<CLIMutable>]
type Note =
    {
        Id: int
        MatterId: int
        Subject: string
        Body: string
        User: User
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
