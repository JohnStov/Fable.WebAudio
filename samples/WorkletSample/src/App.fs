module App.View

open Elmish
open Fable.React
open Fable.React.Props
open Browser
open Fulma
open Fable.WebAudio

type Model =
    { context: AudioContext; state: AudioContextState; hasWorklets: bool  }

type Msg =
    | StopStart
    | OnStateChange
    | WorkletInitialised
    | NoWorklets

let setup (context : AudioContext) =
    let osc = context.createOscillator ()
    let amp = context.createGain ()
    let bypass = AudioWorkletNode.Create (context, "bypass-processor")
    osc.connect(bypass).connect(amp).connect(context.destination) |> ignore
    osc.start()
    context.suspend()

let init _ = 
  let context = AudioContext.Create()
  let state = {context=context; state=context.state; hasWorklets = false}
  let success () = WorkletInitialised
  let failure _ = NoWorklets
  if isNull context.audioWorklet then
    state, Cmd.ofMsg NoWorklets
  else
    state, Cmd.OfPromise.either context.audioWorklet.addModule "bypass.js" success failure

let private update msg model =
    let notifyStateChange () = OnStateChange
    
    match msg with
    | StopStart ->
      if model.context.state = Running then
        model, Cmd.OfPromise.perform model.context.suspend () notifyStateChange
      else
        model, Cmd.OfPromise.perform model.context.resume () notifyStateChange
    | OnStateChange -> 
      { model with state = model.context.state }, Cmd.none
    | WorkletInitialised ->
        setup model.context |> ignore
        { model with hasWorklets = true }, Cmd.ofMsg OnStateChange
    | NoWorklets -> 
        model, Cmd.none

let stopStartButton model dispatch = 
  if model.hasWorklets then 
    [ 
      Button.button 
        [
          Button.OnClick(fun _ -> dispatch StopStart) ] 
        [ str (if model.state = Suspended then "Resume" else "Suspend") ] ] 
  else
    [ str "This browser does not support audio worklets - try this in Chrome"]

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
                          Field.div [ ] (stopStartButton model dispatch) ] ] ] ] ]

open Elmish.Debug
open Elmish.HMR

Program.mkProgram init update view
|> Program.withReactSynchronous "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
