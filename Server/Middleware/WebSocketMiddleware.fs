namespace TriageServer

module Middleware =
    open System
    open System.Threading.Tasks
    open System.Net.WebSockets
    open Microsoft.AspNetCore.Http
    open WebSocketManager
    
    type WebSocketMiddleware(next : RequestDelegate) =
        let Receive (id: Guid, socket: WebSocket) =
            async {
                let buffer : byte[] = Array.zeroCreate 4096
                let! ct = Async.CancellationToken
            
                while socket.State = WebSocketState.Open do
                    let! result = socket.ReceiveAsync(new ArraySegment<byte>(buffer), ct) |> Async.AwaitTask
                    
                    match result.MessageType with
                    | WebSocketMessageType.Close -> do! WebSocketManager.RemoveClient id
                    | WebSocketMessageType.Text -> ()
                    | _ -> ()
            }
        
        member __.Invoke(ctx : HttpContext) =
            async {
                if ctx.Request.Path = PathString("/ws") then
                    match ctx.WebSockets.IsWebSocketRequest with
                    | true ->
                        let! webSocket = ctx.WebSockets.AcceptWebSocketAsync() |> Async.AwaitTask
                        //sockets <- addSocket sockets webSocket
                        let id = WebSocketManager.AddClient webSocket
                        do! Receive (id, webSocket)

                    | false -> ctx.Response.StatusCode <- 400
                else
                    do! next.Invoke ctx |> Async.AwaitTask
            } |> Async.StartAsTask :> Task
