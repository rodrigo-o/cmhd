#nowarn "25"
namespace Cmhd.SuccinctDataStructures.Tests

open Cmhd.SuccinctDataStructures

module Data =
    // Data creation functions
    let bitmapCreation byteArr =
        match Bitmap.ofByteArray byteArr with
        | Ok bitmap -> bitmap

    let loudsCreation byteArr =
        match Louds.ofBitmap (bitmapCreation byteArr) with
        | Ok louds -> louds

    // Bitmaps
    //
    // 11 10110101 10010101
    let bitmap = bitmapCreation [| byte 0b11; byte 0b10110101; byte 0b10010101 |]
            

    // LOUDS
    //
    // 10 | 1110 | 11110 10 110 | 10 10 10 110 110 1110 11110 
    let louds =  loudsCreation [| byte 0b10111; byte 0b01111010; byte 0b11010101;
                                  byte 0b01101101; byte 0b11011110 |]

    // 1110 | 11110 10 110 | 10 10 10 110 110 1110 11110 
    let noRootedLouds = loudsCreation [| byte 0b111; byte 0b01111010; byte 0b11010101;
                                         byte 0b01101101; byte 0b11011110 |]