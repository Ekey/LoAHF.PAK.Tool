using System;

namespace LoAHF.Unpacker
{
    [Flags]
    public enum PakFlags : UInt16
    {
        NONE = 0,
        LZO = 1,
        LZMA = 2,
        SNAPPY = 3,
    }
}
