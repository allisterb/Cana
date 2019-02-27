namespace Cana

open System
open System.Diagnostics
open System.Threading.Tasks
open System.Threading

[<AbstractClass>]
type Api(?ct: CancellationToken) =
    do if Api.Logger = None then failwith "A logger is not assigned." else ()

    static member val Logger :Logger option = None with get,set

    static member CTS = new CancellationTokenSource()

    static member Info(messageTemplate:string, [<ParamArray>]args:obj[]) = Api.Logger.Value.Info(messageTemplate, args)
    static member Debug(messageTemplate:string, [<ParamArray>]args:obj[]) = Api.Logger.Value.Debug(messageTemplate, args)
    static member Error(messageTemplate:string, [<ParamArray>]args:obj[]) = Api.Logger.Value.Error(messageTemplate, args)

    member x.CancellationToken = if ct.IsSome then ct.Value else Api.CTS.Token 

[<AutoOpen>]
module ApiLogic = 
    let inline (~%) (x:Option<_>) = x.IsSome
    
    let SetLogger logger = 
        if % Api.Logger then
            failwith "A logger is already assigned."
        else Api.Logger <- Some(logger)

    type ApiResult<'TSuccess,'TFailure> = 
        | Success of 'TSuccess
        | Failure of 'TFailure

    let bind switchFunction = 
        function
        | Success s -> switchFunction s
        | Failure f -> Failure f

    let (>>=) twoTrackInput switchFunction = 
        bind switchFunction twoTrackInput 


    let (>=>) switch1 switch2 = 
        switch1 >> (bind switch2)
    (*
        match switch1 x with
        | Success s -> switch2 s
        | Failure f -> Failure f 
    *)

    let tryCatch f x =
        try
            f x |> Success
        with
        | ex -> Failure ex.Message

    let (|Default|) defaultValue input =
        defaultArg input defaultValue

    

    let inline (?) (l:bool) (r, j) = if l then r else j