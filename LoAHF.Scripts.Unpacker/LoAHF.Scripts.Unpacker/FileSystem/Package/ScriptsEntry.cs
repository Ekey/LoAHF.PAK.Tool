using System;

namespace LoAHF.Scripts.Unpacker
{
    class ScriptsEntry
    {
        public Int32 dwFileID { get; set; } // ???
        public UInt32 dwHash { get; set; } // FileName hash???
        public UInt32 dwOffset { get; set; }
        public Int32 dwSize { get; set; }
    }
}
