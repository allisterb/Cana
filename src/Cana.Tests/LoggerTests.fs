module LoggerTests

open System
open Xunit

open Cana

printfn "%i log file(s) deleted." <| IO.DeleteFiles true "." "*.log"

[<Fact>]
let ``Can create SerilogLogger ``() =
    
    let s = SerilogLogger()
    do s.Info("Hello")
    IO.GetFiles true "." "*.log" |> fst > 0 |> Assert.True
    
