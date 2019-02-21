namespace Cana
open System
open System.Diagnostics

/// Logging interface
[<AbstractClass>]
type CanaApi() =
    do if CanaApi.Logger = None then failwith "A logger is not assigned." else ()

    static member val Logger :Logger option = None with get,set
    
    abstract member Info : messageTemplate:string * [<ParamArray>]args:obj[] -> unit
    abstract member Debug : messageTemplate:string * [<ParamArray>]args:obj[] -> unit

    