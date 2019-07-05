module App.Model

open System
open Fable.WebAudio

type Model =
    { Context : AudioContext; Time: DateTime; Osc: OscillatorNode; Analyser: AnalyserNode }

type Msg =
    | Start
    | Stop
    | Tick of DateTime
    | Wave of OscillatorType
    | Frequency of float
    | Detune of float
