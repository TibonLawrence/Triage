module AuthHandlers

open Giraffe
open Microsoft.AspNetCore.Http
open System
open Microsoft.AspNetCore.Identity
open Microsoft.AspNetCore.Authentication.JwtBearer
open System.Security.Claims
open System.Collections.Generic

let mustBeAuthenticated : HttpHandler = 
    requiresAuthentication (challenge JwtBearerDefaults.AuthenticationScheme)

let getCurrentUser =
    fun (ctx: HttpContext) ->
        try
            Guid.Parse(ctx.User.FindFirst(fun c -> c.Type = ClaimTypes.NameIdentifier).Value)
        with
            | _ -> Guid.Empty
