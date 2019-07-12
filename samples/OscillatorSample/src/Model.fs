module App.Model

open System
open Fable.WebAudio

type Model =
    { Context : AudioContext; Osc: OscillatorNode; Analyser: AnalyserNode; Frequency: float; Detune: float; State: AudioContextState }

type Msg =
    | Start
    | Stop
    | Wave of OscillatorType
    | Frequency of float
    | Detune of float
    | Initialize
    | Animate
