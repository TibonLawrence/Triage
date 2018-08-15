﻿module GetHandlersV1

open System
open Giraffe
open Giraffe.Razor
open Microsoft.AspNetCore.Http
open Data
open JsonV1
open System.Net
open DbHelpers
open Newtonsoft.Json.Converters
open System.IO
open Newtonsoft.Json

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
    ((DbHelpersV1.getEvents ((timeStamp |> timeStampOrNow), ctx)).Events |> json) next ctx

let getNotesHandler(timeStamp: string) (next : HttpFunc) (ctx : HttpContext) =
    ((DbHelpersV1.getNotes ((timeStamp |> timeStampOrNow), ctx)).Notes |> json) next ctx

let getNotesAndEventsHandler (timeStamp: string) (next: HttpFunc) (ctx : HttpContext) =
    let settings = JsonSerializerSettings()
    settings.Converters.Add(Converters.DiscriminatedUnionConverter())
    (DbHelpersV1.getEventsAndNotes ((timeStamp|> timeStampOrNow), ctx) |> json) next ctx

let getUserHandler (id) (next : HttpFunc) (ctx : HttpContext) =
    let dataContext = ctx.GetService<TriageData>()
    dataContext.users |> Seq.find(fun seq -> seq.Id = id)
