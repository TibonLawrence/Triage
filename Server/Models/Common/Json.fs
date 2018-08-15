module Json

open System

[<CLIMutable>]
type User =
    {
        Id: Guid
        LastName: string
        FirstName: string
        Email: string
    }