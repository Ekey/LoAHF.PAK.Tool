using System;
using System.IO;
using SevenZip.Compression.LZMA;

namespace LoAHF.Unpacker
{
    class LZMA
    {
        public static Byte[] iDecompress(Byte[] lpBuffer, Int32 dwSize)
        {
            Byte[] result;

            using (MemoryStream TDstMemoryStream = new MemoryStream())
            {
                using (MemoryStream TSrcMemoryStream = new MemoryStream(lpBuffer))
                {
                    Decoder LZMADecoder = new Decoder();

                    Byte[] lpProperties = new Byte[5];
                    TSrcMemoryStream.Read(lpProperties, 0, 5);

                    Byte[] fileLength = new Byte[8];
                    TSrcMemoryStream.Read(fileLength, 0, 8);
                    Int64 dwDecompressedSize = BitConverter.ToInt64(fileLength, 0);

                    LZMADecoder.SetDecoderProperties(lpProperties);
                    LZMADecoder.Code(TSrcMemoryStream, TDstMemoryStream, TSrcMemoryStream.Length, dwDecompressedSize, null);

                    result = TDstMemoryStream.ToArray();
                }
            }
            return result;
        }
    }
}
