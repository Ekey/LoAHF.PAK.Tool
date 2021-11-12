using System;

namespace LoAHF.Cache.Unpacker
{
    class CacheEntry
    {
        public Int32 dwNameLength { get; set; }
        public UInt32 dwHash { get; set; }
        public String m_FileName { get; set; }
        public UInt32 dwOffset { get; set; }
        public Int32 dwSize { get; set; }
    }
}
