using System;
using System.IO;
using System.Collections.Generic;

namespace LoAHF.Unpacker
{
    class PakHashList
    {
        static String m_Path = Utils.iGetApplicationPath();
        static String m_ProjectFile = @"\Projects\FileNames.list";
        static String m_ProjectFilePath = m_Path + m_ProjectFile;

        static Dictionary<UInt64, String> m_HashList = new Dictionary<UInt64, String>();

        public static void iLoadProject()
        {
            String m_Line = null;
            if (!File.Exists(m_ProjectFilePath))
            {
                Utils.iSetWarning("[WARNING]: Unable to load project file " + m_ProjectFile);
                return;
            }

            Int32 i = 0;
            m_HashList.Clear();

            StreamReader TProjectFile = new StreamReader(m_ProjectFilePath);
            while ((m_Line = TProjectFile.ReadLine()) != null)
            {
                UInt32 dwHashV2 = Hash.iGetHash_v2(m_Line);
                UInt32 dwHashV3 = Hash.iGetHash_v3(m_Line);
                UInt64 dwHash = (UInt64)dwHashV3 << 32 | dwHashV2;

                if (m_HashList.ContainsKey(dwHash))
                {
                    String m_Collision = null;
                    m_HashList.TryGetValue(dwHash, out m_Collision);
                    Utils.iSetError("[COLLISION]: " + m_Collision + " <-> " + m_Line);
                }

                m_HashList.Add(dwHash, m_Line);
                i++;
            }

            TProjectFile.Close();
            Utils.iSetInfo("[INFO]: Project File Loaded: " + i.ToString());
            Console.WriteLine();
        }

        public static String iGetNameFromHashList(UInt64 dwHash)
        {
            String m_FileName = null;

            if (m_HashList.ContainsKey(dwHash))
            {
                m_HashList.TryGetValue(dwHash, out m_FileName);
            }
            else
            {
                m_FileName = @"__Unknown\" + dwHash.ToString("X16");
            }

            return m_FileName;
        }
    }
}
