namespace Cana

open System;

type Feed = {Title: FeedTitle; Uri: Uri; Description: FeedDescription; Author: Person; Attrs: Map<string, Object>; Items: Content list}
    and FeedTitle = FeedTitle of string
    and FeedDescription = FeedDescription of string

and Content = { Text: string; Uri : Uri option; Attrs: Map<string, obj> option}

and Category = {Name: string; Label:string option; Scheme: string option; Attrs: Map<string, Object> option}

and Urls = Urls of Uri list  

 