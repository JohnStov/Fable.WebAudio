namespace Fable

open Browser.Types
open Fable.Core
open Fable.Core.JS

module WebAudio =
    type [<AllowNullLiteral>] BaseAudioContext = 
        inherit EventTarget
        
        abstract destination: AudioDestinationNode with get
        abstract sampleRate: float with get
        abstract currentTime: float with get
        // abstract listener: AudioListener with get
        // abstract state: AudioContextState with get
        // abstract audioWorklet: AudioWorklet with get
        abstract onstatechange : (Event -> 'Out) with get, set

        // abstract createAnalyser: unit -> AnalyserNode
        // abstract createBiquadFilter: unit -> BiquadFilterNode
        // abstract createBuffer: numberOfChannels: int * length: int * sampleRate: float -> BiquadFilterNode
        // abstract createBufferSource: unit -> AudioBufferSourceNode
        // abstract createChannelMerger: ?numberOfInputs: int -> ChannelMergerNode
        // abstract createChannelSplitter: ?numberOfOutputs: int -> ChannelSplitterNode
        // abstract createConstantSource: unit -> ConstantSourceNode
        // abstract createConvolver: unit -> ConvolverNode
        // abstract createDelay: ?maxDelayTime: float -> DelayNode
        // abstract createDynamicsCompressor: unit -> DynamicsCompressorNode
        // abstract createGain: unit -> GainNode
        // abstract createIIRFilter: feedforward: float seq * feedback: float seq -> IIRFilterNode
         abstract createOscillator: unit -> OscillatorNode
        // abstract createPanner: unit -> PannerNode
        // abstract createPeriodicWave: real: float seq * imag: float seq * ?constrants: PeriodicWaveConstraints -> PeriodicWave
        // abstract createScriptProcessor: ?bufferSize: int * ?numberOfInputChannels: int * ?numberOfOutputChannels: int -> ScriptProcessorNode
        // abstract createStereoPanner: unit -> StereoPannerNode
        // abstract createWaveShaper: unit -> WaveShaperNode
        // abstract decodeAudioData: audioData: ArrayBuffer * ?successCallback: (AudioBuffer -> unit) * ?errorCallback: (DOMException -> unit) -> Promise<AudioBuffer>
        abstract resume: unit -> Promise<unit>

    and [<AllowNullLiteral>] AudioContext = 
        inherit BaseAudioContext

        abstract baseLatency: float with get
        abstract outputLatency: float with get

        abstract close: unit -> Promise<unit>
        // abstract createMediaElementSource: mediaElement: HTMLMediaElement -> MediaElementSourceNode
        // abstract createMediaStreamDestination: unit -> MediaStreamAudioDestinationNode
        // abstract createMediaStreamSource: mediaStream: MediaStream -> MediaStreamAudioSourceNode
        // abstract createMediaStreamTrackSource: mediaStreamTrack: MediaStreamTrack -> MediaStreamTrackAudioSourceNode
        // abstract getOutputTimestamp : unit -> AudioTimestamp
        abstract suspend: unit -> Promise<unit>

    and [<AllowNullLiteral>] AudioContextOptions = 
        abstract latencyHint: AudioContextLatencyCategory
        abstract sampleRate: float

    and [<StringEnum>] AudioContextLatencyCategory = 
        | Balanced 
        | Interactive 
        | Playback 

    and [<AllowNullLiteral>] AudioContextType = 
        abstract prototype: AudioContext with get, set
        [<Emit("new $0($1...)")>] abstract Create: ?contextOptions: AudioContextOptions -> AudioContext

    and [<AllowNullLiteral>] AudioNode = 
        inherit EventTarget

        abstract context: BaseAudioContext with get
        abstract numberOfInputs: int with get
        abstract numberOfOutputs: int with get
        abstract channelCount: int with get, set
        // abstract channelCountMode: ChannelCountMode with get, set
        // abstract channelInterpretation: ChannelInterpretation with get, set

        abstract connect: destinationNode : AudioNode * ?output: int * ?input: int -> AudioNode
        // abstract connect: destinationParam: AudioParam * ?output: int * ?input: int -> unit
        abstract disconnect: unit -> unit
        abstract disconnect: destinationNode: AudioNode -> unit
        abstract disconnect: destinationNode: AudioNode * output: int -> unit
        abstract disconnect: destinationNode: AudioNode * output: int * input: int -> unit
        // abstract disconnect: destinationParam: AudioParam -> unit
        // abstract disconnect: destinationParam: AudioParam * output: int -> unit

    and [<AllowNullLiteral>] AudioScheduledSourceNode = 
        inherit AudioNode

        abstract onended: (Event -> 'Out) with get, set
        abstract start: ?``when``: float -> unit
        abstract stop: ?``when``: float -> unit

    
    and [<AllowNullLiteral>] AudioDestinationNode = 
        inherit AudioNode

        abstract maxChannelCount: int with get

    and [<AllowNullLiteral>] AudioParam = 
        abstract value: float with get, set
        abstract automationRate: AutomationRate with get, set    
        abstract defaultValue: float with get
        abstract minValue: float with get
        abstract maxValue: float with get

        abstract setValueAtTime: value: float * startTime: float -> AudioParam
        abstract linearRampToValueAtTime:  value: float * startTime: float -> AudioParam
        abstract exponentialRampToValueAtTime:  value: float * startTime: float -> AudioParam
        abstract setTargetAtTime:  target: float * startTime: float * timeConstant: float -> AudioParam
        abstract setValueCurveAtTime:  values: float seq * startTime: float * duration: float -> AudioParam
        abstract cancelScheduledValues:  cancelTime: float -> AudioParam
        abstract cancelAndHoldAtTime:  cancelTime: float -> AudioParam

    and [<StringEnum>] AutomationRate = 
        | [<CompiledName("a-rate")>] A_Rate
        | [<CompiledName("k-rate")>] K_rate

    and [<AllowNullLiteral>] OscillatorNode = 
        inherit AudioScheduledSourceNode

        // abstract detune: AudioParam with get
        abstract frequency: AudioParam with get
        abstract ``type``: OscillatorType with get, set

        abstract close: unit -> Promise<unit>
        // abstract createMediaElementSource: mediaElement: HTMLMediaElement -> MediaElementSourceNode
        // abstract createMediaStreamDestination: unit -> MediaStreamAudioDestinationNode
        // abstract createMediaStreamSource: mediaStream: MediaStream -> MediaStreamAudioSourceNode
        // abstract createMediaStreamTrackSource: mediaStreamTrack: MediaStreamTrack -> MediaStreamTrackAudioSourceNode
        // abstract getOutputTimestamp : unit -> AudioTimestamp
        abstract suspend: unit -> Promise<unit>

    and [<StringEnum>] OscillatorType = 
        | Sine 
        | Square 
        | Sawtooth 
        | Triangle 
        | Custom 

    and [<AllowNullLiteral>] OscillatorNodeType = 
        abstract prototype: OscillatorNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: OscillatorOptions -> OscillatorNode

    and [<AllowNullLiteral>] OscillatorOptions = 
        abstract ``type``: OscillatorType with get, set
        abstract frequency: float with get, set
        abstract detune: float with get, set
        // abstract periodicWave: PeriodicWave with get, set

    let [<Global>] AudioContext: AudioContextType = jsNative
    let [<Global>] OscillatorNode: OscillatorNodeType = jsNative
