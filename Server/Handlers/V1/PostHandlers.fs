﻿module PostHandlersV1

open System
open Giraffe
open Microsoft.AspNetCore.Http
open FSharp.Control
open WebSocketHandlerV1
open Data
open Enums
open JsonV1
open DbHelpers

let dummyUser userId =
    {
        Id = userId
        Email = ""
        DisplayName = ""
        Roles = Role.Guest
    }

let sendDataToSockets(ctx : HttpContext) = 
    let list = getEventsAndNotes (DateTime.Now, ctx)
    list |> MappingHelperV1.mapEventsNotes |> WebSocketHandler.SendDataToSockets

let postNoteHandler (next: HttpFunc) (ctx: HttpContext) =
    task {    
        let! message = ctx.BindJsonAsync<Note>() 
        let dataContext = ctx.GetService<TriageData>()
        let dbNote = {
                        Id = 0
                        MatterId = message.MatterId
                        Subject = message.Subject
                        Body = message.Body
                        User = dummyUser message.UserId
                        Timestamp = DateTime.Now
                    }

        dataContext.Notes.Add (dbNote) |> ignore
        dataContext.SaveChanges() |> ignore
        do! ctx |> sendDataToSockets |> Async.StartAsTask
        return! next ctx
    }

let postEventHandler (next : HttpFunc) (ctx : HttpContext) =
    task {
        let! message = ctx.BindJsonAsync<Event>() 
        let dataContext = ctx.GetService<TriageData>()
        let dbEvent = {
                Id = 0
                MatterId = message.MatterId
                Category = Enum.Parse(typeof<Category>, message.Category) :?> Category
                Subject = message.Subject
                User = dummyUser message.UserId
                Action = message.Action
                Timestamp = DateTime.Now
            }

        dataContext.Events.Add (dbEvent) |> ignore
        dataContext.SaveChanges() |> ignore
        do! ctx |> sendDataToSockets |> Async.StartAsTask
            
        return! next ctx
    }

let postUserHandler (next : HttpFunc) (ctx : HttpContext) =
    task {
        let! message = ctx.BindJsonAsync<User>() 
        //do! sendMessageToSockets message.Text
        let dataContext = ctx.GetService<TriageData>()
        dataContext.Users.Add (message) |> ignore
        dataContext.SaveChanges() |> ignore
        return! next ctx
    }