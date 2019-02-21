namespace Cana.IR

open System
open System.IO
open System.Net

open Cana

type HttpClient(url: string, ?proxy: string) =
    inherit CanaApi()
    
    member this.fetchUrl callback url =        
        let req = WebRequest.Create(Uri(url)) 
        use resp = req.GetResponse() 
        use stream = resp.GetResponseStream() 
        use reader = new StreamReader(stream) 
        callback reader url
    