namespace Exceptions

open System

module ExceptionTypes =
    
    // TryResult, used to pass results or exception as values.
    type TryResult<'r, 'e> =
        | Result of 'r
        | Error of 'e :> Exception

    /// <summary>
    /// Returns a value wrapped as a Result.
    /// </summary>
    /// <param name="res">The result to wrap.</param>
    /// <returns>Returns the wrapped result.</returns>
    let asResult value : TryResult<_,Exception> = Result value

    /// <summary>
    /// Returns an exception wrapped as an Error.
    /// </summary>
    /// <param name="err">The exception to wrap.</param>
    /// <returns>Returns the wrapped exception.</returns>
    let asError (err : Exception) : TryResult<unit,_> = Error(err)

    /// <summary>
    /// Runs the specified function inside a try / with handler.
    /// Any exceptions get returned as an Error.
    /// </summary>
    /// <param name="runFunc">The function to run.</param>
    /// <returns>Returns the wrapped result or error.</returns>
    let tryRun<'a> (runFunc : (unit -> 'a)) : TryResult<'a,Exception> =
        try
            let tryResult = runFunc()
            Result tryResult
        with
            | ex -> Error ex

    // Computation expression to handle Try exception handling
    // Wrapping up into a TryResult<'a,Exception> type.
    type TryMonad() =
        // Called for let! and !do in computation expressions.
        member this.Bind(m, f) =
            match m with
            | Result v -> f v
            | Error _ -> m

        // Called for return in computation expressions.
        member this.Return(v) = Result v

        // Called for return! in computation expressions.
        member this.ReturnFrom(c) = c

        // Called for empty else branches of if...then expressions in computation expressions.
        member this.Zero() = Result 0