using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace LoAHF.Unpacker
{
    class PakUnpack
    {
        private static List<PakEntry> m_EntryTable = new List<PakEntry>();

        private static Byte[] iGetLibraryData(Byte[] lpBuffer)
        {
            Int32 dwOffset = 0;
            Int32 bLength = lpBuffer[0];

            dwOffset += 1 + bLength + 16;

            Byte[] lpTemp = new Byte[lpBuffer.Length - dwOffset];

            Array.Copy(lpBuffer, dwOffset, lpTemp, 0, lpBuffer.Length - dwOffset);

            return lpTemp;
        }

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            PakHashList.iLoadProject();
            using (FileStream TPakStream = File.OpenRead(m_Archive))
            {
                var lpHeader = TPakStream.ReadBytes(16);
                var m_Header = new PakHeader();

                using (var THeaderReader = new MemoryStream(lpHeader))
                {
                    m_Header.dwMagic = THeaderReader.ReadUInt32();
                    m_Header.dwTotalFiles = THeaderReader.ReadInt32();
                    m_Header.dwArchiveSize = THeaderReader.ReadInt32();
                    m_Header.dwHeaderCRC = THeaderReader.ReadUInt32();

                    if (m_Header.dwMagic != 0x4B41504A)
                    {
                        throw new Exception("[ERROR]: Invalid magic of PAK file!");
                    }

                    THeaderReader.Dispose();
                }

                //For patch PAK files
                if (m_Header.dwTotalFiles == 0) {
                    m_Header.dwTotalFiles += 1;
                }

                m_EntryTable.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    if (TPakStream.Position >= m_Header.dwArchiveSize) {
                        break;
                    }

                    var lpBlock = TPakStream.ReadBytes(8);
                    var m_BlockHeader = new PakBlockHeader();

                    using (var TBlockHeaderReader = new MemoryStream(lpBlock))
                    {
                        m_BlockHeader.dwMaxFiles = TBlockHeaderReader.ReadInt32();
                        m_BlockHeader.dwTotalFiles = TBlockHeaderReader.ReadInt32();
                    }

                    var lpTable = TPakStream.ReadBytes(m_BlockHeader.dwTotalFiles * 36);
                    using (MemoryStream TEntryReader = new MemoryStream(lpTable))
                    {
                        for (Int32 j = 0; j < m_BlockHeader.dwTotalFiles; j++, i++)
                        {
                            UInt64 dwUnknown = TEntryReader.ReadUInt64();
                            UInt64 dwNameHash = TEntryReader.ReadUInt64();
                            UInt32 dwOffset = TEntryReader.ReadUInt32();
                            Int32 dwCompressedSize = TEntryReader.ReadInt32();
                            Int32 dwEncryptedSize = TEntryReader.ReadInt32();
                            Int32 dwDecompressedSize = TEntryReader.ReadInt32();
                            UInt16 wIsCompressed = TEntryReader.ReadUInt16();
                            UInt16 wCompressionType = TEntryReader.ReadUInt16();
                            wCompressionType = (UInt16)((wCompressionType << 8) | (wCompressionType >> 8));
                            wCompressionType &= 0xF;

                            var TEntry = new PakEntry
                            {
                                dwUnknown = dwUnknown,
                                dwNameHash = dwNameHash,
                                dwOffset = dwOffset,
                                dwCompressedSize = dwCompressedSize,
                                dwEncryptedSize = dwEncryptedSize,
                                dwDecompressedSize = dwDecompressedSize,
                                wIsCompressed = wIsCompressed,
                                wCompressionType = (PakFlags)wCompressionType,
                            };

                            m_EntryTable.Add(TEntry);
                        }
                    }

                    TPakStream.Position = m_EntryTable[m_EntryTable.Count - 1].dwOffset + m_EntryTable[m_EntryTable.Count - 1].dwEncryptedSize;
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FileName = PakHashList.iGetNameFromHashList(m_Entry.dwNameHash);
                    String m_FullPath = m_DstFolder + m_FileName;

                    if (m_Entry.dwCompressedSize != 0 && m_Entry.dwDecompressedSize != 0)
                    {
                        Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                        Utils.iCreateDirectory(m_FullPath);

                        TPakStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
                        var lpBuffer = TPakStream.ReadBytes((Int32)m_Entry.dwCompressedSize);
                        lpBuffer = PakCipher.iDecryptData(lpBuffer, m_Entry.dwNameHash, m_Entry.dwCompressedSize);

                        if (m_Entry.wCompressionType == PakFlags.NONE)
                        {
                            //game.dll, D3DCompiler_47.dll
                            if (m_Entry.dwNameHash == 0x8D6D71A8587761CD || m_Entry.dwNameHash == 0xA7D7D024316132AE)
                            {
                                lpBuffer = iGetLibraryData(lpBuffer);
                            }

                            File.WriteAllBytes(m_FullPath, lpBuffer);
                        }
                        else if(m_Entry.wCompressionType == PakFlags.LZO)
                        {
                            //TODO.... (unused at this time)
                            File.WriteAllBytes(m_FullPath, lpBuffer);
                        }
                        else if (m_Entry.wCompressionType == PakFlags.LZMA)
                        {
                            var lpDstBuffer = LZMA.iDecompress(lpBuffer, m_Entry.dwCompressedSize);
                            File.WriteAllBytes(m_FullPath, lpDstBuffer);
                        }
                        else if (m_Entry.wCompressionType == PakFlags.SNAPPY)
                        {
                            //game.dll, D3DCompiler_47.dll
                            if (m_Entry.dwNameHash == 0x8D6D71A8587761CD || m_Entry.dwNameHash == 0xA7D7D024316132AE)
                            {
                                lpBuffer = iGetLibraryData(lpBuffer);
                            }

                            var lpDstBuffer = SNAPPY.iDecompress(lpBuffer, lpBuffer.Length);
                            File.WriteAllBytes(m_FullPath, lpDstBuffer);
                        }
                        else
                        {
                            throw new Exception("[ERROR]: Unknown compression type -> " + m_Entry.wCompressionType.ToString());
                        }
                    }
                    else
                    {
                        Utils.iSetWarning("[SKIPPED]: File " + m_FileName + " was removed from archive");
                    }
                }
            }
        }
    }
}
