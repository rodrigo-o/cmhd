#nowarn "25"
namespace Cmhd.SuccinctDataStructures.Tests

open Expecto
open Swensen.Unquote

open Cmhd.SuccinctDataStructures     
open Cmhd.SuccinctDataStructures.Common


module LoudsTest =    
    // 10 | 1110 | 11110 10 110 | 10 10 10 110 110 1110 11110 
    let louds = Data.louds
    let root = 1

    // 1110 | 11110 10 110 | 10 10 10 110 110 1110 11110 
    let noRootedLouds = Data.noRootedLouds

    [<Tests>]
    let creationCases =
        testList "Louds.Creation" [
            testCase "validation cases" <| fun () -> 
                "Expected" =! "Not implemented"

            testCase "ofByteArray and ofString are analogous" <| fun () -> 
                let bitmapLouds = 
                    Data.bitmapCreation [| byte 0b10111; byte 0b01111010; byte 0b11010101;
                                          byte 0b01101101; byte 0b11011110 |]
                
                let ofBytes = Louds.ofBitmap bitmapLouds
                let ofString = Louds.ofString "1011101111010110101010110110111011110"

                (Ok louds) =! ofBytes
                ofBytes =! ofString
        ]  

    [<Tests>]
    let rootedCases = 
        testList "Louds.Rooted." [
            testCase "root should be at index 1" <| fun () -> root =! 1
            testCase "second child of the root should be at index 4" <| fun () -> 
                let secChildRoot = 
                    louds @ root
                    |>> Louds.firstChild
                    |>> Louds.nextSibling
                    |>= rid

                secChildRoot =! Ok 4
                
            testCase "the third Child of the root is the same as the last child of the root" <| fun () ->
                let thirdChildRoot = 
                    louds @ root
                    |>> Louds.firstChild
                    |>> Louds.nextSibling
                    |>> Louds.nextSibling
                    |>= rid

                let lastChildRoot = 
                    louds @ root 
                    |>> Louds.lastChild 
                    |>= rid

                thirdChildRoot =! lastChildRoot

            testCase "the root doesn't have a fourth child" <| fun () ->
                let thirdChildRoot = 
                    louds @ root
                    |>> Louds.firstChild
                    |>> Louds.nextSibling
                    |>> Louds.nextSibling
                    |>= rid
                
                match thirdChildRoot with
                | Ok tcr -> 
                    let fourthChildRoot = 
                        louds @ tcr 
                        |>> Louds.nextSibling  
                        |>= rid
                    
                    let fourthChildRoot2 = 
                        louds @ root 
                        |>> Louds.child 4
                        |>= rid

                    fourthChildRoot
                    =! (Error <| LoudsError (InexistentNextSiblingFor tcr))

                    fourthChildRoot2
                    =! (Error <| LoudsError (InexistentNthChildFor (1, 4)))

            testCase "the first child doesn't have a previous sibling" <| fun () ->
                let firstChildRoot = 
                    louds @ root
                    |>> Louds.firstChild
                    |>= rid

                match firstChildRoot with
                | Ok fcr -> 
                    let prevFirstChildRoot = 
                        louds @ fcr 
                        |>> Louds.prevSibling  
                        |>= rid
                    
                    prevFirstChildRoot
                    =! (Error <| LoudsError (InexistentPrevSiblingFor fcr))
            
            testCase "the second child of the third child of the root is at index 15" <| fun () ->
                let sndChildThirdChildRoot = 
                    louds @ root
                    |>> Louds.firstChild 
                    |>> Louds.nextSibling
                    |>> Louds.nextSibling
                    |>> Louds.firstChild
                    |>> Louds.nextSibling
                    |>= rid

                let lastChildLastChildRoot = 
                    louds @ root
                    |>> Louds.lastChild
                    |>> Louds.lastChild
                    |>= rid

                sndChildThirdChildRoot =! lastChildLastChildRoot
                lastChildLastChildRoot =! Ok 15
                

            testCase "the parent of the node at index 15 is the third child of the root" <| fun () ->
                let thirdChildRoot = 
                    louds @ root 
                    |>> Louds.lastChild
                    |>= rid

                let parentOf15 = 
                    louds @ 15 
                    |>> Louds.parent
                    |>= rid

                thirdChildRoot =! parentOf15
            
            testCase "the root doesn't have a parent" <| fun () ->
                Louds.parent 1 louds 
                =! (Error <| LoudsError (InexistentParentFor 1))

            testCase "degree of root is 1 and degree of the first child of the root is 3" <| fun () ->
                let degreeOfRoot = louds @ root |>> Louds.degree |>= rid
                let degreeFirstChildRoot = 
                    louds @ root
                    |>> Louds.firstChild
                    |>> Louds.degree
                    |>= rid

                degreeOfRoot =! Ok 1                       
                degreeFirstChildRoot =! Ok 3

            testCase "child quantity of root is 3 and of the first child of the root is 4" <| fun () ->
                let childsOfRoot = louds @ root |>> Louds.childQty |>= rid
                let childsFirstChildRoot = 
                    louds @ root
                    |>> Louds.firstChild
                    |>> Louds.childQty
                    |>= rid

                childsOfRoot =! Ok 3                       
                childsFirstChildRoot =! Ok 4

            testCase "the second child of the root ocupy the second place among it's siblings" <| fun () ->
                let sndChildRootRank = 
                    louds @ root 
                    |>> Louds.firstChild 
                    |>> Louds.nextSibling
                    |>> Louds.childRank
                    |>= rid
                
                sndChildRootRank =! Ok 2
            
            testCase "the second child of the root should be the previous sibling of the last child" <| fun () ->
                let sndChildRoot = 
                    louds @ root
                    |>> Louds.lastChild
                    |>> Louds.prevSibling
                    |>= rid
                
                sndChildRoot =! Louds.child 2 root louds
            
            testCase "the second child of the third child of the root should be the next sibling of the first child of the last child of the root" <| fun () ->
                let sndChildLastChildRoot = 
                    louds @ root
                    |>> Louds.lastChild
                    |>> Louds.firstChild
                    |>> Louds.nextSibling
                    |>= rid
                
                let sndChildThirdChildRoot = 
                    louds @ root
                    |>> Louds.child 3 
                    |>> Louds.child 2
                    |>= rid
                
                sndChildLastChildRoot =! sndChildThirdChildRoot
            
            testCase "the ancestors for node 36 are [ 1; 5; 15; 36 ]" <| fun () ->
                let ancestors = 
                    louds |> Louds.ancestors 36
                
                
                let parents = List.map (fun (Ok x) -> x) [
                    Ok root
                    louds @ 36 |>> Louds.parent |>> Louds.parent |>= rid
                    louds @ 36 |>> Louds.parent |>= rid
                    louds @ 36 |>= rid
                ]
                
                parents =! ancestors
                ancestors =! [ 1; 5; 15; 36 ]

            testCase "range per Level in Louds must be [|(1, 1); (3, 5); (7, 15); (17, 36)|]" <| fun () ->
                louds 
                |> Louds.rangeLevels 
                =! [ (1, 1); (3, 5); (7, 15); (17, 36) ]
                
            testCase "this child should not exist, it's the edge case for the louds" <| fun () -> 
                // This is bad, really really bad, but a lot of other 
                // things have worked thanks to this , and i don't know 
                // why xD
                louds @ root
                |>> Louds.firstChild
                |>> Louds.firstChild
                |>> Louds.firstChild
                |>> Louds.firstChild
                |>= rid
                =! (Error <| LoudsError (InexistentChildFor 17))
        ]

    [<Tests>]
    let unrootedCases = 
        testList "Louds.Unrooted." [
            testCase "degree of node 2 should be 3" <| fun () ->
                noRootedLouds @ 2
                |>> Louds.degree 
                |>= rid =! Ok 3
                
            testCase "child rank of node 2 should be 2" <| fun () ->
                noRootedLouds @ 2
                |>> Louds.childRank 
                |>= rid =! Ok 2
                
            testCase "child quantity of node 2 should be 1" <| fun () ->
                noRootedLouds @ 2
                |>> Louds.childQty 
                |>= rid =! Ok 1
                
            testCase "the second child of the first child of node 2 should be 25" <| fun () ->
                let sndOfFirstChildOf2 = noRootedLouds @ 2
                                            |>> Louds.firstChild
                                            |>> Louds.firstChild
                                            |>> Louds.nextSibling
                                            |>= rid
                    
                let sndOfFirstChildOf2SndForm = noRootedLouds @ 2
                                                |>> Louds.child 1
                                                |>> Louds.child 2
                                                |>= rid
                    
                let sndOfFirstChildOf2ThirdForm = noRootedLouds @ 2
                                                    |>> Louds.firstChild
                                                    |>> Louds.lastChild
                                                    |>= rid

                sndOfFirstChildOf2 =! sndOfFirstChildOf2SndForm
                sndOfFirstChildOf2 =! sndOfFirstChildOf2ThirdForm
                sndOfFirstChildOf2 =! Ok 25

            testCase "the parent of the parent of node 25 should be 2" <| fun () ->
                noRootedLouds @ 25
                |>> Louds.parent
                |>> Louds.parent
                |>= rid =! Ok 2
                
            testCase "the ancestors for node 25 are [ 2; 10; 25 ]" <| fun () ->
                let ancestors = 
                    noRootedLouds |> Louds.ancestors 25
                
                let parents = List.map (fun (Ok x) -> x) [
                    noRootedLouds @ 25 |>> Louds.parent |>> Louds.parent |>= rid
                    noRootedLouds @ 25 |>> Louds.parent |>= rid
                    noRootedLouds @ 25 |>= rid
                ]
                
                parents =! ancestors
                ancestors =! [ 2; 10; 25 ]

            testCase "node 25 don't have a child" <| fun () ->
                noRootedLouds @ 25
                |>> Louds.firstChild
                |>> Louds.firstChild
                |>= rid =! Error (LoudsError (InexistentChildFor 25))
                
            testCase "range per Level in Louds must be [|(1, 3); (5, 13); (15, 34)|]" <| fun () ->
                noRootedLouds 
                |> Louds.rangeLevels =! [ (1, 3); (5, 13); (15, 34) ]

        ]

    [<Tests>]
    let generalCases =
        testList "Louds.General" [
            testCase "Louds should be capable of pretty printing" <| fun () ->
                sprintf "%O" louds 
                =! "Louds [| byte 0b10111; byte 0b01111010; byte 0b11010101; byte 0b01101101; byte 0b11011110 |]"

                sprintf "%A" louds 
                =! "Louds [| 10 | 1110 | 11110 10 110 | 10 10 10 110 110 1110 11110 |]"

        ]
