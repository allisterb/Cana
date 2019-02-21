namespace Cana
open System

/// Logging interface
[<AbstractClass>]
type Logger() =
    static member val IsConfigured = false with get,set
    
    abstract member Info : messageTemplate:string * [<ParamArray>]args:obj[] -> unit
    abstract member Debug : messageTemplate:string * [<ParamArray>]args:obj[] -> unit
    abstract member Error : messageTemplate:string * [<ParamArray>]args:obj[] -> unit
    abstract member Error : ex:Exception * messageTemplate:string * [<ParamArray>]args:obj[] -> unit

    