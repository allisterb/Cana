namespace Cana.IR

open System
open System.Collections.Generic

open CodeHollow.FeedReader

open Cana

module RssFeed = 
    let getHtml response = response.Content |> Success
    
    let parseFeedUrlsFromHtml html = 
        let o =  !> FeedReader.ParseFeedUrlsFromHtml <| html
        o
        
        
    let getFeed(url:string) = 
        let c = HttpClient(url, Html)
        !!> c |><| Async.RunSynchronously << c.GetAsync <| url >>= getHtml