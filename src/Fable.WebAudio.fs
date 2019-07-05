namespace Fable

open Browser.Types
open Fable.Core
open Fable.Core.JS
open System

module WebAudio =
    type [<AllowNullLiteral>] BaseAudioContext = 
        inherit EventTarget
        
        /// The final destination for all audio
        abstract destination: AudioDestinationNode with get
        /// The sample rate for the AudioContext
        abstract sampleRate: float with get
        /// The time in seconbds of the next sample frame
        abstract currentTime: float with get
        /// The AudioListener used for 3D spatialization
        abstract listener: AudioListener with get
        /// The current state of the AudioContext: suspended, running or closed
        abstract state: AudioContextState with get
        /// An event handler fired when the state changes
        abstract onstatechange : (Event -> 'Out) with get, set

        /// Create an AnalyserNode with default settings
        abstract createAnalyser: unit -> AnalyserNode
        /// Create a BiquadFilterNode with default settings
        abstract createBiquadFilter: unit -> BiquadFilterNode
        /// Create an AudioBuffer with specified number of channels, length (in frames) and sample rate
        abstract createBuffer: numberOfChannels: int * length: int * sampleRate: float -> AudioBuffer
        /// Create an AudioBufferSourceNode with default settings
        abstract createBufferSource: unit -> AudioBufferSourceNode
        /// Create a ChannelMergerNode with the specified number of inputs (default 6)
        abstract createChannelMerger: ?numberOfInputs: int -> ChannelMergerNode
        /// Create a ChannelSplitterNode with the specified number of outputs (default 6)
        abstract createChannelSplitter: ?numberOfOutputs: int -> ChannelSplitterNode
        /// Create a ConstantSourceNode with default settings
        abstract createConstantSource: unit -> ConstantSourceNode
        /// Create a ConvolverNode with default settings
        abstract createConvolver: unit -> ConvolverNode
        /// Create a DelayNode with the specified maximum delay in seconds (default 1, maximum 300)
        abstract createDelay: ?maxDelayTime: float -> DelayNode
        /// Create a DynamicsCompressorNode with default settings
        abstract createDynamicsCompressor: unit -> DynamicsCompressorNode
        /// Create a GainNode with default settings
        abstract createGain: unit -> GainNode
        /// Create an IIRFilterNode with the specified coefficients (max 20)
        abstract createIIRFilter: feedforward: float32 seq * feedback: float32 seq -> IIRFilterNode
        /// Create an OscillatorNode with default settings
        abstract createOscillator: unit -> OscillatorNode
        /// Create a PannerNode with default settings
        abstract createPanner: unit -> PannerNode
        /// Create a PeriodicWave with specified real and imaginary parts and normalization constraints
        abstract createPeriodicWave: real: float32 seq * imag: float32 seq * ?constrants: PeriodicWaveConstraints -> PeriodicWave
        [<Obsolete>]
        /// Create a ScriptProcessorNode with the specified parameters.
        /// This method is DEPRECATED, as it is intended to be replaced by AudioWorkletNode.
        abstract createScriptProcessor: ?bufferSize: int * ?numberOfInputChannels: int * ?numberOfOutputChannels: int -> ScriptProcessorNode
        /// Create a StereoPannerNode with default parameters.
        abstract createStereoPanner: unit -> StereoPannerNode
        /// Create a WaveShaperNode with default parameters.
        abstract createWaveShaper: unit -> WaveShaperNode
        /// Asynchronously decodes the audio file data contained in the ArrayBuffer
        abstract decodeAudioData: audioData: ArrayBuffer * ?successCallback: (AudioBuffer -> unit) option * ?errorCallback: (DOMException -> unit) option -> Promise<AudioBuffer>
        /// Resumes the progression of currentTime when it has been suspended.
        abstract resume: unit -> Promise<unit>

    and [<StringEnum>] AudioContextState = 
        | Suspended
        | Running 
        | Closed 

    and [<AllowNullLiteral>] AudioContext = 
        inherit BaseAudioContext

        /// The number of seconds of processing latency incurred by the AudioContext passing the audio from the AudioDestinationNode to the audio subsystem
        abstract baseLatency: float with get
        /// The estimation in seconds of audio output latency
        abstract outputLatency: float with get

        /// Close the context and release resources
        abstract close: unit -> Promise<unit>
        /// Returns a new AudioTimestamp instance containing two correlated context’s audio stream position values
        abstract getOutputTimestamp : unit -> AudioTimestamp
        /// Suspends the progression of AudioContext's currentTime
        abstract suspend: unit -> Promise<unit>

    and [<AllowNullLiteral>] AudioContextOptions = 
        /// Identify the type of playback, which affects tradeoffs between audio output latency and power consumption
        abstract latencyHint: AudioContextHint with get, set
        /// Set the sampleRate to this value for the AudioContext that will be created
        abstract sampleRate: float with get, set

    and [<AllowNullLiteral>] AudioTimestamp = 
        /// Represents a point in the time coordinate system of BaseAudioContext’s currentTime
        abstract contextTime: double with get, set
        /// Represents a point in the time coordinate system of a Performance interface implementation
        abstract performanceTime: double with get, set

    and [<StringEnum>] AudioContextLatencyCategory = 
        | Balanced 
        | Interactive 
        | Playback 

    and AudioContextHint = 
        | LatencyCategory of AudioContextLatencyCategory
        | Latency of float

    and [<AllowNullLiteral>] AudioContextType = 
        abstract prototype: AudioContext with get, set
        [<Emit("new $0($1...)")>] abstract Create: ?contextOptions: AudioContextOptions -> AudioContext

    and [<AllowNullLiteral>] OfflineAudioContext =
        inherit BaseAudioContext

        /// Start rendering audio
        abstract startRendering: unit -> Promise<AudioBuffer>
        /// Schedule a suspension of the time progression in the audio context
        abstract suspend: suspendTime: float -> Promise<unit>
        /// The size of the buffer in sample-frames
        abstract length: int with get
        /// An EventHandler of type OfflineAudioCompletionEvent. It is the last event fired on an OfflineAudioContext
        abstract oncomplete: (OfflineAudioCompletionEvent -> 'Out) with get, set
    
    and [<AllowNullLiteral>] OfflineAudioContextType =
        abstract prototype: OfflineAudioContext with get, set
        [<Emit("new $0($1...)")>] abstract Create: ?contextOptions: OfflineAudioContextOptions -> OfflineAudioContext

    and [<AllowNullLiteral>] OfflineAudioContextOptions =
        /// The number of channels for this OfflineAudioContext (default 6)
        abstract numberOfChannels: int with get, set
        /// The length of the rendered AudioBuffer in sample-frames
        abstract length: int with get, set
        /// The sample rate for this OfflineAudioContext
        abstract sampleRate: float with get, set

    and [<AllowNullLiteral>] OfflineAudioCompletionEvent =
        inherit Event
        
        /// An AudioBuffer containing the rendered audio data        
        abstract renderedBuffer: AudioBuffer with get

    and [<AllowNullLiteral>] OfflineAudioCompletionEventType =
        abstract prototype: OfflineAudioCompletionEvent with get, set
        [<Emit("new $0($1...)")>] abstract Create: ?eventInitDict: OfflineAudioCompletionEventInit -> OfflineAudioCompletionEvent

    and [<AllowNullLiteral>] OfflineAudioCompletionEventInit =
        inherit EventInit

        /// Value to be assigned to the renderedBuffer attribute of the event
        abstract renderedBuffer: AudioBuffer with get, set

    and [<AllowNullLiteral>] AudioBuffer =
        /// The sample-rate for the PCM audio data in samples per second
        abstract samplerate: float with get
        /// Length of the PCM audio data in sample-frames
        abstract length: int with get
        /// Duration of the PCM audio data in second
        abstract duration: float with get
        /// The number of discrete audio channels
        abstract numberOfChannels: int with get
        /// Either get a reference to or get a copy of the bytes stored in [[internal data]] in a new Float32Array
        abstract getChannelData: channel: int -> float32 array
        /// Copy the samples from the specified channel of the AudioBuffer to the destination array
        abstract copyFromChannel: destination: float32 array * channelNumber: int * ?startChannel: int -> unit
        /// Copy the samples to the specified channel of the AudioBuffer from the source array
        abstract copyToChannel: source: float32 array * channelNumber: int * ?startChannel: int -> unit

    and [<AllowNullLiteral>] AudioBufferType = 
        abstract prototype: AudioBuffer with get, set
        [<Emit("new $0($1...)")>] abstract Create: options: AudioBufferOptions -> AudioBuffer

    and [<AllowNullLiteral>] AudioBufferOptions = 
        /// The number of channels for the buffer
        abstract numberOfChannels: int with get, set
        /// The length in sample frames of the buffer
        abstract length: int with get, set
        /// The sample rate in Hz for the buffer
        abstract sampleRate: float with get, set

    and [<AllowNullLiteral>] AudioNode = 
        inherit EventTarget

        /// The BaseAudioContext which owns this AudioNode
        abstract context: BaseAudioContext with get
        /// The number of inputs feeding into the AudioNode
        abstract numberOfInputs: int with get
        /// The number of outputs coming out of the AudioNode
        abstract numberOfOutputs: int with get
        /// The number of channels used when up-mixing and down-mixing connections to any inputs to the node
        abstract channelCount: int with get, set
        /// Determines how channels will be counted when up-mixing and down-mixing connections to any inputs to the node
        abstract channelCountMode: ChannelCountMode with get, set
        /// Determines how individual channels will be treated when up-mixing and down-mixing connections to any inputs to the node
        abstract channelInterpretation: ChannelInterpretation with get, set

        /// Connects the AudioNode output to athe input of another AudioNode
        abstract connect: destinationNode : AudioNode * ?output: int * ?input: int -> AudioNode
        /// Connects the AudioNode to an AudioParam, controlling the parameter value with an audio-rate signal
        abstract connect: destinationParam: AudioParam * ?output: int * ?input: int -> unit
        /// Disconnects all outgoing connections from the AudioNode
        abstract disconnect: unit -> unit
        /// Disconnects all outputs of the AudioNode that go to a specific destination AudioNode
        abstract disconnect: destinationNode: AudioNode -> unit
        /// Disconnects a specific output of the AudioNode from a specific input of some destination AudioNode
        abstract disconnect: destinationNode: AudioNode * output: int -> unit
        /// Disconnects a specific output of the AudioNode from a specific input of some destination AudioNode
        abstract disconnect: destinationNode: AudioNode * output: int * input: int -> unit
        /// Disconnects all outputs of the AudioNode that go to a specific destination AudioParam
        abstract disconnect: destinationParam: AudioParam -> unit
        /// Disconnects a specific output of the AudioNode that go to a specific destination AudioParam
        abstract disconnect: destinationParam: AudioParam * output: int -> unit

    and [<AllowNullLiteral>] AudioNodeOptions =
        /// Desired number of channels for the channelCount attribute
        abstract channelCount: int with get, set
        /// Desired mode for the channelCountMode attribute
        abstract channelCountMode: ChannelCountMode with get, set
        /// Desired mode for the channelInterpretation attribute
        abstract channelInterpretation: ChannelInterpretation with get, set

    and [<StringEnum>] ChannelCountMode = 
        | Max
        | [<CompiledName("clamped-max")>] ClampedMax
        | Explicit

    and [<StringEnum>] ChannelInterpretation = 
        | Speakers
        | Discrete

    and [<AllowNullLiteral>] AudioParam = 
        /// The parameter’s floating-point value
        abstract value: float with get, set
        /// The automation rate for the AudioParam
        abstract automationRate: AutomationRate with get, set    
        /// Initial value for the value attribute
        abstract defaultValue: float with get
        /// The nominal minimum value that the parameter can take
        abstract minValue: float with get
        /// The nominal maximum value that the parameter can take
        abstract maxValue: float with get

        /// Schedules a parameter value change at the given time
        abstract setValueAtTime: value: float * startTime: float -> AudioParam
        /// Schedules a linear continuous change in parameter value from the previous scheduled parameter value to the given value
        abstract linearRampToValueAtTime:  value: float * startTime: float -> AudioParam
        /// Schedules an exponential continuous change in parameter value from the previous scheduled parameter value to the given value
        abstract exponentialRampToValueAtTime:  value: float * startTime: float -> AudioParam
        /// Start exponentially approaching the target value at the given time
        abstract setTargetAtTime:  target: float * startTime: float * timeConstant: float -> AudioParam
        /// Sets an array of arbitrary parameter values starting at the given time for the given duration
        abstract setValueCurveAtTime:  values: float seq * startTime: float * duration: float -> AudioParam
        /// Cancel all scheduled parameter changes with times greater than or equal to cancelTime
        abstract cancelScheduledValues:  cancelTime: float -> AudioParam
        /// Cancel all scheduled parameter changes with times greater than or equal to cancelTime
        abstract cancelAndHoldAtTime:  cancelTime: float -> AudioParam

    and [<StringEnum>] AutomationRate = 
        | [<CompiledName("a-rate")>] A_Rate
        | [<CompiledName("k-rate")>] K_rate

    and [<AllowNullLiteral>] AudioScheduledSourceNode = 
        inherit AudioNode

        /// The EventHandler for the ended event that is dispatched to AudioScheduledSourceNode node types
        abstract onended: (Event -> 'Out) with get, set
        /// Schedule a sound to playback at an exact time
        abstract start: ?``when``: float -> unit
        /// Schedule a sound to stop playback at an exact time
        abstract stop: ?``when``: float -> unit
        
    and [<AllowNullLiteral>] AnalyserNode =
        inherit AudioNode

        /// The size of the FFT used for frequency-domain analysis (in sample-frames)
        abstract fftSize: int with get, set
        /// Half the FFT size
        abstract frequencyBinCount: int with get
        /// The minimum power value in the scaling range for the FFT analysis data
        abstract minDecibels: float with get, set
        /// The maximum power value in the scaling range for the FFT analysis data
        abstract maxDecibels: float with get, set
        /// A value from 0 -> 1 where 0 represents no time averaging with the last analysis frame. (default 0.8)
        abstract smoothingTimeConstant: float with get, set

        /// Copies the current frequency data into the passed floating-point array
        abstract getFloatFrequencyData: array: float32 array -> unit
        /// Copies the current frequency data into the passed unsigned byte array
        abstract getByteFrequencyData: array: uint8 array -> unit
        /// Copies the current time-domain data (waveform data) into the passed floating-point array
        abstract getFloatTimeDomainData: array: float32 array -> unit
        /// Copies the current time-domain data (waveform data) into the passed unsigned byte array
        abstract getByteTimeDomainData: array: uint8 array -> unit

    and [<AllowNullLiteral>] AnalyserNodeType = 
        abstract prototype: AnalyserNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: AnalyserOptions -> AnalyserNode

    and [<AllowNullLiteral>] AnalyserOptions =
        inherit AudioNodeOptions

        /// The desired initial size of the FFT for frequency-domain analysis
        abstract fftSize: int with get, set
        /// The desired initial maximum power in dB for FFT analysis
        abstract maxDecibels: float with get, set
        /// The desired initial minimum power in dB for FFT analysis
        abstract minDecibels: float with get, set
        /// The desired initial smoothing constant for the FFT analysis
        abstract smoothingTimeConstant: float with get, set

    and [<AllowNullLiteral>] AudioBufferSourceNode =
        inherit AudioScheduledSourceNode

        /// Represents the audio asset to be played
        abstract buffer: AudioBuffer with get, set
        /// The speed at which to render the audio stream
        abstract playbackRate: AudioParam with get
        /// An additional parameter, in cents, to modulate the speed at which is rendered the audio stream.
        abstract detune: AudioParam with get
        /// Indicates if the region of audio data designated by loopStart and loopEnd should be played continuously in a loop
        abstract loop: bool with get, set
        /// An optional playhead position where looping should begin if the loop attribute is true
        abstract loopStart: float with get, set
        /// An optional playhead position where looping should end if the loop attribute is true
        abstract loopEnd: float with get, set
        /// Schedules a sound to playback at an exact time
        abstract start: ?``when``: float * ?offset: float * ?duration: float -> unit
    
    and [<AllowNullLiteral>] AudioBufferSourceNodeType = 
        abstract prototype: AudioBufferSourceNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: AudioBufferSourceOptions -> AudioBufferSourceNode

    and [<AllowNullLiteral>] AudioBufferSourceOptions =
        /// The audio asset to be played
        abstract buffer: AudioBuffer with get, set
        /// The initial value for the detune AudioParam
        abstract detune: float with get, set
        /// The initial value for the loop attribute
        abstract loop: bool with get, set
        /// The initial value for the loopEnd attribute
        abstract loopEnd: float with get, set
        /// The initial value for the loopStart attribute
        abstract loopStart: float with get, set
        /// The initial value for the playbackRate AudioParam
        abstract playbackRate: float with get, set

    and [<AllowNullLiteral>] AudioDestinationNode = 
        inherit AudioNode
        
        /// The maximum number of channels that the channelCount attribute can be set to
        abstract maxChannelCount: int with get

    and [<AllowNullLiteral>] AudioListener =
        /// Sets the x coordinate position of the audio listener in a 3D Cartesian coordinate space
        abstract positionX: AudioParam with get
        /// Sets the Y coordinate position of the audio listener in a 3D Cartesian coordinate space
        abstract positionY: AudioParam with get
        /// Sets the Z coordinate position of the audio listener in a 3D Cartesian coordinate space
        abstract positionZ: AudioParam with get
        /// Sets the x coordinate component of the forward direction the listener is pointing in 3D Cartesian coordinate space
        abstract forwardX: AudioParam with get
        /// Sets the Y coordinate component of the forward direction the listener is pointing in 3D Cartesian coordinate space
        abstract forwardY: AudioParam with get
        /// Sets the Z coordinate component of the forward direction the listener is pointing in 3D Cartesian coordinate space
        abstract forwardZ: AudioParam with get
        /// Sets the x coordinate component of the up direction the listener is pointing in 3D Cartesian coordinate space
        abstract upX: AudioParam with get
        /// Sets the Y coordinate component of the up direction the listener is pointing in 3D Cartesian coordinate space
        abstract upY: AudioParam with get
        /// Sets the Z coordinate component of the up direction the listener is pointing in 3D Cartesian coordinate space
        abstract upZ: AudioParam with get
        
        [<Obsolete>]
        /// Sets the position of the listener in a 3D cartesian coordinate space
        abstract setPosition: x: float * y: float * z: float -> unit
        [<Obsolete>]
        /// Describes which direction the listener is pointing in the 3D cartesian coordinate space
        abstract setOrientation: x: float * y: float * z: float * xUp: float * yUp: float * zUp: float -> unit
    
    and [<AllowNullLiteral>] BiquadFilterNode =
        inherit AudioNode

        /// The type of this BiquadFilterNode. Its default value is "lowpass"
        abstract ``type``: BiquadFilterType with get, set
        /// The frequency at which the BiquadFilterNode will operate, in Hz
        abstract frequency: AudioParam with get
        /// A detune value, in cents, for the frequency
        abstract detune: AudioParam with get
        /// The Q factor of the filter
        abstract Q: AudioParam with get
        /// The gain of the filter. Its value is in dB units
        abstract gain: AudioParam with get
        /// Given the current filter parameter settings, synchronously calculates the frequency response for the specified frequencies
        abstract getFrequencyResponse: frequencyHz: float32 array * magResponse: float32 array * phaseResponse: float32 array -> unit

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

        /// The desired initial type of the filter
        abstract ``type``: BiquadFilterType with get, set
        /// The desired initial value for Q
        abstract Q: float with get, set
        /// The desired initial value for detune
        abstract detune: float with get, set
        /// The desired initial value for frequency
        abstract frequency: float with get, set
        /// The desired initial value for gain
        abstract gain: float with get, set

    and [<AllowNullLiteral>] ChannelMergerNode =
        inherit AudioNode

    and [<AllowNullLiteral>] ChannelMergerNodeType =
        abstract prototype: ChannelMergerNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: ChannelMergerOptions -> ChannelMergerNode

    and [<AllowNullLiteral>] ChannelMergerOptions =
        inherit AudioNodeOptions

        /// The number inputs for the ChannelMergerNode
        abstract numberOfOutputs: int with get, set
    
    and [<AllowNullLiteral>] ChannelSplitterNode =
        inherit AudioNode

    and [<AllowNullLiteral>] ChannelSplitterNodeType =
        abstract prototype: ChannelSplitterNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: ChannelSplitterOptions -> ChannelSplitterNode

    and [<AllowNullLiteral>] ChannelSplitterOptions =
        inherit AudioNodeOptions

        /// The number outputs for the ChannelSplitterNode
        abstract numberOfOutputs: int with get, set

    and [<AllowNullLiteral>] ConstantSourceNode =
        inherit AudioScheduledSourceNode

        /// The constant value of the source
        abstract offset: AudioParam with get

    and [<AllowNullLiteral>] ConstantSourceNodeType =
        abstract prototype: ConstantSourceNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: ConstantSourceOptions -> ConstantSourceNode

    and [<AllowNullLiteral>] ConstantSourceOptions =
        
        /// The initial value for the offset AudioParam of this node
        abstract offset: float with get, set
    
    and [<AllowNullLiteral>] ConvolverNode =
        inherit AudioNode

        /// A mono, stereo, or 4-channel AudioBuffer containing the (possibly multi-channel) impulse response used by the ConvolverNode
        abstract buffer: AudioBuffer with get, set
        /// Controls whether the impulse response from the buffer will be scaled by an equal-power normalization when the buffer atttribute is set
        abstract normalize: bool with get, set

    and [<AllowNullLiteral>] ConvolverNodeType =
        abstract prototype: ConvolverNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: ConvolverOptions -> ConvolverNode

    and [<AllowNullLiteral>] ConvolverOptions =
        inherit AudioNodeOptions

        /// The desired buffer for the ConvolverNode
        abstract buffer: AudioBuffer with get, set
        /// The opposite of the desired initial value for the normalize attribute of the ConvolverNode
        abstract disableNormalization: bool with get, set
    
    and [<AllowNullLiteral>] DelayNode =
        inherit AudioNode

        /// An AudioParam object representing the amount of delay (in seconds) to apply
        abstract delayTime: AudioParam with get

    and [<AllowNullLiteral>] DelayNodeType =
        abstract prototype: DelayNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: DelayOptions -> DelayNode

    and [<AllowNullLiteral>] DelayOptions =
        inherit AudioNodeOptions

        /// The maximum delay time for the node
        abstract maxDelayTime: float with get, set
        /// The initial delay time for the node
        abstract delayTime: float with get, set
    
    and [<AllowNullLiteral>] DynamicsCompressorNode =
        inherit AudioNode

        /// The decibel value above which the compression will start taking effect
        abstract threshold: AudioParam with get
        /// A decibel value representing the range above the threshold where the curve smoothly transitions to the "ratio" portion
        abstract knee: AudioParam with get
        /// The amount of dB change in input for a 1 dB change in output
        abstract ratio: AudioParam with get
        /// A read-only decibel value for metering purposes, representing the current amount of gain reduction that the compressor is applying to the signal
        abstract reduction: float with get
        /// The amount of time (in seconds) to reduce the gain by 10dB
        abstract attack: AudioParam with get
        /// The amount of time (in seconds) to increase the gain by 10dB
        abstract release: AudioParam with get

    and [<AllowNullLiteral>] DynamicsCompressorNodeType =
        abstract prototype: DynamicsCompressorNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: DynamicsCompressorOptions -> DynamicsCompressorNode

    and [<AllowNullLiteral>] DynamicsCompressorOptions =
        inherit AudioNodeOptions

        /// The initial value for the attack AudioParam
        abstract attack: float with get, set
        /// The initial value for the knee AudioParam
        abstract knee: float with get, set
        /// The initial value for the ratio AudioParam
        abstract ratio: float with get, set
        /// The initial value for the release AudioParam
        abstract release: float with get, set
        /// The initial value for the threshold AudioParam
        abstract threshold: float with get, set
    
    and [<AllowNullLiteral>] GainNode =
        inherit AudioNode

        /// Represents the amount of gain to apply
        abstract gain: AudioParam with get

    and [<AllowNullLiteral>] GainNodeType =
        abstract prototype: GainNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: GainOptions -> GainNode

    and [<AllowNullLiteral>] GainOptions =
        inherit AudioNodeOptions

        /// The initial gain value for the gain AudioParam
        abstract gain: float with get, set
    
    and [<AllowNullLiteral>] IIRFilterNode =
        inherit AudioNode

        /// Given the current filter parameter settings, synchronously calculates the frequency response for the specified frequencies
        abstract getFrequencyResponse: frequencyHz: float32 array * magResponse: float32 array * phaseResponse: float32 array -> unit

    and [<AllowNullLiteral>] IIRFilterNodeType =
        abstract prototype: IIRFilterNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: IIRFilterOptions -> IIRFilterNode

    and [<AllowNullLiteral>] IIRFilterOptions =
        inherit AudioNodeOptions

        /// The feedforward coefficients for the IIRFilterNode
        abstract feedforward: float seq with get, set
        /// The feedback coefficients for the IIRFilterNode
        abstract feedback: float seq with get, set
    
    and [<AllowNullLiteral>] OscillatorNode = 
        inherit AudioScheduledSourceNode

        /// The shape of the periodic waveform
        abstract ``type``: OscillatorType with get, set
        /// The frequency (in Hertz) of the periodic waveform
        abstract frequency: AudioParam with get
        /// A detuning value (in cents) which will offset the frequency by the given amount
        abstract detune: AudioParam with get
        /// Sets an arbitrary custom periodic waveform given a PeriodicWave
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
        
        /// The type of oscillator to be constructed
        abstract ``type``: OscillatorType with get, set
        /// The initial frequency for the OscillatorNode
        abstract frequency: float with get, set
        /// The initial detune value for the OscillatorNode
        abstract detune: float with get, set
        /// The PeriodicWave for the OscillatorNode
        abstract periodicWave: PeriodicWave with get, set

    and [<AllowNullLiteral>] PannerNode = 
        inherit AudioNode

        /// Specifies the panning model used by this PannerNode
        abstract panningModel: PanningModelType with get, set
        /// Sets the x coordinate position of the audio source in a 3D Cartesian system
        abstract positionX: AudioParam with get
        /// Sets the Y coordinate position of the audio source in a 3D Cartesian system
        abstract positionY: AudioParam with get
        /// Sets the Z coordinate position of the audio source in a 3D Cartesian system
        abstract positionZ: AudioParam with get
        /// Describes the x component of the vector of the direction the audio source is pointing in 3D Cartesian coordinate space
        abstract orientationX: AudioParam with get
        /// Describes the Y component of the vector of the direction the audio source is pointing in 3D Cartesian coordinate space
        abstract orientationY: AudioParam with get
        /// Describes the Z component of the vector of the direction the audio source is pointing in 3D Cartesian coordinate space
        abstract orientationZ: AudioParam with get
        /// Specifies the distance model used by this PannerNode
        abstract distanceMode: DistanceModelType with get, set
        /// A reference distance for reducing volume as source moves further from the listener
        abstract refDistance: float with get, set
        /// The maximum distance between source and listener, after which the volume will not be reduced any further
        abstract maxDistance: float with get, set
        /// Describes how quickly the volume is reduced as source moves away from listener
        abstract rolloffFactor: float with get, set
        /// A parameter for directional audio sources, this is an angle, in degrees, inside of which there will be no volume reduction
        abstract coneInnerAngle: float with get, set
        /// A parameter for directional audio sources, this is an angle, in degrees, outside of which the volume will be reduced to a constant value of coneOuterGain
        abstract coneOuterAngle: float with get, set
        /// A parameter for directional audio sources, this is the gain outside of the coneOuterAngle
        abstract coneOuterGain: float with get, set
        [<Obsolete>]
        /// Sets the position of the audio source relative to the listener attribute
        abstract setPosition: x: float * y: float * z: float -> unit
        [<Obsolete>]
        /// Describes which direction the audio source is pointing in the 3D cartesian coordinate space
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
        
        /// The initial value for the panningModel attribute of the node
        abstract panningModel: PanningModelType with get, set
        /// The initial value for the distanceModel attribute of the node
        abstract distanceModel: DistanceModelType with get, set
        /// The initial value for the positionX attribute of the node
        abstract positionX: float with get, set
        /// The initial value for the positionY attribute of the node
        abstract positionY: float with get, set
        /// The initial value for the positionZ attribute of the node
        abstract positionZ: float with get, set
        /// The initial value for the orientationX attribute of the node
        abstract orientationX: float with get, set
        /// The initial value for the orientationY attribute of the node
        abstract orientationY: float with get, set
        /// The initial value for the orientationZ attribute of the node
        abstract orientationZ: float with get, set
        /// The initial value for the refDistance attribute of the node
        abstract refDistance: float with get, set
        /// The initial value for the maxDistance attribute of the node
        abstract maxDistance: float with get, set
        /// The initial value for the rolloffFactor attribute of the node
        abstract rolloffFactor: float with get, set
        /// The initial value for the coneInnerAngle attribute of the node
        abstract coneInnerAngle: float with get, set
        /// The initial value for the coneOuterAngle attribute of the node
        abstract coneOuterAngle: float with get, set
        /// The initial value for the coneOuterGain attribute of the node
        abstract coneOuterGain: float with get, set

    and [<AllowNullLiteral>] PeriodicWave = 
        interface end

    and [<AllowNullLiteral>] PeriodicWaveType =
        abstract prototype: PeriodicWave with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: PeriodicWaveOptions -> PeriodicWave

    and [<AllowNullLiteral>] PeriodicWaveConstraints =
        /// Controls whether the periodic wave is normalized or not
        abstract disableNormalization: bool with get, set

    and [<AllowNullLiteral>] PeriodicWaveOptions =
        inherit PeriodicWaveConstraints

        /// The imag parameter represents an array of sine terms
        abstract real: float32 seq with get, set
        /// The real parameter represents an array of cosine terms
        abstract imag: float32 seq with get, set
    
    
    and [<Obsolete>] [<AllowNullLiteral>] ScriptProcessorNode =
        inherit AudioNode

        /// A property used to set the EventHandler for the onaudioprocess event that is dispatched to ScriptProcessorNode node types
        abstract onaudioprocess: (Event -> 'Out) with get, set
        /// The size of the buffer (in sample-frames) which needs to be processed each time onaudioprocess is called
        abstract bufferSize: int with get

    and [<AllowNullLiteral>] StereoPannerNode = 
        inherit AudioNode

        abstract pan: AudioParam with get

    and [<AllowNullLiteral>] StereoPannerNodeType =
        abstract prototype: StereoPannerNode with get, set
        [<Emit("new $0($1...)")>] abstract Create: context: BaseAudioContext * ?options: StereoPannerOptions -> StereoPannerNode

    and [<AllowNullLiteral>] StereoPannerOptions = 
        inherit AudioNodeOptions

        /// The position of the input in the output’s stereo image. -1 represents full left, +1 represents full right        
        abstract pan: float with get, set
   
    and [<AllowNullLiteral>] WaveShaperNode = 
        inherit AudioNode

        /// The shaping curve used for the waveshaping effect
        abstract curve: float32 array with get, set
        /// Specifies what type of oversampling (if any) should be used when applying the shaping curve
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

        /// The shaping curve for the waveshaping effect
        abstract curve: float32 seq with get, set
        /// The type of oversampling to use for the shaping curve
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
