namespace Cmhd.SuccinctDataStructures

open System
open Common

[<StructuredFormatDisplayAttribute("Louds {AsString}")>]
type Louds =
    | Louds of Bitmap   

    override this.ToString() =
        let cleanBitmap = String.clean "Bitmap "
        
        match this with
        | Louds (bitmap) -> 
            $"Louds {bitmap.ToString() |> cleanBitmap}"


module Louds = 
    
    [<AutoOpen>]
    module private Implementation = 
        /// Given a Bitmap, it return the number of nodes
        let length (Louds bitmap) = bitmap |> Bitmap.length

        /// Given a position, it return the number of 1s up to that position in
        /// the louds bitmap.
        let rank n (Louds bitmap) = bitmap |> Bitmap.rank n
        
        /// Given a position, it return the number of 1s up to that position in
        /// the louds bitmap.
        let rank0 n (Louds bitmap) = bitmap |> Bitmap.rank0 n
        
        /// Given a number nth, it return the value of the nth bit in the louds
        /// bitmap.
        let nthBit n (Louds bitmap) = bitmap |> Bitmap.nthBit n
        
        /// Given a number nth, it return the position of the nth 1 in the 
        /// louds bitmap.
        let select n (Louds bitmap) = bitmap |> Bitmap.select n
        
        /// Given a number nth, it return the position of the nth 0 in the 
        /// louds bitmap.
        let select0 n (Louds bitmap) = bitmap |> Bitmap.select0 n

        /// Given an index nth, it return true if the index is in the louds 
        /// bitmap range
        let isValidBit n (Louds bitmap) = bitmap |> Bitmap.isValidBit n

        /// Given a louds and a louds function it generates a seq for every Ok 
        /// result returned by the recursive call of the louds function for a 
        /// given initial value, the order is first the value and then the
        /// function result.
        let valueFirstGenerator loudsFunc louds =
            let rec gen value = seq {
                match louds |> loudsFunc value with
                | Error _ -> yield value
                | Ok resNode ->
                    yield value
                    yield! gen resNode }
            gen

        /// Given a louds and a louds function it generates a seq for every Ok 
        /// result returned by the recursive call of the louds function for a 
        /// given initial value, the order is first the function result and then
        /// the value.
        let funcFirstGenerator loudsFunc louds =
            let rec gen value = seq {
                match louds |> loudsFunc value with
                | Error _ -> yield value
                | Ok resNode ->
                    yield! gen resNode 
                    yield value }
            gen
        
        /// Given a node, it return if it's part of the root
        let isFromRoot node louds =
            match louds |> rank0 node with
            | Ok 0 -> true
            | _ -> false

        /// Given a bitmap it checks if it's a valid Louds
        let validateBitmapAsLouds bitmap =
            // TODO: implement louds validation
            if true then Ok <| Louds bitmap     
                    else (Error <| LoudsError (InexistentChildFor 1))

        /// Given a string it checks if it's a valid Louds
        let validateStringAsLouds string =
            string
            |> Bitmap.ofString
            >>= validateBitmapAsLouds


    let ofString = validateStringAsLouds
    
    let ofBitmap = validateBitmapAsLouds
    
    let firstChild node louds =
        louds @ node 
        |>> rank 
        |>> select0
        |>= (fun nth -> 
            match isValidBit (nth + 1) louds with
            | true -> Ok (nth + 1)
            | false -> Error <| LoudsError (InexistentChildFor nth))
        |> errorMap <| LoudsError (InexistentChildFor node)

    let lastChild node louds =
        louds @ node 
        |>> rank 
        |>> fun node _ -> Ok (node + 1)
        |>> select0 
        |>= fun nth -> Ok (nth - 1)
        |> errorMap <| LoudsError (InexistentChildFor node)
        
    let parent node louds =
        louds @ node 
        |>> rank0 
        |>> select 
        |>= rid
        |> errorMap <| LoudsError (InexistentParentFor node)

    let nextSibling node louds =
        louds @ (node + 1)
        |>> nthBit
        |>= function 
            | 1 -> Ok (node + 1) 
            | _ -> Error <| LoudsError (InexistentNextSiblingFor node)

    let prevSibling node louds =
        louds @ (node - 1)
        |>> nthBit 
        |>= function 
            | 1 -> Ok (node - 1) 
            | _ -> Error <| LoudsError (InexistentPrevSiblingFor node)
    
    let root louds = 
        1 
        |> valueFirstGenerator nextSibling louds 
        |> Seq.toList

    let child position node louds =
        louds @ node 
        |>> firstChild 
        |>= fun fstChild ->        
            match position with
            | 1 -> Ok fstChild
            | n ->
                [ fstChild..(fstChild + n - 2) ]
                |> List.fold (fun acc _ -> 
                    acc
                    >>= fun node ->
                        louds @ node |>> nextSibling |>= rid) (Ok fstChild)
        |> errorMap <| LoudsError (InexistentNthChildFor (node, position))

    let degree node louds =
        match louds |> isFromRoot node with
        | true -> louds |> select0 1 >>= (fun x -> Ok (x - 1))
        | false ->
            let first = louds @ node |>> parent |>> firstChild |> fst
            let last = louds @ node |>> parent |>> lastChild |> fst
            match first, last with
            | Ok firstVal, Ok lastVal -> Ok (lastVal - firstVal + 1)
            | Error err, _ -> Error err
            | _, Error err -> Error err                

    let childRank node louds =
        match louds |> isFromRoot node with
        | true -> Ok node
        | false -> 
            louds @ node 
            |>> parent
            |>> firstChild 
            |>= fun firstSib -> Ok (node - firstSib + 1)

    let childQty node louds =
        // FIXME: check that InexistetChildFor could be misleadig due to errorMap
        louds @ node
        |>> firstChild
        |>= rid
        |> function 
            | Ok childNode -> louds |> degree childNode 
            | Error (LoudsError (InexistentChildFor _)) -> Ok 0
            | Error err -> Error err

    let ancestors node louds =     
        let genSeq = louds |> funcFirstGenerator parent

        match louds |> isValidBit node with
        | false -> []
        | true -> 
            node
            |> genSeq
            |> Seq.toList
    
    let childs node louds =
        match louds |> firstChild node with
        | Error _ -> []
        | Ok first ->
            first
            |> valueFirstGenerator nextSibling louds
            |> Seq.toList
    
    let levels louds =
        1 
        |> funcFirstGenerator firstChild louds
        |> Seq.distinct
        |> Seq.length
        |> fun x -> x - 2
    
    /// Function that act as an interleaved range generotor; usefull for range
    /// levels and for getting first or last of every level.
    let private interleavedRange ((_last, newStart), finish) louds = 
        match finish with
        | true -> Error <| LoudsError (InexistentNode -1)
        | false -> 
            louds 
            |> firstChild newStart 
            |> function 
                | Ok firstChild -> 
                    Ok ((firstChild - 2, firstChild), false)
                | Error _ -> 
                    louds 
                    |> length
                    |> fun len -> Ok ((len - 1, len + 1), true)

    let rangeLevels louds = 
        let ranges = 
            ((-1, 1), false)
            |> valueFirstGenerator interleavedRange louds 
        
        [] |> Seq.foldBack2 (fun ((_,act), _) ((last,_), _) acc ->
            (act, last) :: acc
        ) ranges (ranges |> Seq.tail)
    
    let rangeMembers (levelStart, levelEnd) louds =
        [levelStart..levelEnd]
        |> List.filter (fun node -> (louds |> nthBit node) = Ok 1)

    let firstNodePerLevel louds = 
        ((-1,1), false)
        |> valueFirstGenerator interleavedRange louds 
        |> Seq.map (fst >> snd)
               

type Louds with    
    /// Pretty printing member for %A prints
    member this.AsString = 
        let loudsString = 
            this.ToString()
            |> String.clean "Louds"
            |> String.clean " "
            |> String.clean ";byte0b"
            |> String.replaceString("byte0b"," ")
            |> String.clean "[| "
            |> String.clean "|]"
        
        let levelSeparators = Louds.firstNodePerLevel this

        let separatedLevels =
            Seq.map2 (fun x y ->
                loudsString.[x-1..y-2]
            ) levelSeparators (Seq.tail levelSeparators)

        let asString =
            String.Join("| ",separatedLevels) 
            |> String.replaceString("0", "0 ")

        $"[| {asString}|]" 
        

        
