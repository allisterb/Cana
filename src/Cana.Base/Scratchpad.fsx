
#load "Logger.fs"
#load "Api.fs"

open Cana
open System

new ConsoleLogger() |> Api.SetLogger

new ConsoleLogger() |> Api.SetLogger

type TestApi(init) as this =
    inherit Api() 
    
    override this.Initialized with get() = init
    
    member x.A() = match true with | true -> Success this | false -> Exception("foo") |> Failure
    
    member x.B(b:TestApi) = match true with | true -> Success this | false -> Exception("foo") |> Failure

    

let f = !? TestApi(Success null)

//let z0 = !?> TestApi(Success null)

//let z1 = !?> TestApi(Failure null)

//let foo = !> t.B()

let z3 = bind0 f.B f.A

let z4 = f.A >=> f.B





