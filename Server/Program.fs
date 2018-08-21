module TriageServer.App

open System
open System.IO
open Giraffe
open Giraffe.Razor
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Configuration
open Microsoft.EntityFrameworkCore


open ErrorHandlers
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.FileProviders
open Microsoft.AspNetCore.Authentication.JwtBearer
open Context


// ---------------------------------
// Config and Main
// ---------------------------------

[<EntryPoint>]
let main _ =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot     = contentRoot

    let configureCors (builder : CorsPolicyBuilder) =
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               |> ignore


    let configureApp (app : IApplicationBuilder) =
        let env = app.ApplicationServices.GetService<IHostingEnvironment>()

        (match env.IsDevelopment() with
        | true  -> app.UseDeveloperExceptionPage()
        | false -> app.UseGiraffeErrorHandler errorHandler)
            .UseCors(configureCors)
            .UseStaticFiles()
            .UseWebSockets()
            .UseStaticFiles(
                new StaticFileOptions(
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "static")),
                    RequestPath = new PathString("/static")))
            .UseMiddleware<TriageServer.Middleware.WebSocketMiddleware>()
            .UseHttpsRedirection()
            .UseAuthentication()
            .UseGiraffe(Webapp.webApp)

    
     
    let configureServices (services : IServiceCollection) =

        let sp  = services.BuildServiceProvider()
        let env = sp.GetService<IHostingEnvironment>()
        let viewsFolderPath = Path.Combine(env.ContentRootPath, "Views")
        let configuration = (new ConfigurationBuilder())
                                .SetBasePath(contentRoot)
                                .AddJsonFile("appsettings.json")
                                .AddEnvironmentVariables().Build()
        let connString = configuration.GetConnectionString("TriageServer")
        services.AddDbContext<TriageContext>(fun contextBuilder -> contextBuilder.UseSqlServer connString |> ignore) |> ignore
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(fun opt -> configuration.Bind("JwtBearer", opt)) |> ignore
        //services.AddAuthorization(fun options ->
        //    options.AddPolicy(
        //            "Administrators", 
        //            ( fun policy -> policy.RequireClaim(ClaimTypes.Role, "Administrator") |> ignore ) 
        //        ) |> ignore
        //    options.AddPolicy(
        //            "Users", 
        //            ( fun policy -> policy.RequireClaim(ClaimTypes.Role, "Administrator", "User") |> ignore ) 
        //        ) |> ignore
        //    ) |> ignore

                //opt.Authority <- "https://login.microsoftonline.com/{TENANT NAME}";
                //opt.Audience <- "{Azure App ID URI}") |> ignore

        services
            .AddRazorEngine(viewsFolderPath)
            .AddSession()
            .AddDistributedMemoryCache()
            .AddCors()
            .AddGiraffe() |> ignore

    let configureLogging (builder : ILoggingBuilder) =
        let filter (l : LogLevel) = l.Equals LogLevel.Error
        builder.AddFilter(filter).AddConsole().AddDebug() |> ignore

    WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(contentRoot)
        .UseIISIntegration()
        .UseWebRoot(webRoot)
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0
 