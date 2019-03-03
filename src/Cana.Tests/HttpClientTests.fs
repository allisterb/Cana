module HttpClient

open System
open System.Threading
open Xunit

open Cana
open Cana.IR

[<Fact>]
let ``Can create client and get Url``() =    
    do Api.SetDefaultLoggerIfNone()
    let c = HttpClient("https://www.google.com", Html)
    let g = !>>> c.GetAsync  "/"
    !>? g |> Assert.True
    !>= g |> Assert.NotNull
    (!>= g).Content |> Assert.NotEmpty

[<Fact>]
let ``HttpClient API behaves correctly``() =    
    do Api.SetDefaultLoggerIfNone()
    
    let badUrl = HttpClient("foo", Html)
    let r1 = badUrl |> init' |><| (badUrl.GetAsync >> Async.RunSynchronously) <| "/"  
    !>? r1 |> Assert.False

    let badRequest = "@!*"
    let c = HttpClient("https://www.google.com", Html)
    let r2 = c |> init' |><<| c.GetAsync <| badRequest    
    !>? r2 |> Assert.False

    let goodRequest = "/"
    let r3 = c |!><<| c.GetAsync <| goodRequest
    !>? r3 |> Assert.True


  

   
    

    
    

    

    
   
    
    

