namespace Cmhd.SuccinctDataStructures

///Level Order Unary Degree Sequence (LOUDS) based on Bitmap.
type Louds

/// Level Order Unary Degree Sequence (LOUDS) module implementing traversal
/// and structure information operations.
//      https://www.computer.org/csdl/proceedings-article/focs/1989/063533/12OmNx2QUHQ (Download)
module Louds =
    
    /// <summary>
    /// Given a string it creates a Louds Data Structure
    /// </summary>
    val ofString: (string -> Result<Louds, SDSError>)

    /// <summary>
    /// Given a bitmap it creates a Louds Data Structure
    /// </summary>
    val ofBitmap: (Bitmap -> Result<Louds, SDSError>)

    /// <summary>
    /// Given a node, it return the first child.
    /// </summary>
    /// <param name="node"> louds node position (starting at 1) </param>
    /// <param name="louds"> Louds data structure </param>
    val firstChild: node: int -> louds: Louds -> Result<int, SDSError>
    
    /// <summary>
    /// Given a node, it return the last child.
    /// </summary>
    /// <param name="node"> louds node position (starting at 1) </param>
    /// <param name="louds"> Louds data structure </param>
    val lastChild: node: int -> louds: Louds -> Result<int, SDSError>
    
    /// <summary>
    /// Given a node, it return it's parent
    /// </summary>
    /// <param name="node"> louds node position (starting at 1) </param>
    /// <param name="louds"> Louds data structure </param>
    val parent: node: int -> louds: Louds -> Result<int, SDSError>
    
    /// <summary>
    /// Given a node, it return it's next sibling
    /// </summary>
    /// <param name="node"> louds node position (starting at 1) </param>
    /// <param name="louds"> Louds data structure </param>
    val nextSibling: node: int -> louds: Louds -> Result<int, SDSError>
    
    /// <summary>
    /// Given a node, it return it's previous sibling
    /// </summary>
    /// <param name="node"> louds node position (starting at 1) </param>
    /// <param name="louds"> Louds data structure </param>
    val prevSibling: node: int -> louds: Louds -> Result<int, SDSError>
    
    /// <summary>
    /// Given a louds, it return a list of the root nodes.
    /// </summary>    
    /// <param name="louds"> Louds data structure </param>
    val root: louds: Louds -> int list

    /// <summary>
    /// Given a nth position and a node, it return the nth child of the node
    /// </summary>
    /// <param name="position"> Position among the childrens </param>
    /// <param name="node"> louds node position (starting at 1) </param>
    /// <param name="louds"> Louds data structure </param>
    val child: position: int -> node: int -> louds: Louds -> Result<int, SDSError>
    
    /// <summary>
    /// Given a node, it return it's number of siblings plus himself
    /// </summary>
    /// <param name="node"> louds node position (starting at 1) </param>
    /// <param name="louds"> Louds data structure </param>
    val degree: node: int -> louds: Louds -> Result<int, SDSError>
    
    /// <summary>
    /// Given a node, it return the position among it's siblings
    /// </summary>
    /// <param name="node"> louds node position (starting at 1) </param>
    /// <param name="louds"> Louds data structure </param>
    val childRank: node: int -> louds: Louds -> Result<int, SDSError>

    /// <summary>
    /// Given a node, it returns the number of child it has
    /// </summary>
    /// <param name="node"> louds node position (starting at 1) </param>
    /// <param name="louds"> Louds data structure </param>
    val childQty: node: int -> louds: Louds -> Result<int, SDSError>

    /// <summary>
    /// Given a node, it return an array of all it's ancestors, including himself
    /// </summary>
    /// <param name="node"> louds node position (starting at 1) </param>
    /// <param name="louds"> Louds data structure </param>
    val ancestors: node: int -> louds: Louds -> int list

    /// <summary>
    /// Given a node, it return an array of all it's childrens.
    /// </summary>
    /// <param name="node"> louds node position (starting at 1) </param>
    /// <param name="louds"> Louds data structure </param>
    val childs: node: int -> louds: Louds -> int list

    /// <summary>
    /// Given a louds, it return the number of levels it has.
    /// </summary>    
    /// <param name="louds"> Louds data structure </param>
    val levels: louds: Louds -> int

    /// <summary>
    /// Given a Louds it retrun an array of the first and last node for every
    /// level.
    /// </summary>
    /// <param name="louds"> Louds data structure </param>
    val rangeLevels: louds: Louds -> (int * int) list
    
    /// <summary>
    /// Given a Louds it retrun a array of every valid node between first and 
    /// last node.
    /// </summary>
    /// <param name="levelStart"> start node position </param>
    /// <param name="levelEnd"> end node position </param>
    /// <param name="louds"> Louds data structure </param>
    val rangeMembers: levelStart: int * levelEnd: int -> louds: Louds -> int list
    
    /// <summary>
    /// Given a Louds it return a list of the first node of every level
    /// </summary>
    /// <param name="louds"> Louds data structure </param>
    val firstNodePerLevel: louds: Louds -> seq<int>
    
