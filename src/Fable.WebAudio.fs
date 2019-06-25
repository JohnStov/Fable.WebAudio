namespace Fable

open Browser.Types
open Fable.Core
open Fable.Core.JS
open System

module WebAudio =
    type [<AllowNullLiteral>] BaseAudioContext = 
        inherit EventTarget
        
        abstract destination: AudioDestinationNode with get
        abstract sampleRate: float with get
        abstract currentTime: float with get
        abstract listener: AudioListener with get
        abstract state: AudioContextState with get
        abstract onstatechange : (Event -> 'Out) with get, set

        abstract createAnalyser: unit -> AnalyserNode
        abstract createBiquadFilter: unit -> BiquadFilterNode
        abstract createBuffer: numberOfChannels: int * length: int * sampleRate: float -> AudioBuffer
        abstract createBufferSource: unit -> AudioBufferSourceNode
        abstract createChannelMerger: ?numberOfInputs: int -> ChannelMergerNode
        abstract createChannelSplitter: ?numberOfOutputs: int -> ChannelSplitterNode
        abstract createConstantSource: unit -> ConstantSourceNode
        abstract createConvolver: unit -> ConvolverNode
        abstract createDelay: ?maxDelayTime: float -> DelayNode
        abstract createDynamicsCompressor: unit -> DynamicsCompressorNode
        abstract createGain: unit -> GainNode
        abstract createIIRFilter: feedforward: float seq * feedback: float seq -> IIRFilterNode
        abstract createOscillator: unit -> OscillatorNode
        abstract createPanner: unit -> PannerNode
        abstract createPeriodicWave: real: float seq * imag: float seq * ?constrants: PeriodicWaveConstraints -> PeriodicWave
        [<Obsolete>]
        abstract createScriptProcessor: ?bufferSize: int * ?numberOfInputChannels: int * ?numberOfOutputChannels: int -> ScriptProcessorNode
        abstract createStereoPanner: unit -> StereoPannerNode
        abstract createWaveShaper: unit -> WaveShaperNode
        abstract decodeAudioData: audioData: ArrayBuffer * ?successCallback: (AudioBuffer -> unit) * ?errorCallback: (DOMException -> unit) -> Promise<AudioBuffer>
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
        abstract getOutputTimestamp : unit -> AudioTimestamp
        abstract suspend: unit -> Promise<unit>

    and [<AllowNullLiteral>] AudioContextOptions = 
        abstract latencyHint: AudioContextLatencyCategory with get, set
        abstract sampleRate: float with get, set

    and [<AllowNullLiteral>] AudioTimestamp = 
        abstract contextTime: double with get, set
        abstract performanceTime: double with get, set

    and [<StringEnum>] AudioContextLatencyCategory = 
        | Balanced 
        | Interactive 
        | Playback 

    and [<AllowNullLiteral>] AudioContextType = 
        abstract prototype: AudioContext with get, set
        [<Emit("new $0($1...)")>] abstract Create: ?contextOptions: AudioContextOptions -> AudioContext

    and [<AllowNullLiteral>] OfflineAudioContext =
        inherit BaseAudioContext

        abstract startRendering: unit -> Promise<AudioBuffer>
        abstract suspend: suspendTime: float -> Promise<unit>
        abstract length: int with get
        abstract oncomplete: (OfflineAudioCompletionEvent -> 'Out) with get, set
    
    and [<AllowNullLiteral>] OfflineAudioContextType =
        abstract prototype: OfflineAudioContext with get, set
        [<Emit("new $0($1...)")>] abstract Create: ?contextOptions: OfflineAudioContextOptions -> OfflineAudioContext

    and [<AllowNullLiteral>] OfflineAudioContextOptions =
        abstract numberOfChannels: int with get, set
        abstract length: int with get, set
        abstract sampleRate: float with get, set

    and [<AllowNullLiteral>] OfflineAudioCompletionEvent =
        inherit Event

        abstract renderedBuffer: AudioBuffer with get

    and [<AllowNullLiteral>] OfflineAudioCompletionEventType =
        abstract prototype: OfflineAudioCompletionEvent with get, set
        [<Emit("new $0($1...)")>] abstract Create: ?eventInitDict: OfflineAudioCompletionEventInit -> OfflineAudioCompletionEvent

    and [<AllowNullLiteral>] OfflineAudioCompletionEventInit =
        inherit EventInit

        abstract renderedBuffer: AudioBuffer with get, set

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
        abstract channelCountMode: ChannelCountMode with get, set
        abstract channelInterpretation: ChannelInterpretation with get, set

        abstract connect: destinationNode : AudioNode * ?output: int * ?input: int -> AudioNode
        abstract connect: destinationParam: AudioParam * ?output: int * ?input: int -> unit
        abstract disconnect: unit -> unit
        abstract disconnect: destinationNode: AudioNode -> unit
        abstract disconnect: destinationNode: AudioNode * output: int -> unit
        abstract disconnect: destinationNode: AudioNode * output: int * input: int -> unit
        abstract disconnect: destinationParam: AudioParam -> unit
        abstract disconnect: destinationParam: AudioParam * output: int -> unit

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

    and [<AllowNullLiteral>] AudioBufferSourceNode =
        inherit AudioScheduledSourceNode

        abstract buffer: AudioBuffer with get, set
        abstract playbackRate: AudioParam with get
        abstract detune: AudioParam with get
        abstract loop: bool with get, set
        abstract loopStart: float with get, set
        abstract loopEnd: float with get, set
        abstract start: ?``when``: float * ?offset: float * ?duration: float -> unit
    
    and [<AllowNullLiteral>] AudioBufferSourceNodeType = 
        abstract prototype: AudioBufferSourceNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: AudioBufferSourceOptions -> AudioBufferSourceNode

    and [<AllowNullLiteral>] AudioBufferSourceOptions =
        abstract buffer: AudioBuffer with get, set
        abstract detune: float with get, set
        abstract loop: bool with get, set
        abstract loopEnd: float with get, set
        abstract loopStart: float with get, set
        abstract playbackRate: float with get, set

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

    and [<AllowNullLiteral>] ChannelMergerNode =
        inherit AudioNode

    and [<AllowNullLiteral>] ChannelMergerNodeType =
        abstract prototype: ChannelMergerNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: ChannelMergerOptions -> ChannelMergerNode

    and [<AllowNullLiteral>] ChannelMergerOptions =
        inherit AudioNodeOptions

        abstract numberOfOutputs: int with get, set
    
    and [<AllowNullLiteral>] ChannelSplitterNode =
        inherit AudioNode

    and [<AllowNullLiteral>] ChannelSplitterNodeType =
        abstract prototype: ChannelSplitterNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: ChannelSplitterOptions -> ChannelSplitterNode

    and [<AllowNullLiteral>] ChannelSplitterOptions =
        inherit AudioNodeOptions

        abstract numberOfOutputs: int with get, set

    and [<AllowNullLiteral>] ConstantSourceNode =
        inherit AudioScheduledSourceNode

        abstract offset: AudioParam with get

    and [<AllowNullLiteral>] ConstantSourceNodeType =
        abstract prototype: ConstantSourceNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: ConstantSourceOptions -> ConstantSourceNode

    and [<AllowNullLiteral>] ConstantSourceOptions =
        abstract offset: float with get, set
    
    and [<AllowNullLiteral>] ConvolverNode =
        inherit AudioNode

        abstract audioBuffer: AudioBuffer with get, set
        abstract normalize: bool with get, set

    and [<AllowNullLiteral>] ConvolverNodeType =
        abstract prototype: ConvolverNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: ConvolverOptions -> ConvolverNode

    and [<AllowNullLiteral>] ConvolverOptions =
        inherit AudioNodeOptions

        abstract buffer: AudioBuffer with get, set
        abstract disableNormalization: bool with get, set
    
    and [<AllowNullLiteral>] DelayNode =
        inherit AudioNode

        abstract delayTime: AudioParam with get

    and [<AllowNullLiteral>] DelayNodeType =
        abstract prototype: DelayNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: DelayOptions -> DelayNode

    and [<AllowNullLiteral>] DelayOptions =
        inherit AudioNodeOptions

        abstract maxDelayTime: float with get, set
        abstract delayTime: float with get, set
    
    and [<AllowNullLiteral>] DynamicsCompressorNode =
        inherit AudioNode

        abstract threshold: AudioParam with get
        abstract knee: AudioParam with get
        abstract ratio: AudioParam with get
        abstract reduction: float with get
        abstract attack: AudioParam with get
        abstract release: AudioParam with get

    and [<AllowNullLiteral>] DynamicsCompressorNodeType =
        abstract prototype: DynamicsCompressorNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: DynamicsCompressorOptions -> DynamicsCompressorNode

    and [<AllowNullLiteral>] DynamicsCompressorOptions =
        inherit AudioNodeOptions

        abstract attack: float with get, set
        abstract knee: float with get, set
        abstract ratio: float with get, set
        abstract release: float with get, set
        abstract threshold: float with get, set
    
    and [<AllowNullLiteral>] GainNode =
        inherit AudioNode

        abstract gain: AudioParam with get

    and [<AllowNullLiteral>] GainNodeType =
        abstract prototype: GainNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: GainOptions -> GainNode

    and [<AllowNullLiteral>] GainOptions =
        inherit AudioNodeOptions

        abstract gain: float with get, set
    
    and [<AllowNullLiteral>] IIRFilterNode =
        inherit AudioNode

        abstract getFrequencyResponse: frequencyHz: float array * magResponse: float array * phaseResponse: float array -> unit

    and [<AllowNullLiteral>] IIRFilterNodeType =
        abstract prototype: IIRFilterNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: IIRFilterOptions -> IIRFilterNode

    and [<AllowNullLiteral>] IIRFilterOptions =
        inherit AudioNodeOptions

        abstract feedforward: float seq with get, set
        abstract feedback: float seq with get, set
    
    and [<AllowNullLiteral>] OscillatorNode = 
        inherit AudioScheduledSourceNode

        abstract ``type``: OscillatorType with get, set
        abstract frequency: AudioParam with get
        abstract detune: AudioParam with get
        abstract setPeriodicWave: periodicWave: PeriodicWave -> unit

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
        abstract periodicWave: PeriodicWave with get, set

    and [<AllowNullLiteral>] PannerNode = 
        inherit AudioNode

        abstract panningModel: PanningModelType with get, set
        abstract positionX: AudioParam with get
        abstract positionY: AudioParam with get
        abstract positionZ: AudioParam with get
        abstract orientationX: AudioParam with get
        abstract orientationY: AudioParam with get
        abstract orientationZ: AudioParam with get
        abstract distanceMode: DistanceModelType with get, set
        abstract refDistance: float with get, set
        abstract maxDistance: float with get, set
        abstract rolloffFactor: float with get, set
        abstract coneInnerAngle: float with get, set
        abstract coneOuterAngle: float with get, set
        abstract coneOuterGain: float with get, set
        abstract setPosition: x: float * y: float * z: float -> unit
        abstract setOrientation: x: float * y: float * z: float -> unit


    and [<StringEnum>] PanningModelType = 
        | [<CompiledName("equalpower")>]EqualPower 
        | [<CompiledName("HRTF")>]HRTF

    and [<StringEnum>] DistanceModelType = 
        | Linear 
        | Inverse
        | Exponential

    and [<AllowNullLiteral>] PannerNodeType = 
        abstract prototype: PannerNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: PannerOptions -> PannerNode

    and [<AllowNullLiteral>] PannerOptions = 
        inherit AudioNodeOptions
        
        abstract panningModel: PanningModelType with get, set
        abstract distanceModel: DistanceModelType with get, set
        abstract positionX: float with get, set
        abstract positionY: float with get, set
        abstract positionZ: float with get, set
        abstract orientationX: float with get, set
        abstract orientationY: float with get, set
        abstract orientationZ: float with get, set
        abstract refDistance: float with get, set
        abstract maxDistance: float with get, set
        abstract rolloffFactor: float with get, set
        abstract coneInnerAngle: float with get, set
        abstract coneOuterAngle: float with get, set
        abstract coneOuterGain: float with get, set

    and PeriodicWave = Object

    and [<AllowNullLiteral>] PeriodicWaveType =
        abstract prototype: PeriodicWave with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: PeriodicWaveOptions -> PeriodicWave

    and [<AllowNullLiteral>] PeriodicWaveConstraints =
        abstract disableNormalization: bool with get, set

    and [<AllowNullLiteral>] PeriodicWaveOptions =
        inherit PeriodicWaveConstraints

        abstract real: float seq with get, set
        abstract imag: float seq with get, set
    
    
    and [<Obsolete>] [<AllowNullLiteral>] ScriptProcessorNode =
        inherit AudioNode

        abstract onaudioprocess: (Event -> 'Out) with get, set
        abstract bufferSize: int with get

    and [<AllowNullLiteral>] StereoPannerNode = 
        inherit AudioNode

        abstract pan: AudioParam with get

    and [<AllowNullLiteral>] StereoPannerNodeType =
        abstract prototype: StereoPannerNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: StereoPannerOptions -> StereoPannerNode

    and [<AllowNullLiteral>] StereoPannerOptions = 
        inherit AudioNodeOptions

        abstract pan: float with get, set
   
    and [<AllowNullLiteral>] WaveShaperNode = 
        inherit AudioNode

        abstract curve: float array with get, set
        abstract oversample: OverSampleType with get, set

    and [<StringEnum>] OverSampleType =
        | None
        | [<CompiledName("2x")>] Twice
        | [<CompiledName("4x")>] Four_Times

    and [<AllowNullLiteral>] WaveShaperNodeType =
        abstract prototype: WaveShaperNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: WaveShaperOptions -> WaveShaperNode

    and [<AllowNullLiteral>] WaveShaperOptions = 
        inherit AudioNodeOptions

        abstract curve: float seq with get, set
        abstract oversample: OverSampleType with get, set
   
    let [<Global>] AudioContext: AudioContextType = jsNative
    let [<Global>] OfflineAudioContext: OfflineAudioContextType = jsNative
    let [<Global>] AudioBuffer: AudioBufferType = jsNative
    let [<Global>] AnalyserNode: AnalyserNodeType = jsNative
    let [<Global>] AudioBufferSourceNode: AudioBufferSourceNodeType = jsNative
    let [<Global>] BiquadFilterNode: BiquadFilterNodeType = jsNative
    let [<Global>] ChannelMergerNode: ChannelMergerNodeType = jsNative
    let [<Global>] ChannelSplitterNode: ChannelSplitterNodeType = jsNative
    let [<Global>] ConstantSourceNode: ConstantSourceNodeType = jsNative
    let [<Global>] ConvolverNode: ConvolverNodeType = jsNative
    let [<Global>] DynamicsCompressorNode: DynamicsCompressorNodeType = jsNative
    let [<Global>] GainNode: GainNodeType = jsNative
    let [<Global>] IIRFilterNode: IIRFilterNodeType = jsNative
    let [<Global>] OscillatorNode: OscillatorNodeType = jsNative
    let [<Global>] PannerNode: PannerNodeType = jsNative
    let [<Global>] PeriodicWave: PeriodicWaveType = jsNative
    let [<Global>] StereoPannerNode: StereoPannerNodeType = jsNative
    let [<Global>] WaveShaperNode: WaveShaperNodeType = jsNative
