module App

open System
open Elmish
open Fable.Core
open App.Model
open App.State
open App.View

open Elmish.Debug
open Elmish.HMR

Program.mkProgram init update view
|> Program.withReactSynchronous "elmish-app"
|> Program.withSubscription drawSub
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
