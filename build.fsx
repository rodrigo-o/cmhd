#r "paket:
nuget FSharp.Core 4.7.0
nuget Fake.Core
nuget Fake.IO
nuget Fake.DotNet //"

#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

Target.initEnvironment ()

Target.create "Clean" (fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    |> Shell.cleanDirs 
)

Target.create "Build" (fun _ ->
    !! "src/**/*.*proj"
    |> Seq.iter (DotNet.build id)
)

Target.create "Test" (fun _ ->
    !! "tests/*/"
    |> Seq.iter (DotNet.test id)
)

Target.create "All" ignore

"Clean"
    ==> "Build"

"Clean"
    ==> "Build"
    ==> "Test"
    ==> "All"

Target.runOrDefault "All"
