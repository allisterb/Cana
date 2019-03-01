module BaseApi

open System

open Xunit

open Cana
open Cana.Tests

[<Fact>]
let ``Can construct``() =
    !! TestApi(true) |> Assert.NotNull

 

