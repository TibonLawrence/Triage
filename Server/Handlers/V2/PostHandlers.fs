module PostHandlersV2

open System
open Giraffe
open Microsoft.AspNetCore.Http
open System.Security.Claims
open Microsoft.AspNetCore.Authorization
open FSharp.Control
open WebSocketHandler
open Data
open Enums
open DbHelpers



let sendDataToSockets(ctx : HttpContext) = 
    let list = getEventsAndNotes (DateTime.Now, ctx)
    list |> WebSocketHandler.SendDataToSockets

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
                        User = AuthHelpers.getUser ctx
                        Timestamp = DateTime.Now
                    }

        dataContext.Notes.Add (dbNote) |> ignore
        dataContext.SaveChanges() |> ignore
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
                User = AuthHelpers.getUser ctx
                        //if String.IsNullOrEmpty ctx.User.Identity.Name then
                        //    Guid.Empty
                        // else
                        //    Guid.Empty
                        //User.FindFirst(ClaimTypes.NameIdentifier).Value
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