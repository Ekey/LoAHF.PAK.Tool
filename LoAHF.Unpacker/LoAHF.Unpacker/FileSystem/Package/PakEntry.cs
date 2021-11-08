using System;

namespace LoAHF.Unpacker
{
    class PakEntry
    {
        public UInt64 dwUnknown { get; set; } // File hash???
        public UInt64 dwNameHash { get; set; } // FileName hash V2 + V3
        public UInt32 dwOffset { get; set; }
        public Int32 dwCompressedSize { get; set; }
        public Int32 dwEncryptedSize { get; set; }
        public Int32 dwDecompressedSize { get; set; }
        public UInt16 wFlag1 { get; set; }
        public UInt16 wFlag2 { get; set; }
    }
}
