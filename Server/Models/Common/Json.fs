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
