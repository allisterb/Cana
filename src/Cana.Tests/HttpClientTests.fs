module HttpClientTests

open System
open System.Threading
open Xunit

open Cana
open Cana.IR

do SerilogLogger() |> SetLogger

[<Fact>]
let ``Can create client ``() =    
    let c = HttpClient("https://www.google.com", Html)
    let g = c.GetAsync "/" |> Async.RunSynchronously
    g.Content |> Assert.NotEmpty
    printfn "%s" g.Content
    

    
    

    

    
   
    
    

