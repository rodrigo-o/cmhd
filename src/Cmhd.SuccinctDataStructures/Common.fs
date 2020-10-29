namespace Cmhd.SuccinctDataStructures

open System

/// Common compose operators and errors for all Succinct Data Structures
module Common =
    /// Return the identity inside a Result
    let rid x = Ok x

    /// Result bind operator
    let ( >>= ) m f = 
        match m with
        | Error err -> Error err
        | Ok x -> f x

    /// It maps an error to a new one if the result isn't Ok
    let inline errorMap result mappedError =
        match result with
        | Error _ -> Error mappedError
        | okRes -> okRes
    
    /// Function used to convert ByteArrays to Strings represented as 
    /// [|byte 0bxx|] i.e. [| 5uy; 7uy |] => "[| byte 0b101; byte 0b00000111 |]"
    let byteArrayToString (byteArray: byte[]) =
        byteArray
        |> Array.tail
        |> Array.fold (fun acc act -> 
            sprintf "%s; byte 0b%s" acc 
                    (Convert.ToString(int act, 2).PadLeft(8, '0'))) ""
        |> fun tail -> 
            sprintf "%s%s%s" "byte 0b" 
                    (Convert.ToString(int (Array.head byteArray), 2)) tail
        |> fun byteString -> sprintf "[| %s |]" byteString

    /// Given a string composed of 1 and 0 and max length of 8 it return a byte
    let private bitStringToByte string =
        Convert.ToByte(Convert.ToInt32(string, 2))

    /// Function used to convert Strings ByteArrays represented as 
    /// [10]+ i.e. "10100000111" => [| 5uy; 7uy |]
    let stringToByteArray string =
        let rec by8 string = seq {
            match string with
            | "" -> ()
            | _ -> yield string.[..7]
                   yield! by8 string.[8..]
        }

        let firstLen = string |> String.length |> (%) <| 8
       
        string.[firstLen..] 
        |> by8
        |> Seq.map bitStringToByte
        |> Seq.append [| bitStringToByte string.[..firstLen - 1]|]
        |> Seq.toArray

    //
    // EXISTING MODULES MODIFICATIONS
    //
    // Added String functions to make Cleaning and replacing more convenient
    module String =
        /// <summary>
        /// Clean the input string from a given one
        /// </summary>
        /// <param name="cleanThis"> string to be eliminated </param>
        /// <param name="input"> input string </param>
        /// <returns> The input string without any occurrence of cleanThis </returns>
        let clean (cleanThis: string) (input: string) =
            input.Replace(cleanThis, "")

        /// <summary>
        /// Replace a string with another one in the input string
        /// </summary>
        /// <param name="replaceThis"> string to be replaced </param>
        /// <param name="withThis"> string that will replace replaceThis </param>
        /// <param name="input"> input string </param>
        /// <returns> The toClean String without any occurrence of cleanThis </returns>
        let replaceString ((replaceThis: string), withThis) (input: string) = 
            input.Replace(replaceThis, withThis)