namespace Cmhd.SuccintDataStructures.Tests

open System
open Expecto

module Runner =
    [<EntryPoint>]
    let main argv =
        let returnCode = Tests.runTestsInAssembly defaultConfig argv
        
        Console.ReadKey() |> ignore

        returnCode
