module App.State

open System
open FSharp.Core
open Elmish
open Browser.Dom
open Browser.Types
open Fable.Core
open Fable.WebAudio

open App.Model

let draw model =
  let sampleData = Array.create model.Analyser.frequencyBinCount (byte 0)
  let freqData = Array.create model.Analyser.frequencyBinCount (byte 0)
  model.Analyser.getByteTimeDomainData sampleData
  model.Analyser.getByteFrequencyData freqData

  let canvas : HTMLCanvasElement = unbox document.getElementById "canvas"
  let ctx = canvas.getContext_2d ()

  let colWidth = canvas.width / float sampleData.Length
  let colHeight = canvas.height
  let scale (values: byte array) i =
    let xPos = float i * colWidth
    let yPos = colHeight - (float values.[i] / 256.0 * colHeight)
    xPos, yPos

  ctx.clearRect(0., 0., ctx.canvas.width, ctx.canvas.height)

  ctx.strokeStyle <-  U3.Case1 "red"
  for i in 0 .. freqData.Length-1 do
    let (x, y) = scale freqData i
    ctx.beginPath ()
    ctx.moveTo (x, ctx.canvas.height)
    ctx.lineTo (x, y)
    ctx.stroke ()

  ctx.strokeStyle <-  U3.Case1 "black"
  ctx.beginPath ()
  for i in 0 .. sampleData.Length-1 do
    ctx.lineTo (scale sampleData i)
  ctx.stroke ()

let createModel () =
  let ctx = AudioContext.Create ()
  let osc = ctx.createOscillator ()
  let analyser = ctx.createAnalyser ()
  let gain = ctx.createGain ()

  osc.frequency.value <- 440.
  gain.gain.value <- 0.01
  osc.connect analyser |> ignore
  analyser.connect gain |> ignore
  gain.connect ctx.destination |> ignore
  osc.start ()
  ctx.suspend () |> ignore

  { Context = ctx; Osc = osc; Analyser = analyser }

let redraw dispatch =
  window.requestAnimationFrame (fun _ -> dispatch Redraw) |> ignore

let drawSub _ =
  Cmd.ofSub redraw

let init _ =
  let model = createModel ()
  model, Cmd.none

let update msg model =
    match msg with
    | Start ->
        if model.Context.state = Suspended then
          model.Context.resume () |> ignore
        model, Cmd.none
    | Stop ->
        if model.Context.state = Running then
          model.Context.suspend () |> ignore
        model, Cmd.none
    | Wave w ->
        model.Osc.``type`` <- w
        model, Cmd.none
    | Frequency f ->
        model.Osc.frequency.value <- f
        model, Cmd.none
    | Detune f ->
        model.Osc.detune.value <- f
        model, Cmd.none
    | Redraw ->
        model |> draw
        model, Cmd.ofSub redraw

