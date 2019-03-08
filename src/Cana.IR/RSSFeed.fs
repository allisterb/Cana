namespace Cana.IR

open System
open System.Collections.Generic
open System.IO
open System.Runtime.CompilerServices
open CodeHollow.FeedReader

open Cana

module RssFeed =  
    
    // Retrieve content from a remote Url as a string.
    let internal getUrlContent (c:HttpClient) url =
       Async.RunSynchronously << c.GetAsync <| url >>= fun r -> r.Content
    
    let internal parseFeedUrlsFromHtml html = !> FeedReader.ParseFeedUrlsFromHtml <| html 
        
    let internal getFeedUrlsFromHtmlPage (c:HttpClient) url  = getUrlContent c url >>>= parseFeedUrlsFromHtml >>= Seq.map (fun l -> l.Url)
    
    // Get a Feed object given a Url and HttpClient
    let internal getFeedFromUrl (c:HttpClient) url callback = c |>> getUrlContent c <| url >>= FeedReader.ReadFromString >>= callback
        