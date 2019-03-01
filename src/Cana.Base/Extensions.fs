namespace Cana

open System.Collections.Generic

module Extensions =

    type Dictionary<'TKey, 'TValue> with
            member this.TryFind key =
                let value = ref (Unchecked.defaultof<'TValue>)
                if this.TryGetValue (key, value) then Some !value
                else None

