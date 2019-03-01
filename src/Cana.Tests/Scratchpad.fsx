#r @"./bin/Debug/net461/System.Net.Http.dll"
#r @"./bin/Debug/net461/Cana.Base.dll"
#r @"./bin/Debug/net461/Cana.IR.dll"
#r @"./bin/Debug/net461/Cana.Logger.Serilog.dll"

open Cana
open Cana.IR

Api.SetLogger <| new SerilogLogger()
let c = HttpClient("https://www.google.com", Html)
let g = c.GetAsync "/" |> Async.RunSynchronously
