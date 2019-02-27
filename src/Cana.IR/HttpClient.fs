namespace Cana.IR

open System
open System.IO
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Threading
open System.Threading.Tasks

open Cana
open System.Runtime.Serialization.Json

type SystemHttpClient = System.Net.Http.HttpClient

type HttpClientContentType =
    | Json
    | Html

type HttpClientResponse = {Success: bool; StatusCode: int; Content: string}

type HttpClient(url: string, contentType: HttpClientContentType, ?proxy: string, ?ct: CancellationToken) =
    inherit Api(?ct = ct)

    let urlParsed, urlResult = Uri.TryCreate (url, UriKind.Absolute)

    let proxyUrlParsed, proxyUrlResult = if % proxy then Uri.TryCreate (proxy.Value, UriKind.Absolute) else false, null
        
    let contentTypeStr ct = 
        match ct with
        | Json -> "application/json"
        | Html -> "text/html"

    do
        if not urlParsed then 
            failwithf "Could not parse Url string %s." url 
        else if % proxy && not proxyUrlParsed then
            failwithf "Could not parse proxy Url string %s." proxy.Value 
  
    static member UserAgent = "Cana"

    member x.Url = urlResult
   
    member x.ProxyUrl = proxyUrlResult

    member x.ContentType = contentType

    member x.ContentTypeStr = contentTypeStr x.ContentType

    member x.GetAsync (query:string) :Async<HttpClientResponse> =        
        async {
            use client = new SystemHttpClient()
            client.BaseAddress <- x.Url
            do client.DefaultRequestHeaders.Accept.Clear()
            do client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue(x.ContentTypeStr))
            do client.DefaultRequestHeaders.Add("user-agnt", HttpClient.UserAgent)

            let! r = client.GetAsync(query, x.CancellationToken) |> Async.AwaitTask
            let! content = r.IsSuccessStatusCode ? (r.Content.ReadAsStringAsync(), Task.FromResult "") |> Async.AwaitTask 
            return {Success = r.IsSuccessStatusCode; StatusCode = (int) r.StatusCode; Content = content}
        }
        
        
    