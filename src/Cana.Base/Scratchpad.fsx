
#load "Logger.fs"
#load "Api.fs"

open Cana
open System

new ConsoleLogger() |> Api.SetLogger

type TestApi(init) as this =
    inherit Api() 
    
    override this.Initialized with get() = init
    
    member x.A() = match true with | true -> Success this | false -> Exception("foo") |> Failure
    
    member x.B(b:TestApi) = match true with | true -> Success this | false -> Exception("foo") |> Failure

    

let f = !? TestApi(Success null)

let z5 = f.A >=> f.B

let z6 = f.A() >>= f.B

z5()
z6




