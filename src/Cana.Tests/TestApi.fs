namespace Cana.Tests

open System

open Cana

type TestApi(init) =
    inherit Api() 
    
    do Api.SetDefaultLoggerIfNone()

    override this.Initialized with get() = init

    member x.A() = Success "You're a success"
    member x.B() = Exception("You have failed.") |> Failure