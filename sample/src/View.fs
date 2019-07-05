module App.View

open App.Model
open Fable.React
open Fable.React.Props
open Fulma
open Fulma.Extensions.Wikiki
open Fable.WebAudio

let view model dispatch =
    Hero.hero [ Hero.IsFullHeight ]
        [ Hero.body [ ]
            [ Container.container [ ]
                [ Columns.columns [ Columns.CustomClass "has-text-centered" ]
                    [ Column.column [ Column.Width(Screen.All, Column.IsHalf)
                                      Column.Offset(Screen.All, Column.IsOneQuarter) ]
                        [ Image.image [ Image.Is128x128
                                        Image.Props [ Style [ Margin "auto"] ] ]
                            [ img [ Src "assets/fulma_logo.svg" ] ]
                          Field.div []
                            [
                               canvas [Id "canvas"] []
                            ]
                          Field.div []
                            [
                               Checkradio.radioInline [Checkradio.Id "check-sine"; Checkradio.Name "wave"; Checkradio.OnChange(fun _ -> dispatch (Wave Sine))] [str "Sine"]
                               Checkradio.radioInline [Checkradio.Id "check-square"; Checkradio.Name "wave"; Checkradio.OnChange(fun _ -> dispatch (Wave Square))] [str "Square"]
                               Checkradio.radioInline [Checkradio.Id "check-sawtooth"; Checkradio.Name "wave"; Checkradio.OnChange(fun _ -> dispatch (Wave Sawtooth))] [str "Sawtooth"]
                               Checkradio.radioInline [Checkradio.Id "check-triangle"; Checkradio.Name "wave"; Checkradio.OnChange(fun _ -> dispatch (Wave Triangle))] [str "Triangle"]
                            ]
                          Field.div []
                            [
                               str "Frequency"
                               Slider.slider [Slider.Id "frequency-slider"; Slider.DefaultValue 440.0; Slider.Min 0.0; Slider.Max 20000.0; Slider.OnChange(fun evt -> dispatch (Frequency (float evt.Value)))]
                               str (sprintf "%.2f Hz" model.Osc.frequency.value)
                            ]
                          Field.div []
                            [
                               str "Detune"
                               Slider.slider [Slider.Id "detune-slider"; Slider.DefaultValue 0.0; Slider.Min -100.0; Slider.Max 100.0; Slider.OnChange(fun evt -> dispatch (Detune (float evt.Value)))]
                               str (sprintf "%.1f Cents" model.Osc.detune.value)
                            ]
                          Field.div []
                            [
                               str (sprintf "SampleRate: %.0f Hz" model.Context.sampleRate)
                            ]
                          Field.div []
                            [
                               str (sprintf "State: %O" model.Context.state)
                            ]
                          Field.div []
                            [
                               str (sprintf "Running Time: %.2f seconds" model.Context.currentTime)
                            ]
                          Field.div []
                            [
                               str (sprintf "Time Now: %O" model.Time)
                            ]
                          Field.div []
                            [
                              Button.button [ Button.OnClick (fun _ -> dispatch Msg.Start) ] [str "Start"]
                              Button.button [ Button.OnClick (fun _ -> dispatch Msg.Stop) ] [str "Stop"]
                            ] ] ] ] ] ]
