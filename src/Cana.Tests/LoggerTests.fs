module LoggerTests

open System
open System.IO
open Xunit

open Cana

[<Fact>]
let ``Can create SerilogLogger ``() =
    let s = SerilogLogger()
    do s.Info("Hello")
    File.Exists "Cana.log" |> Assert.True
    
