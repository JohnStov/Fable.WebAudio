namespace Fable

open Browser.Types
open Fable.Core
open Fable.Core.JS

module Worklet =
    type [<AllowNullLiteral>] Worklet =
        abstract addModule: moduleURl: string * ?options: WorkletOptions -> Promise<unit>

    and [<AllowNullLiteral>] WorkletOptions =
        abstract credentials: CredentialsMode

    and [<StringEnum>] CredentialsMode =
        | Omit
        | [<CompiledName("same-origin")>] SameOrigin
        | Include

    and [<AllowNullLiteral>] WorkletGlobalScope =
        interface end

    and [<AllowNullLiteral>] MessagePort =
        inherit EventTarget

        abstract postMessage: message: obj * transfer: obj seq -> unit
        abstract postMessage: message: obj * ?options: PostMessageOptions -> unit
        abstract start: unit -> unit
        abstract close: unit -> unit
        abstract onmessage: (Event -> 'Out) with get, set
        abstract onmessageerror: (Event -> 'Out) with get, set

    and [<AllowNullLiteral>] PostMessageOptions =
        abstract transfer: obj seq with get, set
    
