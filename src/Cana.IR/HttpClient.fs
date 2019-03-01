namespace Cana.IR

open System
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Threading
open System.Threading.Tasks

open Cana
open System.Threading

type HttpClient(baseUrl: string, contentType: HttpClientContentType, ?proxy: string, ?ct: CancellationToken) =
    inherit Api(?ct = ct)

    let mutable _Initialized = false

    let urlParsed, urlResult = Uri.TryCreate (baseUrl, UriKind.Absolute)

    let proxyUrlParsed, proxyUrlResult = if !? proxy then Uri.TryCreate (proxy.Value, UriKind.Absolute) else false, null
        
    do
        if not urlParsed then 
            err "Could not parse Url string {0}." [baseUrl]
            
        else if !? proxy && not proxyUrlParsed then
            err "Could not parse proxy Url string {0}." [proxy.Value] 
        else _Initialized <- true
  
    static member UserAgent = "Cana"

    override x.Initialized with get() = _Initialized

    member x.Url = urlResult
   
    member x.ProxyUrl = proxyUrlResult

    member x.ContentType = contentType

    member x.GetAsync (query:string) :Async<HttpClientResponse> =        
        async {
            use client = new SystemHttpClient()
            client.BaseAddress <- x.Url
            do client.DefaultRequestHeaders.Accept.Clear()
            do client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue(x.ContentType.Str))
            do client.DefaultRequestHeaders.Add("user-agent", HttpClient.UserAgent)

            let! r = client.GetAsync(query, x.CancellationToken) |> Async.AwaitTask
            let! content = r.IsSuccessStatusCode ? (r.Content.ReadAsStringAsync(), Task.FromResult "") |> Async.AwaitTask 
            return {Success = r.IsSuccessStatusCode; StatusCode = (int) r.StatusCode; Content = content}
        }

and SystemHttpClient = System.Net.Http.HttpClient

and HttpClientContentType =
    | Json
    | Html
    with member x.Str with get() = match x with |Json -> "application/json" |Html -> "text/html"
    
and HttpClientResponse = {Success: bool; StatusCode: int; Content: string}

        
        
    