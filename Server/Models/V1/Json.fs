module JsonV1

open System
open Json
open System.ComponentModel.DataAnnotations
    

[<CLIMutable>]
type Event =
    {
        MatterId: int
        Category: string
        Subject: string
        UserId: Guid
        Action: string
        Timestamp: DateTime
    }


[<CLIMutable>]
type Note =
    {
        MatterId: int
        Subject: string
        Body: string
        UserId: Guid
        Timestamp: DateTime
    }

[<CLIMutable>]
type EventRequest =
    {
        DateStart: DateTime
    }
    
[<CLIMutable>]
type NotesAndEvents =
    {
        Notes: Note list
        Events: Event list
    }

type IdToken =
    {
        aud: string
        iss: string
        iat: string
        nbf: string
        exp: string
        aio: string
        name: string
        nonce: string
        oid: string
        preferred_username: string
        sub: string
        tid: string
        uti: string
        ver: string
    }

    
type AuthItem =
    {
        displayableId: string
        name: string
        identityProvider: string
        userIdentifier: string
        idToken: IdToken
    }
