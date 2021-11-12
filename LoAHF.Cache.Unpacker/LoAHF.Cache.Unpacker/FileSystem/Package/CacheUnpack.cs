using System;
using System.IO;
using System.Collections.Generic;

namespace LoAHF.Cache.Unpacker
{
    class CacheUnpack
    {
        static List<CacheEntry> m_EntryTable = new List<CacheEntry>();

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            using (FileStream TCacheStream = File.OpenRead(m_Archive))
            {
                var lpHeader = TCacheStream.ReadBytes(12);
                var m_Header = new CacheHeader();

                using (var THeaderReader = new MemoryStream(lpHeader))
                {
                    m_Header.dwMagic = THeaderReader.ReadUInt32();
                    m_Header.dwVersion = THeaderReader.ReadInt32();
                    m_Header.dwTotalFiles = THeaderReader.ReadInt32();

                    if (m_Header.dwMagic != 0x43465442)
                    {
                        throw new Exception("[ERROR]: Invalid magic of Cache file!");
                    }

                    THeaderReader.Dispose();
                }

                m_EntryTable.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    Int32 dwNameLength = TCacheStream.ReadInt32();
                    UInt32 dwHash = TCacheStream.ReadUInt32();
                    String m_FileName = TCacheStream.ReadStringByLength((Int32)CacheUtils.iAlignUInt32((UInt32)dwNameLength, 4u)).Trim('\0');
                    UInt32 dwOffset = TCacheStream.ReadUInt32();
                    Int32 dwSize = TCacheStream.ReadInt32();

                    var TEntry = new CacheEntry
                    {
                        dwNameLength = dwNameLength,
                        dwHash = dwHash,
                        m_FileName = m_FileName,
                        dwOffset = dwOffset,
                        dwSize = dwSize,
                    };

                    m_EntryTable.Add(TEntry);
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FullPath = m_DstFolder + m_Entry.m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_Entry.m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TCacheStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
                    var lpBuffer = TCacheStream.ReadBytes((Int32)m_Entry.dwSize);

                    File.WriteAllBytes(m_FullPath, lpBuffer);
                }
            }
        }
    }
}
