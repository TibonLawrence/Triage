module Models

open System
open System.ComponentModel.DataAnnotations
open Microsoft.EntityFrameworkCore
open Enums
open System.ComponentModel.DataAnnotations.Schema
open System.Collections.Generic


[<Table("Events")>]
type Event() = 
    let mutable id = 0
    let mutable matterId = 0
    let mutable category = Category.Disposition
    let mutable action = String.Empty
    let mutable subject = String.Empty
    let mutable userId = Guid.Empty
    let mutable timeStamp = DateTime.MinValue


    [<Key>]
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    member __.Id with get() = id and set v = id <- v
    member __.MatterId with get() = matterId and set v = matterId <- v
    member __.Category with get() = category and set v = category <- v
    member __.Action with get() = action and set v = action <- v
    member __.Subject with get() = subject and set v = subject <- v
    [<Required>]
    member __.UserId with get() = userId and set v = userId <- v
    [<ForeignKey("UserId")>]
    abstract member User: User
    default val User = Unchecked.defaultof<User>
    member __.Timestamp with get() = timeStamp and set v = timeStamp <- v

and [<Table("Notes")>] Note() =
    let mutable id = 0
    let mutable matterId = 0
    let mutable category = Category.Disposition
    let mutable subject = String.Empty
    let mutable body = String.Empty
    let mutable userId = Guid.Empty
    let mutable timeStamp = DateTime.MinValue
    
    [<Key>]
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    member __.Id with get() = id and set v = id <- v
    member __.MatterId with get() = matterId and set v = matterId <- v
    member __.Subject with get() = subject and set v = subject <- v
    member __.Body with get() = body and set v = body <- v
    [<Required>]
    member __.UserId with get() = userId and set v = userId <- v
    [<ForeignKey("UserId")>]
    abstract member User: User
    default val User = Unchecked.defaultof<User>
    member __.Timestamp with get() = timeStamp and set v = timeStamp <- v

   
and [<Table("Users")>] User() = 
    [<Key>]
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    member val Id = Guid.Empty with get, set
    member val Email = String.Empty with get, set 
    member val DisplayName = String.Empty with get, set

    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    member val Roles = Role.Guest with get, set

    abstract member Events: ICollection<Event> with get, set
    abstract member Notes: ICollection<Note> with get, set
    default val Notes = null with get, set
    default val Events = null with get, set