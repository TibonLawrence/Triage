module PostHandlersV2

open System
open Giraffe
open Microsoft.AspNetCore.Http
open MappingHelperV2
open FSharp.Control
open WebSocketHandlerV2
open Models
open Enums
open Context
open DbHelpers



let sendDataToSockets(ctx : HttpContext) = 
    let list = getEventsAndNotes (DateTime.Now, ctx)
    list |> mapEventsNotes |> WebSocketHandler.SendDataToSockets

let postNoteHandler (next: HttpFunc) (ctx: HttpContext) =
    task {    
        let! message = ctx.BindJsonAsync<JsonV2.Note>() 
        //do! sendMessageToSockets message.Text
        let dataContext = ctx.GetService<TriageContext>()
        let user = tryAddUser ctx 
        let dbNote = new Note(
                        Id = 0,
                        MatterId = message.MatterId,
                        Subject = message.Subject,
                        Body = message.Body,
                        UserId = user.Id,
                        Timestamp = DateTime.Now
                    )

        dataContext.Notes.Add (dbNote) |> ignore
        dataContext.SaveChanges() |> ignore
        do! ctx |> sendDataToSockets |> Async.StartAsTask
        return! next ctx
    }

let postEventHandler (next : HttpFunc) (ctx : HttpContext) =
    task {
        let! message = ctx.BindJsonAsync<JsonV2.Event>() 
        //do! sendMessageToSockets message.Text
        let dataContext = ctx.GetService<TriageContext>()
        let user = tryAddUser ctx
        let category = Enum.Parse(typeof<Category>, message.Category) :?> Category
        
        let dbEvent = 
            new Event(Id = 0,
                MatterId = message.MatterId,
                Category = category,
                Subject = message.Subject,
                UserId = user.Id,
                Action = message.Action,
                Timestamp = DateTime.Now
            )

        dataContext.Events.Add (dbEvent) |> ignore
        dataContext.SaveChanges() |> ignore
        do! ctx |> sendDataToSockets |> Async.StartAsTask
            
        return! next ctx
    }

let postUserHandler (next : HttpFunc) (ctx : HttpContext) =
    task {
        let! message = ctx.BindJsonAsync<User>()
        let dataContext = ctx.GetService<TriageContext>()
        dataContext.Users.Add (message) |> ignore
        let! _ = dataContext.SaveChangesAsync true
        return! next ctx
    }