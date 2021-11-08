using System;

namespace LoAHF.Unpacker
{
    class PakHeader
    {
        public UInt32 dwMagic { get; set; } // 0x4B41504A (JPAK)
        public Int32 dwTotalFiles { get; set; }
        public Int32 dwArchiveSize { get; set; } // Max is 209715200
        public UInt32 dwHeaderCRC { get; set; } // 12 bytes of header
    }

    class PakBlockHeader
    {
        public Int32 dwMaxFiles { get; set; } // Max files in block (1000)
        public Int32 dwTotalFiles { get; set; } // Total files in current block
    }
}
