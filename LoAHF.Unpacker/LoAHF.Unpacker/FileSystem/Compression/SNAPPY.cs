using System;
using Snappy.Sharp;

namespace LoAHF.Unpacker
{
    class SNAPPY
    {
        public static Byte[] iDecompress(Byte[] lpBuffer, Int32 dwCompressedSize, Int32 dwOffset = 0)
        {
            var TSnappyDecompressor = new SnappyDecompressor();
            return TSnappyDecompressor.Decompress(lpBuffer, dwOffset, dwCompressedSize);
        }
    }
}
