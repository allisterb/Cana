module BaseApi

open System

open Xunit

open Cana
open Cana.Tests


[<Fact>]
let ``Can construct``() =
    do Api.SetDefaultLoggerIfNone()
    !! TestApi(true) |> Assert.NotNull
    do (fun _ -> !! TestApi(false) |> ignore) |> Assert.Throws
    

 

