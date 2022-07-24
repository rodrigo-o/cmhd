namespace Cmhd.SuccinctDataStructures

/// Defined aggregates for usage with the KNITreap as Values
type Aggregate =
    | SUM
    | AVG
    | MAX
    | MIN
    | COUNT

/// K^n Irregular treap data type
type KNITreap

/// This is a k^n irregular treap (KNITreap) module, a k^2 treap extended for
/// k^n partitions instead of k^2 and is irregular beacause partitions are not
/// equal in size:
//     - k^2 treap : https://dsi.face.ubiobio.cl/mcaniupan/pdfs/SCCC2017.pdf
//     - k^n treap : https://core.ac.uk/download/pdf/80522508.pdf (Section 2, 3)
//
// k^n irregular treaps are used for CMHD following the dimension structures
//
module KNITreap =
    
    /// <summary>
    /// Given two strings and a value map it creates a KNITreap Data Structure
    /// </summary>
    val ofStrings: (string -> KNITreap)
    // val ofStrings: (string -> string -> Map<Aggregate, int []> -> Result<KNITreap, SDSError>)

    // /// <summary>
    // /// Given two bitmaps and a value map it creates a KNITreap Data Structure
    // /// </summary>
    // val ofBitmaps: (Bitmap -> Bitmap -> Map<Aggregate, int []> -> Result<KNITreap, SDSError>)

    // /// <summary>
    // /// Given an aggregate, it return the values for that one.
    // /// </summary>
    // /// <param name="aggreagte"> Aggregate to be used for extract values </param>
    // /// <param name="knitreap"> k^n irregular treap data structure </param>
    // val values: aggregate: Aggregate -> knitreap: KNITreap -> int [] option

    // /// <summary>
    // /// Given a node, it return the first child.
    // /// </summary>
    // /// <param name="node"> KNITreap node position (starting at 1) </param>
    // /// <param name="knitreap"> k^n irregular treap data structure </param>
    // val firstChild: node: int -> knitreap: KNITreap -> Result<int, SDSError>
    
    // /// <summary>
    // /// Given a node, it returns the last child.
    // /// </summary>
    // /// <param name="node"> KNITreap node position (starting at 1) </param>
    // /// <param name="knitreap"> k^n irregular treap data structure </param>
    // val lastChild: node: int -> knitreap: KNITreap -> Result<int, SDSError>

    // /// <summary>
    // /// Given a node, it return it's parent
    // /// </summary>
    // /// <param name="node"> KNITreap node position (starting at 1) </param>
    // /// <param name="knitreap"> k^n irregular treap data structure </param>
    // val parent: node: int -> knitreap: KNITreap -> Result<int, SDSError>

    // /// <summary>
    // /// Given a node, it return it's next sibling
    // /// </summary>
    // /// <param name="node"> KNITreap node position (starting at 1) </param>
    // /// <param name="knitreap"> k^n irregular treap data structure </param>
    // val nextSibling: node: int -> knitreap: KNITreap -> Result<int, SDSError>
    
    // /// <summary>
    // /// Given a node, it return it's previous sibling
    // /// </summary>
    // /// <param name="node"> KNITreap node position (starting at 1) </param>
    // /// <param name="knitreap"> k^n irregular treap data structure </param>
    // val prevSibling: node: int -> knitreap: KNITreap -> Result<int, SDSError>

    // /// <summary>
    // /// Given a nth position and a node, it return the nth child of the node
    // /// </summary>
    // /// <param name="position"> Position among the childrens </param>
    // /// <param name="node"> KNITreap node position (starting at 1) </param>
    // /// <param name="knitreap"> k^n irregular treap data structure </param>
    // val child: position: int -> node: int -> knitreap: KNITreap -> Result<int, SDSError>

    // /// <summary>
    // /// Given a list of numbers, it return the nth child of every number 
    // /// effectively going down to that path through the structure
    // /// </summary>
    // /// <param name="path"> list of childrens </param>
    // /// <param name="knitreap"> k^n irregular treap data structure </param>
    // val childFromPath: path: int list -> knitreap: KNITreap -> Result<int, SDSError>