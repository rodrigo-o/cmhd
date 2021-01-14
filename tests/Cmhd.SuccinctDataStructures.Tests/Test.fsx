type IA<'T> =
    abstract member Get : unit -> 'T

type MyClass() =
    interface IA<int> with
        member x.Get() = 1
    interface IA<string> with
        member x.Get() = "hello"

let mc = MyClass()
let iaInt = mc :> IA<int>
let iaString = mc :> IA<string>

iaInt.Get() // 1
iaString.Get() // "hello"

let oneOrAnother (mc: MyClass) t = 
    match box t with
    | :? int -> mc :> IA<int>
    | :? string -> mc :> IA<string>

let x:IA<'a> = 
    2
    |> oneOrAnother mc
    |> unbox
