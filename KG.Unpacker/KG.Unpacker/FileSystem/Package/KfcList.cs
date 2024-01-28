using System;
using System.IO;
using System.Collections.Generic;

namespace KG.Unpacker
{
    class KfcList
    {
        private static String m_ProjectFile = Utils.iGetApplicationPath() + @"\Projects\FileNames.list";

        private static Dictionary<UInt64, String> m_HashList = new Dictionary<UInt64, String>();

        public static void iLoadProject()
        {
            String m_Line = null;
            if (!File.Exists(m_ProjectFile))
            {
                Utils.iSetWarning("[WARNING]: Unable to load project file " + m_ProjectFile);
            }

            Int32 i = 0;
            m_HashList.Clear();

            StreamReader TProjectFile = new StreamReader(m_ProjectFile);
            while ((m_Line = TProjectFile.ReadLine()) != null)
            {
                UInt64 dwHash = KfcHash.iGetHash(m_Line.ToLower());

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
                m_FileName = @"__Unknown/" + dwHash.ToString("X16");
            }

            return m_FileName;
        }
    }
}
