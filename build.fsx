#r "paket:
nuget Fake.Core
nuget Fake.Core.Target
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem //"

#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.IO 
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

// This is necessary due to https://github.com/fsharp/FAKE/issues/2595
let setBuildParams (defaults:DotNet.BuildOptions) =
    { defaults with
        MSBuildParams = { 
            defaults.MSBuildParams with 
                DisableInternalBinLog = true }}

Target.initEnvironment ()

Target.create "Clean" (fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    |> Shell.cleanDirs 
)

Target.create "Build" (fun _ ->
    !! "src/**/*.*proj"
    |> Seq.iter (DotNet.build setBuildParams)
)

// This is necessary due to https://github.com/fsharp/FAKE/issues/2595
let setTestParams (defaults:DotNet.TestOptions) =
    { defaults with
        MSBuildParams = { 
            defaults.MSBuildParams with 
                DisableInternalBinLog = true }}

Target.create "CleanTest" (fun _ ->
    !! "tests/**/bin"
    ++ "tests/**/obj"
    |> Shell.cleanDirs 
)

Target.create "Test" (fun _ ->
    !! "tests/*/"
    |> Seq.iter (DotNet.test setTestParams)
)

Target.create "All" ignore

"Clean"
    ==> "Build"

"CleanTest"
    ==> "Test"

Target.runOrDefault "Build"
