module PostHandlersV2

open System
open Giraffe
open Microsoft.AspNetCore.Http
open MappingHelperV2
open FSharp.Control
open WebSocketHandlerV2
open Data
open Enums
open DbHelpers



let sendDataToSockets(ctx : HttpContext) = 
    let list = getEventsAndNotes (DateTime.Now, ctx)
    list |> mapEventsNotes |> WebSocketHandler.SendDataToSockets

let postNoteHandler (next: HttpFunc) (ctx: HttpContext) =
    task {    
        let! message = ctx.BindJsonAsync<JsonV2.Note>() 
        //do! sendMessageToSockets message.Text
        let dataContext = ctx.GetService<TriageData>()
        let dbNote = {
                        Id = 0
                        MatterId = message.MatterId
                        Subject = message.Subject
                        Body = message.Body
                        User = AuthHelpers.userFromCtx ctx
                        Timestamp = DateTime.Now
                    }

        dataContext.Notes.Add (dbNote) |> ignore
        let! _ = dataContext.SaveChangesAsync true
        do! ctx |> sendDataToSockets |> Async.StartAsTask
        return! next ctx
    }

let postEventHandler (next : HttpFunc) (ctx : HttpContext) =
    task {
        let! message = ctx.BindJsonAsync<JsonV2.Event>() 
        //do! sendMessageToSockets message.Text
        let dataContext = ctx.GetService<TriageData>()
        let dbEvent = {
                Id = 0
                MatterId = message.MatterId
                Category = Enum.Parse(typeof<Category>, message.Category) :?> Category
                Subject = message.Subject
                User = AuthHelpers.userFromCtx ctx
                Action = message.Action
                Timestamp = DateTime.Now
            }

        dataContext.Events.Add (dbEvent) |> ignore
        let! _ = dataContext.SaveChangesAsync true
        do! ctx |> sendDataToSockets |> Async.StartAsTask
            
        return! next ctx
    }

let postUserHandler (next : HttpFunc) (ctx : HttpContext) =
    task {
        let! message = ctx.BindJsonAsync<User>()
        let dataContext = ctx.GetService<TriageData>()
        dataContext.Users.Add (message) |> ignore
        let! _ = dataContext.SaveChangesAsync true
        return! next ctx
    }