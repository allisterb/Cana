namespace CANA.News

open System

open CANA.News.Types

[<AutoOpen>]
module Behaviors = 

    let createPerson (name:string) (email:string option) = 
        {
            Name = FullName name; 
            Email = if email.IsSome then Some {Address = email.Value} else None; 
            Uri = None;
            Attrs = Map.empty
        }
    








