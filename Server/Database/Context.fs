module Context

open Models
open Microsoft.EntityFrameworkCore


type TriageContext (options) = 
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
