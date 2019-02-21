[<AutoOpen>]
module CANA.Extensions

open System;

open CANA.Types

type Person with
   
    static member Create (name:Name) = 
        {
            Name = name; 
            Email = None; 
            Uri = None; 
            Attrs = Map.empty
        } 

    static member Create (name:string, ?email:string) = 
        {
            Name = FullName name; 
            Email = if email = None then None else Some { Address = email.Value}
            Uri = None
            Attrs = Map.empty
        } 
    
    member this.AddEmail email = {this with Email = Some email}

    member this.AddUri uri = {this with Uri = Some uri}

    member this.AddAttr key value = {this with Attrs = this.Attrs.Add(key, value)}


    

    