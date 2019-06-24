namespace Fable

open Browser.Types
open Fable.Core
open Fable.Core.JS
open System.Security

module WebAudio =
    type [<AllowNullLiteral>] BaseAudioContext = 
        inherit EventTarget
        
        abstract destination: AudioDestinationNode with get
        abstract sampleRate: float with get
        abstract currentTime: float with get
        abstract listener: AudioListener with get
        abstract state: AudioContextState with get
        // abstract audioWorklet: AudioWorklet with get
        abstract onstatechange : (Event -> 'Out) with get, set

        abstract createAnalyser: unit -> AnalyserNode
        abstract createBiquadFilter: unit -> BiquadFilterNode
        abstract createBuffer: numberOfChannels: int * length: int * sampleRate: float -> AudioBuffer
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

    and [<StringEnum>] AudioContextState = 
        | Suspended 
        | Running 
        | Closed 

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

    and [<AllowNullLiteral>] AudioBuffer =
        abstract samplerate: float with get
        abstract length: int with get
        abstract duration: float with get
        abstract numberOfChannels: int with get
        abstract getChannelData: channel: int -> float array
        abstract copyFromChannel: destination: float array * channelNumber: int * ?startChannel: int -> unit
        abstract copyToChannel: source: float array * channelNumber: int * ?startChannel: int -> unit

    and [<AllowNullLiteral>] AudioBufferType = 
        abstract prototype: AudioBuffer with get, set
        [<Emit("new $0($1...)")>] abstract Create: options: AudioBufferOptions -> AudioBuffer

    and [<AllowNullLiteral>] AudioBufferOptions = 
        abstract numberOfChannels: int with get, set
        abstract length: int with get, set
        abstract sampleRate: float with get, set

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

    and [<AllowNullLiteral>] AudioNodeOptions =
        abstract channelCount: int with get, set
        abstract channelCountMode: ChannelCountMode with get, set
        abstract channelInterpretation: ChannelInterpretation with get, set

    and [<StringEnum>] ChannelCountMode = 
        | Max
        | [<CompiledName("clamped-max")>] ClampedMax
        | Explicit

    and [<StringEnum>] ChannelInterpretation = 
        | Speakers
        | Discrete

    and [<AllowNullLiteral>] AudioListener =
        abstract positionX: AudioParam with get
        abstract positionY: AudioParam with get
        abstract positionZ: AudioParam with get
        abstract forwardX: AudioParam with get
        abstract forwardY: AudioParam with get
        abstract forwardZ: AudioParam with get
        abstract upX: AudioParam with get
        abstract upY: AudioParam with get
        abstract upZ: AudioParam with get
        
        abstract setPosition: x: float * y: float * z: float -> unit
        abstract setOrientation: x: float * y: float * z: float * xUp: float * yUp: float * zUp: float -> unit
    
    and [<AllowNullLiteral>] AudioScheduledSourceNode = 
        inherit AudioNode

        abstract onended: (Event -> 'Out) with get, set
        abstract start: ?``when``: float -> unit
        abstract stop: ?``when``: float -> unit
        
    and [<AllowNullLiteral>] AnalyserNode =
        inherit AudioNode

        abstract fftSize: int with get, set
        abstract frequencyBinCount: int with get
        abstract minDecibels: float with get, set
        abstract maxDecibels: float with get, set
        abstract smoothingTimeConstant: float with get, set

        abstract getFloatFrequencyData: array: float array -> unit
        abstract getByteFrequencyData: array: int array -> unit
        abstract getFloatTimeDomainData: array: float array -> unit
        abstract getByteTimeDomainData: array: int array -> unit

    and [<AllowNullLiteral>] AnalyserNodeType = 
        abstract prototype: AnalyserNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: AnalyserOptions -> AnalyserNode

    and [<AllowNullLiteral>] AnalyserOptions =
        inherit AudioNodeOptions

        abstract fftSize: int with get, set
        abstract maxDecibels: float with get, set
        abstract minDecibels: float with get, set
        abstract smoothingTimeConstant: float with get, set

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

    and [<AllowNullLiteral>] BiquadFilterNode =
        inherit AudioNode

        abstract ``type``: BiquadFilterType with get, set
        abstract frequency: AudioParam with get
        abstract detune: AudioParam with get
        abstract Q: AudioParam with get
        abstract gain: AudioParam with get
        abstract getFrequencyResponse: frequencyHz: float array * magResponse: float array * phaseResponse: float array -> unit

    and [<StringEnum>] BiquadFilterType =
        | Lowpass
        | Highpass
        | Bandpass
        | Lowshelf
        | Highshelf
        | Peaking
        | Notch
        | AllowPartiallyTrustedCallersAttribute
    
    and [<AllowNullLiteral>] BiquadFilterNodeType =
        abstract prototype: BiquadFilterNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: BiquadFilterOptions -> BiquadFilterNode

    and [<AllowNullLiteral>] BiquadFilterOptions =
        inherit AudioNodeOptions

        abstract ``type``: BiquadFilterType with get, set
        abstract Q: float with get, set
        abstract detune: float with get, set
        abstract frequency: float with get, set
        abstract gain: float with get, set

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
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: BiquadFilterOptions -> OscillatorNode

    and [<AllowNullLiteral>] OscillatorOptions = 
        inherit AudioNodeOptions
        
        abstract ``type``: OscillatorType with get, set
        abstract frequency: float with get, set
        abstract detune: float with get, set
        // abstract periodicWave: PeriodicWave with get, set

    // and [<AllowNullLiteral>] AudioWorklet = 
    //    inherit Worklet

    let [<Global>] AudioContext: AudioContextType = jsNative
    let [<Global>] AudioBuffer: AudioBufferType = jsNative
    let [<Global>] AnalyserNode: AnalyserNodeType = jsNative
    let [<Global>] BiquadFilterNode: BiquadFilterNodeType = jsNative
    let [<Global>] OscillatorNode: OscillatorNodeType = jsNative
