module DbHelpers

open Giraffe
open System
open Microsoft.AspNetCore.Http
open Data
open System.Diagnostics.Tracing
open Enums
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.Extensions.Configuration
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Http
open System.Security.Claims

type EventsNotesBoth =
    | Events = 0
    | Notes = 1
    | Both = 2

let timeStampOrNow (timeStamp: string) =
    try
        DateTime.Parse(timeStamp)
    with
    | :? System.FormatException -> DateTime.Now
    | _ -> DateTime.Now


let dummyUser(id: Guid) =
    {
        Id = id
        Email = String.Empty
        DisplayName = String.Empty
        Roles = Role.Guest
    }

let removeEvent (id:int, ctx: HttpContext)=
    let dataContext = ctx.GetService<TriageData>()
    dataContext.Events.Remove(
        {
            Id = id
            MatterId = 1
            Category = Category.Disposition
            Action = String.Empty
            Subject = String.Empty
            User = Unchecked.defaultof<User>
            Timestamp = DateTime.MinValue
        }) |> ignore

    dataContext.SaveChanges()

let removeNote (id:int, ctx: HttpContext)=
    let dataContext = ctx.GetService<TriageData>()
    dataContext.Notes.Remove(
        {
            Id = id
            Body = ""
            MatterId = 1
            Subject = String.Empty
            User = Unchecked.defaultof<User>
            Timestamp = DateTime.MinValue
        }) |> ignore

    dataContext.SaveChanges()

let _getNotesAndEvents (timeStamp: DateTime, ctx: HttpContext, eventsOrNotes: EventsNotesBoth) : (Event list * Note list) =
    let dataContext = ctx.GetService<TriageData>()
    (
        match eventsOrNotes with
        | EventsNotesBoth.Both | EventsNotesBoth.Events -> 
            dataContext.Events |> Seq.where(fun seq -> seq.Timestamp.Date = timeStamp.Date) |> Seq.toList
        | EventsNotesBoth.Notes -> []
        ,
        match eventsOrNotes with
        | EventsNotesBoth.Both | EventsNotesBoth.Notes -> 
            dataContext.Notes |> Seq.where(fun seq -> seq.Timestamp.Date = timeStamp.Date) |> Seq.toList
                                        
        | EventsNotesBoth.Events -> []
    )

let getNotes (timestamp, ctx) = _getNotesAndEvents (timestamp, ctx, EventsNotesBoth.Notes)

let getEvents (timestamp, ctx) = _getNotesAndEvents (timestamp, ctx, EventsNotesBoth.Events)

let getEventsAndNotes (timestamp, ctx) = _getNotesAndEvents (timestamp, ctx, EventsNotesBoth.Both)

let tryAddUser (ctx: HttpContext) =
    let db = ctx.GetService<TriageData>()
    let user = AuthHelpers.userFromCtx ctx
    match box (db.Users.Find user) with
    | null -> db.Users.Add (user)
    | x -> unbox x