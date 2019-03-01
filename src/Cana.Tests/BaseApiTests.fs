module BaseApiTests

open System

open Xunit

open Cana


new ConsoleLogger() |> Api.SetLogger

type TestApi(init) =
    inherit Api() 
    
    override this.Initialized with get() = init

    member x.A() = Success "I'm a success"
    member x.B() = Exception("You have failed.") |> Failure

    

//let f = !? TestApi(Failure null)

let z0 = !?> TestApi(Success null)

let z1 = !?> TestApi(Failure null)

