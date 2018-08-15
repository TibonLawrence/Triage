module WebSocketManager

open System
open System.Collections.Concurrent
open System.Net.WebSockets
open Microsoft.FSharp.Control
open System.Threading

type WebSocketManager() =
    static let clientCache = new ConcurrentDictionary<Guid, WebSocket>()

    static member GetClient (id: Guid) =
        match clientCache.TryGetValue id with
        | (true, socket) -> Some socket
        | (false, _) -> None

    static member GetConnected =
        clientCache |> Seq.filter (fun kvp -> kvp.Value.State <> WebSocketState.Open) |> Seq.iter (fun kvp -> WebSocketManager.RemoveClient kvp.Key |> ignore)
        clientCache |> Seq.filter (fun kvp -> kvp.Value.State = WebSocketState.Open) |> Seq.map (fun kvp -> (kvp.Key, kvp.Value))

    static member AddClient(socket: WebSocket) =
        let guid = Guid.NewGuid()
        match clientCache.TryAdd (guid, socket) with
        | true -> guid
        | false -> Guid.Empty

    static member RemoveClient(id: Guid) =
        async {
            match clientCache.TryRemove id with
            | (true, r) -> if r.State <> WebSocketState.Closed then
                                r.CloseAsync (WebSocketCloseStatus.NormalClosure, "Client Timeout", CancellationToken.None) |> Async.AwaitTask |> Async.Catch |> ignore
                           else ()
            | (false, _) -> ()
        }
