namespace Cana

open System
open System.Diagnostics
open System.Threading.Tasks
open System.Threading

/// Base class for Cana API.
[<AbstractClass>]
type Api(?ct: CancellationToken) =
    
    do if Api.Logger = None then failwith "A logger is not assigned."

    static member val private Logger :Logger option = None with get,set
    static member SetLogger logger = if Api.Logger.IsNone then Api.Logger <- Some(logger) else failwith "A logger is already assigned."
    static member SetLoggerIfNone logger = if Api.Logger.IsNone then Api.Logger <- Some(logger)
    static member SetDefaultLoggerIfNone() = if Api.Logger.IsNone then Api.Logger <- Some <| upcast ConsoleLogger()
    
    static member Cts = new CancellationTokenSource()

    static member Info(messageTemplate:string, [<ParamArray>]args:obj[]) = Api.Logger.Value.Info(messageTemplate, args)
    static member Debug(messageTemplate:string, [<ParamArray>]args:obj[]) = Api.Logger.Value.Debug(messageTemplate, args)
    static member Error(messageTemplate:string, [<ParamArray>]args:obj[]) = Api.Logger.Value.Error(messageTemplate, args)
    static member Error s = Api.Error(s)

    abstract Initialized: bool with get

    member x.CancellationToken = if (ct.IsSome) then ct.Value else Api.Cts.Token 
    
and ApiResult<'TSuccess,'TFailure> = 
    | Success of 'TSuccess
    | Failure of 'TFailure
    
///Global functions and operators belonging to the Cana API.
[<AutoOpen>]
module Api =
     //Logging functions

    let inline info mt args = Api.Info(mt, List.toArray args)

    let inline debug mt args = Api.Debug(mt, List.toArray args)

    let inline err mt args = Api.Error(mt, List.toArray args)

    let inline errex mt args = Api.Error(mt, List.toArray args)

    let (|Default|) defaultValue input =    
        defaultArg input defaultValue
    
    let tryCatch f x =
        try
            f x |> Success
        with
        | ex -> Failure ex

    let tryCatch' f x =
        try
            f x 
        with
        | ex -> 
            err "jj" []
            Failure ex

    
    let tryCatchAsync' f x = tryCatch' (f >> Async.RunSynchronously) <| x
    

    let bind f = 
        function
        | Success value -> f value |> Success
        | Failure failure -> Failure failure

    let bind' f = 
        function
        | Success value -> f value
        | Failure failure -> Failure failure

   
    let inline (?) (l:bool) (r, j) = if l then r else j

    let inline (!?) (x:Option<_>) = x.IsSome

    let inline (!!) (x: 'T when 'T :> Api) = if x.Initialized then x else failwith "This Api object is not initialized."

    let inline (!>) (x: 'T when 'T :> Api) = if x.Initialized then Success x else exn "This Api object is not initialized." |> Failure

    let inline (!>>) f = tryCatch' f   
    
    let inline (!>>>) f = tryCatchAsync' f

    let inline (!>?) result = match result with | Success _ -> true | Failure _ -> false

    let inline (!>=) result = match result with | Success s -> s | Failure f -> failwith "This Api result was a failure."

    let inline (>>=) f1 f2  = bind' f2 f1 

    let inline (>=>) f1 f2 = tryCatch' f1 >> (bind' f2)

    let inline (>=>>) f1 f2 =  tryCatch' f1 >> f2 

   

   
    
    