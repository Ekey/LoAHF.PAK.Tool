using System;

namespace LoAHF.Cache.Unpacker
{
    class CacheHeader
    {
        public UInt32 dwMagic { get; set; } // 0x43465442 (BTFC)
        public Int32 dwVersion { get; set; } // 0
        public Int32 dwTotalFiles { get; set; }
    }
}
