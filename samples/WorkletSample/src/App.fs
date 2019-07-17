module App.View

open Elmish
open Fable.React
open Fable.React.Props
open Fulma
open Fable.FontAwesome
open Fable.WebAudio
open Fable.Core.JsInterop

type Model =
    { context: AudioContext; state: AudioContextState  }

type Msg =
    | StopStart
    | OnStateChange

let init _ = 
  let context = AudioContext.Create()
  context.audioWorklet.addModule("bypass.js").``then`` (fun () -> 
      let osc = context.createOscillator ()
      let amp = context.createGain ()
      let bypass = AudioWorkletNode.Create (context, "bypass-processor")
      osc.connect(bypass).connect(amp).connect(context.destination) |> ignore
      osc.start()
      context.suspend()) |> ignore
  {context=context; state=context.state}, Cmd.none

let private update msg model =
    match msg with
    | StopStart ->
      if model.state = Running then
        model, Cmd.OfPromise.perform model.context.suspend () (fun () -> OnStateChange)
      else
        model, Cmd.OfPromise.perform model.context.resume ()  (fun () -> OnStateChange)
    | OnStateChange -> 
      { model with state = model.context.state }, Cmd.none

let private view model dispatch =
    Hero.hero [ Hero.IsFullHeight ]
        [ Hero.body [ ]
            [ Container.container [ ]
                [ Columns.columns [ Columns.CustomClass "has-text-centered" ]
                    [ Column.column [ Column.Width(Screen.All, Column.IsOneThird)
                                      Column.Offset(Screen.All, Column.IsOneThird) ]
                        [ Image.image [ Image.Is128x128
                                        Image.Props [ Style [ Margin "auto"] ] ]
                            [ img [ Src "assets/fulma_logo.svg" ] ]
                          Field.div [ ]
                            [ 
                              Button.button 
                                [
                                  Button.OnClick(fun _ -> dispatch StopStart) ] 
                                [ str (if model.state = Suspended then "Resume" else "Suspend") ]
                              ] ] ] ] ] ]

open Elmish.Debug
open Elmish.HMR

Program.mkProgram init update view
|> Program.withReactSynchronous "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
