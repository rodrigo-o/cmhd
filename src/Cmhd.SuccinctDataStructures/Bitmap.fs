namespace Cmhd.SuccinctDataStructures

open Common
open System.Text.RegularExpressions

[<StructuredFormatDisplayAttribute("Bitmap {AsString}")>]
type Bitmap =
    | Bitmap of byte[]

    override this.ToString() =
        match this with
        | Bitmap byteArr -> 
            $"Bitmap {byteArr |> byteArrayToString}"

    /// Pretty printing member for %A prints
    member this.AsString = 
        this.ToString()
        |> String.clean "Bitmap"
        |> String.clean " "
        |> String.replaceString("byte0b", " ")
        |> String.replaceString("|]", " |]")


module Bitmap =
     
    [<AutoOpen>]
    module private Implementation =
        /// Population count array containing the number of set bits for a
        /// given value (represented by the index), in this case it contains
        /// the first 255 numbers a.k.a a byte
        let popCount =
            [| 0; 1; 1; 2; 1; 2; 2; 3; 1; 2; 2; 3; 2; 3; 3; 4; 1; 2; 2; 3; 2; 3;
               3; 4; 2; 3; 3; 4; 3; 4; 4; 5; 1; 2; 2; 3; 2; 3; 3; 4; 2; 3; 3; 4;
               3; 4; 4; 5; 2; 3; 3; 4; 3; 4; 4; 5; 3; 4; 4; 5; 4; 5; 5; 6; 1; 2;
               2; 3; 2; 3; 3; 4; 2; 3; 3; 4; 3; 4; 4; 5; 2; 3; 3; 4; 3; 4; 4; 5;
               3; 4; 4; 5; 4; 5; 5; 6; 2; 3; 3; 4; 3; 4; 4; 5; 3; 4; 4; 5; 4; 5;
               5; 6; 3; 4; 4; 5; 4; 5; 5; 6; 4; 5; 5; 6; 5; 6; 6; 7; 1; 2; 2; 3;
               2; 3; 3; 4; 2; 3; 3; 4; 3; 4; 4; 5; 2; 3; 3; 4; 3; 4; 4; 5; 3; 4;
               4; 5; 4; 5; 5; 6; 2; 3; 3; 4; 3; 4; 4; 5; 3; 4; 4; 5; 4; 5; 5; 6;
               3; 4; 4; 5; 4; 5; 5; 6; 4; 5; 5; 6; 5; 6; 6; 7; 2; 3; 3; 4; 3; 4;
               4; 5; 3; 4; 4; 5; 4; 5; 5; 6; 3; 4; 4; 5; 4; 5; 5; 6; 4; 5; 5; 6;
               5; 6; 6; 7; 3; 4; 4; 5; 4; 5; 5; 6; 4; 5; 5; 6; 5; 6; 6; 7; 4; 5;
               5; 6; 5; 6; 6; 7; 5; 6; 6; 7; 6; 7; 7; 8 |]
        
        /// Giving a byte, it use it for referencing the popCount array by 
        /// the number of bits.
        let popCountFor byte = (popCount.[int byte &&& 0xFF])

        /// Giving a bitmap, it calculates its population count using the 
        /// popCountFor function.
        let bitmapPopCount (Bitmap byteArray) =
            byteArray 
            |> Array.fold (fun acc x -> acc + popCountFor x) 0
    
        /// Length of the byte array implementing the bitmap.
        let implLength (Bitmap byteArray) = byteArray |> Array.length
        
        /// Obtain the first byte of the bitmap.
        let firstByte (Bitmap byteArray) = byteArray.[0]

        /// Obtain the nth byte of bitmap.
        let byteAt nth (Bitmap byteArray) =
            let len = byteArray |> Array.length
            match nth >= 1 && len >= nth with
            | true -> Ok byteArray.[nth - 1]
            | _ -> Error <| BitmapError (ImplIndexOutOfBound (len, nth))            

        /// Length of the first byte of a bitmap starting at the leftmost set bit.
        let firstByteFromSetBitLength bitmap =
            let byte = firstByte bitmap

            [0..7]
            |> List.skipWhile (fun index -> (byte >>> index) <> 1uy)
            |> List.head
            |> (+) 1

        /// Length of the bitmap starting at the leftmost set bit. 
        let fromSetBitLength bitmap =
            bitmap
            |> firstByteFromSetBitLength
            |> fun fstLen -> (fstLen + (implLength bitmap - 1) * 8)

        /// Check that the nth bit is valid and returns the length of the bitmap. 
        /// If the bit is not valid, it returns an IndexOutOfBound error.
        let lengthIfValidBit nth bitmap =
            bitmap 
            |> fromSetBitLength
            |> fun len ->
                if 1 <= nth && nth <= len 
                then Ok len
                else Error <| BitmapError (IndexOutOfBound (len, nth))

        /// Check that the nth bit is valid returning true if it is.
        let isValidBit' nth bitmap =
            bitmap
            |> lengthIfValidBit nth
            |> Result.isOk
      
        /// Check that the nth bit is valid and returns the length of the first 
        /// byte. If the bit is not valid, it returns an IndexOutOfBound error.
        let firstByteLengthIfValidBit nth bitmap =
            bitmap
            |> lengthIfValidBit nth
            >>= fun len -> Ok (len % 8)
        
        /// Given and mth position at the byte array it return it's nth bit due
        /// to a given shift
        let fromMthNthBit mth nth bitmap =
            let shift = (8 - nth) % 8

            bitmap
            |> byteAt mth
            >>= fun mthByte -> Ok (int mthByte &&& (1 <<< shift))
        
        /// Get the actual nth bit counting from the leftmost set bit.
        let nthFromSetBit nth bitmap =
            bitmap
            |> firstByteLengthIfValidBit nth
            >>= fun fstLen ->
                if nth <= fstLen
                then bitmap |> fromMthNthBit 1 (8 - fstLen + nth)
                else bitmap |> fromMthNthBit ((nth - fstLen - 1) / 8 + 2) 
                                             ((nth - fstLen) % 8)
            >>= function | 0 -> Ok 0 | _ -> Ok 1                
                 
        /// Given a number n to take and a byte it return the first n bits of 
        /// that byte.
        let takeFirsts n target =
            target &&& ((255uy <<< (8 - n)) &&& 255uy)
        
        /// Given an nth number and a bitmap it validates the length of the 
        /// bitmap and return a tuple of indices to use for chunking
        let indicesForChunk nth bitmap = 
            bitmap 
            |> firstByteLengthIfValidBit nth
            >>= fun fstLen -> Ok (nth, nth - fstLen, (nth - fstLen) % 8, fstLen)

        /// Given a tuple of indices return a chunk of the bitmap
        let private indexedChunk (Bitmap bitmap) (chunk, index, rest, fstLen) =
            // FIXME: try to not open the Bitmap, instead use private functions
            // also, this function appears to be more complex than it should.
            match chunk <= fstLen, rest with
            | true , _ -> 
                Ok <| Bitmap [|takeFirsts chunk (bitmap.[0] <<< (8 - fstLen))|]
            | _, 0 -> 
                Ok <| Bitmap bitmap.[..index / 8]
            | _ , rest ->
                let newBitmap = bitmap.[..index / 8 + 1]
                Array.set newBitmap (index / 8 + 1) 
                    (takeFirsts rest bitmap.[index / 8 + 1])
                Ok <| Bitmap newBitmap

        /// Given a number n and a Bitmap it takes the first n bits counting
        /// from the first set bit
        let chunk n bitmap =
            bitmap 
            |> indicesForChunk n
            >>= indexedChunk bitmap

        /// Determine if the binary search ended
        let binaryFinished func searched bitmap act position =
            // this first match is for not executing prev when not needed
            match act = searched with
            | false -> Ok false
            | true ->
                bitmap
                |> lengthIfValidBit position
                >>= fun _ ->
                    match bitmap |> func (position - 1) with
                    | Error _ -> Ok true
                    | Ok prev -> Ok (act = searched && prev = (searched - 1))
        
        /// Binary Search a Bitmap using an argument function starting at half 
        /// of the length of the Bitmap and stepping into by half that, doing
        /// the same recursively until it could move only 1 step at a time.
        let binarySearch func len searched bitmap = 
            let finished = binaryFinished func searched bitmap
            let position, step = len / 2, len / 4
            let initial = bitmap |> func position

            // Don't using >>= here because of TCO in the recursive calls
            let rec search position step act =
                match act with
                | Error err -> Error err
                | Ok actVal ->
                    match finished actVal position with
                    | Error err -> Error err
                    | Ok true -> Ok position
                    | Ok false ->
                            match position, actVal < searched with
                            | 1, false when actVal = searched -> Ok 1
                            | _, true ->
                                search (position + step) (1 ||| step / 2) 
                                       (bitmap |> func (position + step)) 
                            | _, false ->
                                search (position - step) (1 ||| step / 2) 
                                       (bitmap |> func (position - step))
            
            search position step initial
        
        /// Given a searched that must be the result of searchFunc applied to
        /// a bitmap return the parameter for which it finds the result
        let searchFor searched searchFunc bitmap =
            bitmap
            |> lengthIfValidBit searched
            >>= fun len -> 
                bitmap |> binarySearch searchFunc len searched 
        
        /// Active pattern for recognizing valid bitmaps
        let (|ValidBitmap|_|) (candidate: string) =
            let pattern = Regex("(^[10]+$)")
            let matches = pattern.Match (candidate.Trim())
            if matches.Success then matches.Groups.[1].Value |> Some else None
        
        /// Given a byte array it checks if it's a valid Bitmap
        let validateByteArrayAsBitmap = function
            | [||] -> Error <| BitmapError EmptyBitmap
            | byteArr when byteArr.[0] = 0uy -> Error <| BitmapError FirstByteIs0
            | byteArr -> Ok <| Bitmap byteArr

        let validateStringAsBitmap = function
            | "" -> Error <| BitmapError EmptyBitmap
            | ValidBitmap bitmapString -> bitmapString 
                                          |> stringToByteArray 
                                          |> validateByteArrayAsBitmap
            | unknown -> Error <| BitmapError (UnidentifiedFormat unknown)



    let ofString = validateStringAsBitmap

    let ofByteArray = validateByteArrayAsBitmap
    
    let length bitmap =
        bitmap
        |> fromSetBitLength
    
    let rank position bitmap =
        bitmap
        |> chunk position
        >>= fun chunked_bitmap ->
            chunked_bitmap |> bitmapPopCount |> Result.return'

    let rank0 position bitmap = 
        bitmap 
        |> rank position 
        >>= fun res -> Ok (position - res)
    
    let select nth bitmap =
        bitmap
        |> searchFor nth rank

    let select0 nth bitmap =
        bitmap
        |> searchFor nth rank0

    let nthBit nth bitmap =
        bitmap
        |> nthFromSetBit nth
    
    let isValidBit nth bitmap = 
        bitmap 
        |> isValidBit' nth