module Data

open System
open System.ComponentModel.DataAnnotations
open Microsoft.EntityFrameworkCore
open Enums
open System.ComponentModel.DataAnnotations.Schema

[<CLIMutable>]
[<Table("Users")>]
type User = 
    {
        [<Key>]
        Id: Guid
        Email: string
        DisplayName: string
        [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
        Role: Role
    }

[<CLIMutable>]
[<Table("Events")>]
type Event = 
    {
        [<Key>]
        [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
        Id: int
        MatterId: int
        Category: Category
        Action: string
        Subject: string
        [<ForeignKey("UserId")>]
        User: User
        Timestamp: DateTime
    } 
    
[<CLIMutable>]
[<Table("Notes")>]
type Note =
    {
        [<Key>]
        [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
        Id: int
        MatterId: int
        Subject: string
        Body: string
        [<ForeignKey("UserId")>]
        User: User
        Timestamp: DateTime
    } 

type TriageData (options) = 
    inherit DbContext (options)
    
    //override __.OnModelCreating modelBuilder = 
    //    let catConvert = ValueConverter<CategoryEnum, string>((fun v -> v.ToString()), (fun v -> Enum.Parse(typedefof<CategoryEnum>, v) :?> EpisodeStatus))
    //    modelBuilder.Entity<Event>().Property(fun e -> e.Category).HasConversion(catConvert) |> ignore    

    [<DefaultValue>] 
    val mutable events : DbSet<Event>
    member __.Events with get() = __.events and set v = __.events <- v

    
    [<DefaultValue>] 
    val mutable users : DbSet<User>
    member __.Users with get() = __.users and set v = __.users <- v

    [<DefaultValue>] 
    val mutable notes : DbSet<Note>
    member __.Notes with get() = __.notes and set v = __.notes <- v
