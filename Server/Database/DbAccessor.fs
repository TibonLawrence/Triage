module DbHelpers

open Giraffe
open System
open Microsoft.AspNetCore.Http
open Models
open System.Diagnostics.Tracing
open Context
open Enums
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.Extensions.Configuration
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Http
open System.Security.Claims
open Microsoft.EntityFrameworkCore
open Microsoft.EntityFrameworkCore

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


let removeEvent (id:int, ctx: HttpContext) =
    async {
        let dataContext = ctx.GetService<TriageContext>()
        dataContext.Events.Remove(new Event(Id = id)) |> ignore
        do! dataContext.SaveChangesAsync() |> Async.AwaitTask |> Async.Ignore
    }

let removeNote (id:int, ctx: HttpContext) =
    async {
        let dataContext = ctx.GetService<TriageContext>()
        dataContext.Notes.Remove(new Note(Id = id)) |> ignore
        do! dataContext.SaveChangesAsync() |> Async.AwaitTask |> Async.Ignore
    }

let _getNotesAndEvents (timeStamp: DateTime, ctx: HttpContext, eventsOrNotes: EventsNotesBoth) : (Event list * Note list) =
    let dataContext = ctx.GetService<TriageContext>()
    (
        match eventsOrNotes with
        | EventsNotesBoth.Both | EventsNotesBoth.Events -> 
            dataContext.Events.Include(fun event -> event.User) |> Seq.where(fun seq -> seq.Timestamp.Date = timeStamp.Date) |> Seq.toList
        | EventsNotesBoth.Notes -> []
        ,
        match eventsOrNotes with
        | EventsNotesBoth.Both | EventsNotesBoth.Notes -> 
            dataContext.Notes.Include(fun note -> note.User) |> Seq.where(fun seq -> seq.Timestamp.Date = timeStamp.Date) |> Seq.toList
                                        
        | EventsNotesBoth.Events -> []
    )

let getNotes (timestamp, ctx) = _getNotesAndEvents (timestamp, ctx, EventsNotesBoth.Notes)

let getEvents (timestamp, ctx) = _getNotesAndEvents (timestamp, ctx, EventsNotesBoth.Events)

let getEventsAndNotes (timestamp, ctx) = _getNotesAndEvents (timestamp, ctx, EventsNotesBoth.Both)

let userFromCtx (ctx : HttpContext) =
    new User(
        Id = Guid.Parse (ctx.User.FindFirst(fun c -> c.Type = "http://schemas.microsoft.com/identity/claims/objectidentifier").Value),
        Email = ctx.User.FindFirst(fun c -> c.Type = "preferred_username").Value,
        DisplayName = ctx.User.FindFirst(fun c -> c.Type = "name").Value,
        Roles = Role.Guest
    )

let tryAddUser (ctx: HttpContext) =
    let db = ctx.GetService<TriageContext>()
    let user = userFromCtx ctx
    match box (db.Users.Find(user.Id)) with
    | null -> 
        db.Users.Add(user) |> ignore
    | _ -> ()

    db.SaveChanges() |> ignore

    user

//let tryAddUser (ctx: HttpContext) =
//    let db = ctx.GetService<TriageContext>()
//    let user = AuthHelpers.userFromCtx ctx
//    match box (db.Users.Find user) with
//    | null -> db.Users.Add (user)
//    | x -> unbox x