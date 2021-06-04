namespace Cmhd.SuccinctDataStructures

/// Bitmap related errors
type BitmapError =
    | EmptyBitmap
    | FirstByteIs0
    | UnidentifiedFormat of given: string
    | IndexOutOfBound of length: int * index: int
    | ImplIndexOutOfBound of implLength: int * implIndex: int

/// Tree like related errors
type TreeLikeError =
    | InexistentNode of node: int
    | InexistentChildFor of node: int
    | InexistentNthChildFor of node: int * nth: int
    | InexistentParentFor of node: int
    | InexistentNextSiblingFor of node: int
    | InexistentPrevSiblingFor of node: int

/// SuccinctDataStructures errors for simpler composition; it contains all kind 
/// of errors
type SDSError =
    | BitmapError of BitmapError
    | LoudsError of TreeLikeError