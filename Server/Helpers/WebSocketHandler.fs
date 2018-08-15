module WebSocketHandler

open System
open System.Text
open WebSocketManager
open System.Net.WebSockets
open Microsoft.FSharp.Control
open System.Threading
open Newtonsoft.Json
open Newtonsoft.Json.Serialization

type WebSocketHandler =
    static member SendDataToSockets (message: JsonV1.NotesAndEvents) =
        WebSocketManager.GetConnected |> Seq.map (fun (id, _) -> WebSocketHandler.SendData (id,  message)) |> Async.Parallel |> Async.Ignore

    static member SendData (id: Guid, message: JsonV1.NotesAndEvents) =
        async {
            let contractOpts = new JsonSerializerSettings(
                                ContractResolver = new DefaultContractResolver ( NamingStrategy = CamelCaseNamingStrategy() ),
                                Formatting = Formatting.Indented)
            let buffer = JsonConvert.SerializeObject (message, contractOpts) |> Encoding.UTF8.GetBytes
            let segment = new ArraySegment<byte>(buffer)

            let client = WebSocketManager.GetClient id
            match client with
            | Some socket -> do! socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None) |> Async.AwaitTask
            | None -> ()
        }

    static member HandleData (socket: WebSocket, buffer: ArraySegment<byte>) =
        async {
            () |> ignore
        }