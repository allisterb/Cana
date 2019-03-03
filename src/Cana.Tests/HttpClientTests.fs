module HttpClient

open System
open System.Threading
open Xunit

open Cana
open Cana.IR

[<Fact>]
let ``Can create client ``() =    
    do Api.SetDefaultLoggerIfNone()
    let c = HttpClient("https://www.google.com", Html)
    let g = !>>> c.GetAsync  "/"
    !>? g |> Assert.True
    !>= g |> Assert.NotNull
    (!>= g).Content |> Assert.NotNull 

   
    

    
    

    

    
   
    
    

