using System;
using System.IO;
using System.Collections.Generic;

namespace LoAHF.Scripts.Unpacker
{
    class ScriptsUnpack
    {
        static List<ScriptsEntry> m_EntryTable = new List<ScriptsEntry>();

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            using (FileStream TScriptStream = File.OpenRead(m_Archive))
            {
                var lpHeader = TScriptStream.ReadBytes(12);
                var m_Header = new ScriptsHeader();

                using (var THeaderReader = new MemoryStream(lpHeader))
                {
                    m_Header.dwMagic = THeaderReader.ReadUInt32();
                    m_Header.dwVersion = THeaderReader.ReadInt32();
                    m_Header.dwTotalFiles = THeaderReader.ReadInt32();

                    if (m_Header.dwMagic != 0x41554C4A)
                    {
                        throw new Exception("[ERROR]: Invalid magic of script cache file!");
                    }

                    THeaderReader.Dispose();
                }

                m_EntryTable.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    Int32 dwFileID = TScriptStream.ReadInt32();
                    UInt32 dwHash = TScriptStream.ReadUInt32();
                    UInt32 dwOffset = TScriptStream.ReadUInt32();
                    Int32 dwSize = TScriptStream.ReadInt32();

                    var TEntry = new ScriptsEntry
                    {
                        dwFileID = dwFileID,
                        dwHash = dwHash,
                        dwOffset = dwOffset,
                        dwSize = dwSize,
                    };

                    m_EntryTable.Add(TEntry);
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FileName = ScriptsHashList.iGetNameFromHashList(m_Entry.dwHash);
                    String m_FullPath = m_DstFolder + m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TScriptStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
                    var lpBuffer = TScriptStream.ReadBytes((Int32)m_Entry.dwSize);

                    File.WriteAllBytes(m_FullPath, lpBuffer);
                }
            }
        }
    }
}
