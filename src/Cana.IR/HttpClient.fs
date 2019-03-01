namespace Cana.IR

open System
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Threading
open System.Threading.Tasks

open Cana
open System.Threading

type HttpClient(url: string, contentType: HttpClientContentType, ?proxy: string, ?ct: CancellationToken) =
    inherit Api(?ct = ct)

    let mutable _Initialized = Failure null

    let urlParsed, urlResult = Uri.TryCreate (url, UriKind.Absolute)

    let proxyUrlParsed, proxyUrlResult = if !% proxy then Uri.TryCreate (proxy.Value, UriKind.Absolute) else false, null
        
    do
        if not urlParsed then 
            failwithf "Could not parse Url string %s." url 
        else if !% proxy && not proxyUrlParsed then
            failwithf "Could not parse proxy Url string %s." proxy.Value 
  
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
            do client.DefaultRequestHeaders.Add("user-agnt", HttpClient.UserAgent)

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

        
        
    