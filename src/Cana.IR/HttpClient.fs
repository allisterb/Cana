namespace Cana.IR

open System
open System.IO
open System.Net

module HttpClient =
    let fetchUrl callback url =        
        let req = WebRequest.Create(Uri(url)) 
        use resp = req.GetResponse() 
        use stream = resp.GetResponseStream() 
        use reader = new StreamReader(stream) 
        callback reader url
    