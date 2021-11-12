using System;

namespace LoAHF.Scripts.Unpacker
{
    class ScriptsHeader
    {
        public UInt32 dwMagic { get; set; } // 0x41554C4A (JLUA)
        public Int32 dwVersion { get; set; } // 1
        public Int32 dwTotalFiles { get; set; }
    }
}
