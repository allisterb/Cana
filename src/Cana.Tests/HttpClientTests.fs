module HttpClient

open System
open System.Threading
open Xunit

open Cana
open Cana.IR

[<Fact>]
let ``Can create client ``() =    
    Api.SetLoggerIfNone <| new SerilogLogger()
    let c = HttpClient("https://www.google.com", Html)
    let g = !>>> c.GetAsync  "/"
    !>? g |> Assert.True
    !>= g |> Assert.NotNull
    let c = !>= g
    Assert.NotNull c.Content

   
    

    
    

    

    
   
    
    

