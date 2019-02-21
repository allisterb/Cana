namespace Cana

[<AutoOpen>]
module Api = 
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



    type State<'Event> =
        | Next of ('Event -> State<'Event>)
        | Stop

    let feed state event =
        match state with
        | Stop -> failwith "Terminal state reached"
        | Next handler -> event |> handler

    type StateMachine<'event>(initial: State<'event>) =
        let mutable current = initial
        member this.Fire event = current <- feed current event
        member this.IsStopped 
            with get () = match current with | Stop -> true | _ -> false

    let createMachine initial = StateMachine(initial)

    let createAgent initial = 
        MailboxProcessor.Start (fun inbox -> 
            let rec loop state = async {
                let! event = inbox.Receive ()
                match event |> feed state with
                | Stop -> ()
                | Next _ as next -> return! loop next
            }
            loop initial
        )

    let (|Default|) defaultValue input =
        defaultArg input defaultValue


