namespace CANA

open System;

module Types = 

    type Person = {Name: Name; Email: Email option; Uri: Uri option; Attrs: Map<string, obj>}
    
    and Email = {Address: string}        
    
    and Name = 
        | FullName of string
        | FirstLastName of string * string

    type Content = { Text: string; Uri : Uri option; Attrs: Map<string, obj> option}

    type Category = {Name: string; Label:string option; Scheme: string option; Attrs: Map<string, Object> option}

    type Feed = {Name: FeedName; Description: FeedDescription; Author: Person; Attrs: Map<string, Object>; Items: Content list}
        and FeedName = FeedName of string
        and FeedDescription = FeedDescription of string

    type Urls = Urls of Uri list

    

  