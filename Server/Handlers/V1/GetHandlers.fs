module GetHandlersV1

open System
open Giraffe
open Microsoft.AspNetCore.Http
open JsonV1
open System.Net
open DbHelpers
open Newtonsoft.Json.Converters
open System.IO
open Newtonsoft.Json
open MappingHelperV1

//let indexHandler : HttpHandler =
//    let model = { 
//        Id = 0
//        MatterId = 0
//        Category = Enums.Category.GetName (typeof<Enums.Category>, Enums.Category.Disposition) 
//        Subject = ""
//        UserId = Guid.Empty
//        Action = ""
//        Timestamp = DateTime.Now
//    }

//    razorHtmlView "Index" model


let getEventsHandler(timeStamp: string) (next : HttpFunc) (ctx : HttpContext) =
    ((DbHelpers.getEvents ((timeStampOrNow timeStamp), ctx) |> mapEventsNotes).Events |> json) next ctx

let getNotesHandler(timeStamp: string) (next : HttpFunc) (ctx : HttpContext) =
    ((DbHelpers.getNotes ((timeStampOrNow timeStamp), ctx) |> mapEventsNotes).Notes |> json) next ctx

let getNotesAndEventsHandler (timeStamp: string) (next: HttpFunc) (ctx : HttpContext) =
    ((DbHelpers.getEventsAndNotes ((timeStampOrNow timeStamp), ctx) |> mapEventsNotes) |> json) next ctx

//let getUserHandler (id) (next : HttpFunc) (ctx : HttpContext) =
//    let dataContext = ctx.GetService<TriageData>()
//    dataContext.users |> Seq.find(fun seq -> seq.Id = id)

