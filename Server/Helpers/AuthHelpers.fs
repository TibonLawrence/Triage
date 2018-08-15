module AuthHelpers

open Data
open Microsoft.AspNetCore.Http
open System
open Enums
open System.Security.Claims

let getUser (ctx : HttpContext) =
    {
        Id = Guid.Parse(ctx.User.FindFirst(fun c -> c.Type = ClaimTypes.NameIdentifier).Value)
        Email = ctx.User.FindFirst(fun c -> c.Type = "preferred_username").Value
        DisplayName = ctx.User.FindFirst(fun c -> c.Type = "name").Value
        Role = Role.User
    }