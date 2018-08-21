module PostHandlersV1

open System
open Giraffe
open Microsoft.AspNetCore.Http
open FSharp.Control
open WebSocketHandlerV1
open Context
open DbHelpers
open Models

let sendDataToSockets(ctx : HttpContext) = 
    let list = getEventsAndNotes (DateTime.Now, ctx)
    list |> MappingHelperV1.mapEventsNotes |> WebSocketHandler.SendDataToSockets

let postNoteHandler (next: HttpFunc) (ctx: HttpContext) =
    task {    
        let! message = ctx.BindJsonAsync<Note>() 
        let dataContext = ctx.GetService<TriageContext>()
       
        dataContext.Notes.Add (
            new Note(Id = 0, MatterId = message.MatterId, Subject = message.Subject,
                Body = message.Body, UserId = message.UserId, Timestamp = DateTime.Now)) |> ignore
        dataContext.SaveChanges() |> ignore
        do! ctx |> sendDataToSockets |> Async.StartAsTask
        return! next ctx
    }

let postEventHandler (next : HttpFunc) (ctx : HttpContext) =
    task {
        let! message = ctx.BindJsonAsync<JsonV1.Event>() 
        let dataContext = ctx.GetService<TriageContext>()
        let category = Enum.Parse(typeof<Enums.Category>, message.Category) :?> Enums.Category
        let dbEvent = 
            new Event(
                Id = 0,
                MatterId = message.MatterId,
                Subject = message.Subject,
                UserId = message.UserId,
                Action = message.Action,
                Timestamp = DateTime.Now,
                Category = category)

        dataContext.Events.Add (dbEvent) |> ignore
        let! _ = dataContext.SaveChangesAsync()
        do! ctx |> sendDataToSockets |> Async.StartAsTask
            
        return! next ctx
    }

let postUserHandler (next : HttpFunc) (ctx : HttpContext) =
    task {
        let! message = ctx.BindJsonAsync<User>() 
        //do! sendMessageToSockets message.Text
        let dataContext = ctx.GetService<TriageContext>()
        dataContext.Users.Add (message) |> ignore
        let! _ = dataContext.SaveChangesAsync()
        return! next ctx
    }