namespace Cmhd.SuccinctDataStructures

/// Bitmap related errors
type BitmapError =
    | EmptyBitmap
    | FirstByteIs0
    | UnidentifiedFormat of given: string
    | IndexOutOfBound of length: int * index: int
    | ImplIndexOutOfBound of implLength: int * implIndex: int
    
/// SuccinctDataStructures errors for simpler composition; it contains all kind 
/// of errors
type SDSError =
    | BitmapError of BitmapError
