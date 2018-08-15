module Enums

open System

type Category =
    | Disposition = 1
    | Transition = 2

type Role =
    | Admin = 1
    | User = 2
    | Guest = 3

[<Flags>]
type PartiesPresent =
    | Neither = 0
    | Petitioner = 1
    | Respondent = 2
    | Both = 3