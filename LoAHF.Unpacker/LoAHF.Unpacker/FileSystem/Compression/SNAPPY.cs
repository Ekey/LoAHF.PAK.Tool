using System;
using Snappy.Sharp;

namespace LoAHF.Unpacker
{
    class SNAPPY
    {
        public static Byte[] iDecompress(Byte[] lpBuffer, Int32 dwCompressedSize)
        {
            var TSnappyDecompressor = new SnappyDecompressor();
            return TSnappyDecompressor.Decompress(lpBuffer, 0, dwCompressedSize);
        }

        public static Byte[] iDecompressByOffset(Byte[] lpBuffer, Int32 dwOffset, Int32 dwCompressedSize)
        {
            var TSnappyDecompressor = new SnappyDecompressor();
            return TSnappyDecompressor.Decompress(lpBuffer, dwOffset, dwCompressedSize);
        }
    }
}
