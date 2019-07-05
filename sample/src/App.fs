module App

open System
open Elmish
open Fable.Core
open App.Model
open App.State
open App.View

open Elmish.Debug
open Elmish.HMR

let timer _ =
  let sub dispatch =
    JS.setInterval (fun _ -> dispatch (Tick DateTime.Now)) 10 |> ignore
  Cmd.ofSub sub

Program.mkProgram init update view
|> Program.withReactSynchronous "elmish-app"
|> Program.withSubscription timer
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
