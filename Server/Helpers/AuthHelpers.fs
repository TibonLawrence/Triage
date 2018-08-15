module AuthHelpers

open Data
open Microsoft.AspNetCore.Http
open System
open Enums
open System.Security.Claims


let userFromCtx (ctx : HttpContext) =
    {
        Id = Guid.Parse (ctx.User.FindFirst(fun c -> c.Type = "http://schemas.microsoft.com/identity/claims/objectidentifier").Value)
        Email = ctx.User.FindFirst(fun c -> c.Type = "preferred_username").Value
        DisplayName = ctx.User.FindFirst(fun c -> c.Type = "name").Value
        Roles = Role.Guest
    }
    