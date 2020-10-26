namespace Cmhd.SuccintDataStructures

/// Bitmap type representing an array of bits; because of the limitations of 
/// numeric types its implemented as a byte[]    
type Bitmap

/// Bitmap module implementing rank and select operations. The ideas behind the
/// specifics of this module are inpired in the paper PRACTICAL IMPLEMENTATION
/// OF RANK AND SELECT QUERIES  
module Bitmap =
    /// <summary>
    /// Create a Bitmap from a string.
    /// </summary>
    val ofString: (string -> Result<Bitmap, SDSError>)

    /// <summary>
    /// Create a Bitmap from a byte array.
    /// </summary>
    val ofByteArray: (byte[] -> Result<Bitmap, SDSError>)
        
    /// <summary>
    /// Length of the bitmap
    /// </summary>        
    /// <param name="bitmap"> Bitmap data structure </param>
    val length: bitmap: Bitmap -> Result<int,SDSError>
        
    /// <summary>
    /// Given a position, it return the number of 1s up to that position in
    /// the bitmap.
    /// </summary>
    /// <param name="position"> Bit position (starts at 1) </param>
    /// <param name="bitmap"> Bitmap data structure </param>
    val rank: position: int -> bitmap: Bitmap -> Result<int, SDSError>

    /// <summary>
    /// Given a position, it return the number of 0s up to that position in
    /// the bitmap.
    /// </summary>
    /// <param name="position"> Bit position (starts at 1) </param>
    /// <param name="bitmap"> Bitmap data structure </param>
    val rank0: position: int -> bitmap: Bitmap -> Result<int, SDSError>

    /// <summary>
    /// Given a number nth, it return the position of the nth 1 in the bitmap.
    /// </summary>
    /// <param name="nth"> Nth 1 to look up </param>
    /// <param name="bitmap"> Bitmap data structure </param>
    val select: nth: int -> bitmap: Bitmap -> Result<int, SDSError>

    /// <summary>
    /// Given a number nth, it return the position of the nth 0 in the bitmap.
    /// </summary>
    /// <param name="nth"> Nth 0 to look up </param>
    /// <param name="bitmap"> Bitmap data structure </param>
    val select0: nth: int -> bitmap: Bitmap -> Result<int, SDSError>

    /// <summary>
    /// Given a number nth, it return the value of the nth bit in the bitmap.
    /// </summary>
    /// <param name="nth"> Nth bit </param>
    /// <param name="bitmap"> Bitmap data structure </param>
    val nthBit: nth: int -> bitmap: Bitmap -> Result<int, SDSError>

    /// <summary>
    /// Given an index nth, it return true if the index is in the bitmap range
    /// </summary>
    /// <param name="nth"> Nth bit </param>
    /// <param name="bitmap"> Bitmap data structure </param>
    val isValidBit: nth: int -> bitmap: Bitmap -> bool