module HttpClientTests

open System
open System.Threading
open Xunit

open Cana
open Cana.IR

[<Fact>]
let ``Can create client ``() =    
    Api.SetLogger <| new SerilogLogger()
    let c = HttpClient("https://www.google.com", Html)
    let g = c.GetAsync "/" |> Async.RunSynchronously
    g.Content |> Assert.NotEmpty
    printfn "%s" g.Content
    

    
    

    

    
   
    
    

