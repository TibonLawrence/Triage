module DeleteHandlers

open Giraffe
open Microsoft.AspNetCore.Http
open System.Data.Common

let deleteEventHandler =
    fun (id: int) (next : HttpFunc) (ctx : HttpContext) ->
        let status = 
            try 
                (DbHelpers.removeEvent (id, ctx)) |> ignore 
                "ok"
            with
            |  :? DbException -> "fail" 
        (status |> text) next ctx

let deleteNoteHandler =
    fun (id: int) (next : HttpFunc) (ctx : HttpContext) ->
        let status = 
            try 
                (DbHelpers.removeEvent (id, ctx)) |> ignore 
                "ok"
            with
            |  :? DbException -> "fail" 
        (status |> text) next ctx