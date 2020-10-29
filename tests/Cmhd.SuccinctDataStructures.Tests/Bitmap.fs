namespace Cmhd.SuccinctDataStructures.Tests

#nowarn "25"

open Expecto
open Swensen.Unquote
open Cmhd.SuccinctDataStructures

module Bitmap =   

    // 11 10110101 10010101
    let (Ok bitmap) = Bitmap.ofByteArray [| byte 0b11; byte 0b10110101; byte 0b10010101 |]
        
    [<Tests>]
    let creationCases =
        testList "Bitmap.Creation." [
            testCase "cant create an empty bitmap" <| fun () -> 
                Bitmap.ofByteArray [||] =! (Error <| BitmapError EmptyBitmap)
                
            testCase "cant create a bitmap starting at 0" <| fun () -> 
                Bitmap.ofByteArray [|0uy|] =! Bitmap.ofByteArray [|0uy; 4uy|]
                
                Bitmap.ofByteArray [|0uy; 7uy|] 
                =! (Error <| BitmapError FirstByteIs0)

            testCase "ofByteArray and ofString are analogous" <| fun () -> 
                let ofBytes = Bitmap.ofByteArray [| byte 0b11; byte 0b10110101; byte 0b10010101 |] 
                let ofString = Bitmap.ofString "111011010110010101"

                (Ok bitmap) =! ofBytes
                ofBytes =! ofString
        ]

    [<Tests>]
    let rankCases =
        testList "Bitmap.Rank." [
            testCase "up to the 1st position 111011010110010101 has 1 1s" <| fun () -> 
                bitmap |> Bitmap.rank 1  =! Ok 1
            testCase "up to the 1st position 111011010110010101 has 0 0s" <| fun () -> 
                bitmap |> Bitmap.rank0 1  =! Ok 0
            testCase "up to the 2nd position 111011010110010101 has 2 1s" <| fun () -> 
                bitmap |> Bitmap.rank  2  =! Ok 2
            testCase "up to the 4th position 111011010110010101 has 1 0s" <| fun () -> 
                bitmap |> Bitmap.rank0 4  =! Ok 1
            testCase "up to the 5th position 111011010110010101 has 4 1s" <| fun () -> 
                bitmap |> Bitmap.rank  5  =! Ok 4
            testCase "up to the 7th position 111011010110010101 has 2 0s" <| fun () -> 
                bitmap |> Bitmap.rank0 7  =! Ok 2
            testCase "up to the 10th position 111011010110010101 has 7 1s" <| fun () -> 
                bitmap |> Bitmap.rank  10 =! Ok 7
            testCase "up to the 13th position 111011010110010101 has 5 0s" <| fun () -> 
                bitmap |> Bitmap.rank0 13 =! Ok 5
            testCase "up to the 14th position 111011010110010101 has 9 1s" <| fun () -> 
                bitmap |> Bitmap.rank  14 =! Ok 9
            testCase "up to the 17th position 111011010110010101 has 7 0s" <| fun () -> 
                bitmap |> Bitmap.rank0 17 =! Ok 7
            testCase "up to the 18th position 111011010110010101 has 11 1s" <| fun () -> 
                bitmap |> Bitmap.rank  18 =! Ok 11

            testCase "a number larger than the length returns an error for both" <| fun () -> 
                bitmap 
                |> Bitmap.rank 21 
                =! (Error <| BitmapError (IndexOutOfBound (18, 21)))
              
                bitmap 
                |> Bitmap.rank0 21 
                =! (Error <| BitmapError (IndexOutOfBound (18, 21)))
          
            testCase "a negative number returns an error" <| fun () -> 
                bitmap 
                |> Bitmap.rank -1 
                =! (Error <| BitmapError (IndexOutOfBound (18, -1)))
              
                bitmap 
                |> Bitmap.rank0 -1 
                =! (Error <| BitmapError (IndexOutOfBound (18, -1)))
        ]

    [<Tests>]
    let selectCases =
        testList "Bitmap.Select." [  
            testCase "the 1st 1 in 111011010110010101 is at position 1" <| fun () -> 
                bitmap |> Bitmap.select 1 =! Ok 1
            testCase "the 2nd 1 in 111011010110010101 is at position 2" <| fun () -> 
                bitmap |> Bitmap.select 2 =! Ok 2
            testCase "the 1st 0 in 111011010110010101 is at position 4" <| fun () -> 
                bitmap |> Bitmap.select0 1 =! Ok 4
            testCase "the 4th 1 in 111011010110010101 is at position 5" <| fun () -> 
                bitmap |> Bitmap.select 4 =! Ok 5
            testCase "the 2nd 0 in 111011010110010101 is at position 7" <| fun () -> 
                bitmap |> Bitmap.select0 2 =! Ok 7
            testCase "the 7th 1 in 111011010110010101 is at position 10" <| fun () -> 
                bitmap |> Bitmap.select 7 =! Ok 10
            testCase "the 5th 0 in 111011010110010101 is at position 13" <| fun () -> 
                bitmap |> Bitmap.select0 2 =! Ok 7
            testCase "the 9th 1 in 111011010110010101 is at position 14" <| fun () -> 
                bitmap |> Bitmap.select 9 =! Ok 14
            testCase "the 7th 0 in 111011010110010101 is at position 17" <| fun () -> 
                bitmap |> Bitmap.select0 7  =! Ok 17
            testCase "the 11th 1 in 111011010110010101 is at position 18" <| fun () -> 
                bitmap |> Bitmap.select 11 =! Ok 18

            testCase "a number larger than the length returns an error" <| fun () -> 
                bitmap 
                |> Bitmap.select 21 
                =! (Error <| BitmapError (IndexOutOfBound (18, 21)))

                bitmap 
                |> Bitmap.select0 21 
                =! (Error <| BitmapError (IndexOutOfBound (18, 21)))
          
            testCase "a negative number returns an error" <| fun () -> 
                bitmap 
                |> Bitmap.select -1 
                =! (Error <| BitmapError (IndexOutOfBound (18, -1)))
              
                bitmap 
                |> Bitmap.select0 -1 
                =! (Error <| BitmapError (IndexOutOfBound (18, -1)))
        ]

    [<Tests>]
    let nthBitCases =
        testList "Bitmap.NthBit." [
            testCase  "1st bit of 111011010110010101 is 1"  <| fun() -> 
                bitmap |> Bitmap.nthBit 1  =! Ok 1
            testCase  "2nd bit of 111011010110010101 is 1"  <| fun() ->
                bitmap |> Bitmap.nthBit 2  =! Ok 1
            testCase  "3rd bit of 111011010110010101 is 1"  <| fun() -> 
                bitmap |> Bitmap.nthBit 3  =! Ok 1
            testCase  "4th bit of 111011010110010101 is 0"  <| fun() -> 
                bitmap |> Bitmap.nthBit 4  =! Ok 0
            testCase  "5th bit of 111011010110010101 is 1"  <| fun() -> 
                bitmap |> Bitmap.nthBit 5  =! Ok 1
            testCase  "6th bit of 111011010110010101 is 1"  <| fun() -> 
                bitmap |> Bitmap.nthBit 6  =! Ok 1
            testCase  "7th bit of 111011010110010101 is 0"  <| fun() -> 
                bitmap |> Bitmap.nthBit 7  =! Ok 0
            testCase  "8th bit of 111011010110010101 is 1"  <| fun() -> 
                bitmap |> Bitmap.nthBit 8  =! Ok 1
            testCase  "9th bit of 111011010110010101 is 0"  <| fun() -> 
                bitmap |> Bitmap.nthBit 9  =! Ok 0
            testCase "10th bit of 111011010110010101 is 1"  <| fun() -> 
                bitmap |> Bitmap.nthBit 10 =! Ok 1
            testCase "14th bit of 111011010110010101 is 1"  <| fun() -> 
                bitmap |> Bitmap.nthBit 14 =! Ok 1
            testCase "16th bit of 111011010110010101 is 1"  <| fun() -> 
                bitmap |> Bitmap.nthBit 16 =! Ok 1
            testCase "17th bit of 111011010110010101 is 0"  <| fun() -> 
                bitmap |> Bitmap.nthBit 17 =! Ok 0
            testCase "18th bit of 111011010110010101 is 1"  <| fun() -> 
                bitmap |> Bitmap.nthBit 18 =! Ok 1
            testCase "40th bit of 111011010110010101 returns an error" <| fun() -> 
                bitmap |> 
                Bitmap.nthBit 40 
                =! (Error <| BitmapError (IndexOutOfBound (18, 40)))
        ]

    [<Tests>]
    let isValidBitCases =
        testList "Bitmap.IsValidBit." [
            testCase "1st and 18th bit are valid bits in the bitmap" <| fun() -> 
                bitmap |> Bitmap.isValidBit 1 =! true
                bitmap |> Bitmap.isValidBit 18 =! true
            
            testCase "0, negative numbers and 19+ are invalid bits in the bitmap" <| fun() -> 
                bitmap |> Bitmap.isValidBit 0 =! false
                bitmap |> Bitmap.isValidBit -1 =! false
                bitmap |> Bitmap.isValidBit 19 =! false
        ]

    [<Tests>]
    let generalCases =
        testList "Bitmap.General." [
            testCase "the length of 111011010110010101 is 18" <| fun () -> 
                bitmap |> Bitmap.length =! Ok 18

            testCase "Bitmap should be capable of pretty printing" <| fun () ->
                sprintf "%O" bitmap 
                =! "Bitmap [| byte 0b11; byte 0b10110101; byte 0b10010101 |]"
            
                sprintf "%A" bitmap 
                =! "Bitmap [| 11; 10110101; 10010101 |]"
        ]