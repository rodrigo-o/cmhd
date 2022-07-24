namespace Cmhd.SuccinctDataStructures

type Aggregate =
    | SUM
    | AVG
    | MAX
    | MIN
    | COUNT

type KNITreap = KNITreap of string

module KNITreap =

    let ofStrings = fun string -> KNITreap string

    