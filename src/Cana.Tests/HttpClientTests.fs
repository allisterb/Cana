module HttpClient

open System
open System.Threading
open Xunit

open Cana
open Cana.IR

[<Fact>]
let ``HttpClient API behaves correctly``() =    
    do Api.SetDefaultLoggerIfNone()
    
    let badUrl = HttpClient("foo", Html)
    let r1 = !> badUrl >>& !>>> badUrl.GetAsync  "/"
    !>? r1 |> Assert.False

    let url = HttpClient("https://www.google.com", Html)
    let badRequest = "@!*"
    let r2 = !> url >>& !>>> url.GetAsync badRequest
    !>? r2 |> Assert.False

    let goodRequest = "/"
    let r3 = !> url >>& !>>> url.GetAsync goodRequest
    !>? r3 |> Assert.True

[<Fact>]
let ``Can create client and get Url``() =    
    do Api.SetDefaultLoggerIfNone()
    let c = HttpClient("https://www.google.com", Html)
    let g = !>>> c.GetAsync  "/"
    !>? g |> Assert.True
    !>= g |> Assert.NotNull
    (!>= g).Content |> Assert.NotEmpty
  

   
    

    
    

    

    
   
    
    

