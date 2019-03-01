namespace Cana

open System
open System.Diagnostics
open System.Threading.Tasks
open System.Threading


[<AbstractClass>]
type Api(?ct: CancellationToken) =
    
    do if Api.Logger = None then failwith "A logger is not assigned." else ()

    static member val private Logger :Logger option = None with get,set
    static member SetLogger logger = if Api.Logger.IsNone then Api.Logger <- Some(logger) else failwith "A logger is already assigned."
    static member CTS = new CancellationTokenSource()

    static member Info(messageTemplate:string, [<ParamArray>]args:obj[]) = Api.Logger.Value.Info(messageTemplate, args)
    static member Debug(messageTemplate:string, [<ParamArray>]args:obj[]) = Api.Logger.Value.Debug(messageTemplate, args)
    static member Error(messageTemplate:string, [<ParamArray>]args:obj[]) = Api.Logger.Value.Error(messageTemplate, args)

    abstract Initialized: ApiResult<obj, obj> with get

    member x.CancellationToken = if ct.IsSome then ct.Value else Api.CTS.Token 
    
    static member (!?) (o : 'T when 'T :> Api) =
        match o.Initialized with
        | Success _ -> o
        | Failure _ -> failwith "Api object not initialized."

    static member (!?>) (o : 'T when 'T :> Api) =
        match o.Initialized with
        | Success _ -> Success o
        | Failure _ -> Exception "Api object not initialized." |> Failure
    
and ApiResult<'TSuccess,'TFailure> = 
    | Success of 'TSuccess
    | Failure of 'TFailure

[<AutoOpen>]
module Api =

    let (|Default|) defaultValue input =    
        defaultArg input defaultValue

    let bind f = 
        function
        | Success value -> f value
        | Failure failure -> Failure failure

    let bind0 f (v: unit -> ApiResult<'T, Exception>)  =
        try
            v() |> bind f
        with
        | ex -> Failure ex
    
    let bind1 (f: 'T -> ApiResult<'R, Exception>) (v: 'a -> ApiResult<'T, Exception>)  (x1:'a)=
        try
            v x1 |> bind f
        with
        | ex -> Failure ex

    let bind2 f (v: 'a -> 'b -> ApiResult<'T, Exception>)  (x1:'a) (x2:'b)=
        try
            v x1 x2 |> bind f
        with
        | ex -> Failure ex

    let inline (>>=) f1 f2 = 
        bind f2 f1 

    let inline (>=>) f1 f2 = 
        f1 >> (bind f2)
    
    let inline (!>) f1 f2 = bind0 f2 f1

    let inline (!>=) f1  f2= bind1 f2 f1

    let inline (!%) (x:Option<_>) = x.IsSome
    
    let inline (?) (l:bool) (r, j) = if l then r else j

 
    

    