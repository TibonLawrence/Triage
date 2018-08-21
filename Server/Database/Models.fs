module Models

open System
open System.ComponentModel.DataAnnotations
open Microsoft.EntityFrameworkCore
open Enums
open System.ComponentModel.DataAnnotations.Schema
open System.Collections.Generic


[<Table("Events")>]
type Event() = 
    [<Key>]
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    member val Id = 0 with get, set
    member val MatterId = 0 with get, set
    member val Category = Category.Disposition with get, set
    member val Action = String.Empty with get, set
    member val Subject = String.Empty with get, set
    [<Required>]
    member val UserId = Guid.Empty with get, set
    [<ForeignKey("UserId")>]
    abstract member User: User with get, set
    default val User = Unchecked.defaultof<User> with get, set
    member val Timestamp = DateTime.MinValue with get, set


and [<Table("Notes")>] Note() =
    [<Key>]
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    member val Id = 0 with get, set
    member val MatterId = 0 with get, set
    member val Subject = String.Empty with get, set
    member val Body = String.Empty with get, set
    [<Required>]
    member val UserId = Guid.Empty with get, set
    [<ForeignKey("UserId")>]
    abstract member User: User with get, set
    default val User = Unchecked.defaultof<User> with get, set
    member val Timestamp = DateTime.MinValue with get, set

   
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
