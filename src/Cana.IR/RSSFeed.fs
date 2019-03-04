namespace Cana.IR

open System

open Cana
open Cana.IR.HttpClient

module RssFeed = 
    let getFeed(url:string) = 
        let c = HttpClient(url, Html)
        !> c |><| Async.RunSynchronously << c.GetAsync <| url >>= getString