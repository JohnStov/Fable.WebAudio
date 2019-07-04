#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators
open Fake.BuildServer

let version = 
  if AppVeyor.detect () then
    AppVeyor.Environment.BuildVersion
  else
    "0.0.1"

Target.create "Restore" (fun _ ->
  Paket.restore id
)

Target.create "Clean" (fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    |> Shell.cleanDirs 
)

Target.create "Build" (fun _ ->
    !! "src/**/*.*proj"
    |> Seq.iter (DotNet.build id)
)

Target.create "Pack" (fun _ ->
    !! "src/**/*.*proj"
    |> Seq.iter (fun _ -> 
        Paket.pack (fun parms -> 
          { parms with OutputPath = "package"; Version=version }))
)

Target.create "All" ignore

"Clean"
  ==> "Restore"
  ==> "Build"
  ==> "Pack"
  ==> "All"

Target.runOrDefault "All"
