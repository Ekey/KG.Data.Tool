using System;
using System.IO;
using System.Collections.Generic;

namespace KG.Unpacker
{
    class KfcUnpack
    {
        private static List<KfcEntry> m_EntryTable = new List<KfcEntry>();

        public static void iDoIt(String m_DirFile, String m_DstFolder)
        {
            KfcList.iLoadProject();
            using (FileStream TDirStream = File.OpenRead(m_DirFile))
            {
                var m_Header = new KfcHeader();

                m_Header.dwMagic = TDirStream.ReadUInt32();
                m_Header.dwTotalFiles1 = TDirStream.ReadInt32();
                m_Header.dwTotalFiles2 = TDirStream.ReadInt32();
                m_Header.dwReserved = TDirStream.ReadInt32();
                m_Header.dwArchiveSize = TDirStream.ReadInt64();

                if (m_Header.dwMagic != 0x3043464B)
                {
                    Utils.iSetError("[ERROR]: Invalid magic of KFC_DIR file");
                    return;
                }

                m_EntryTable.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFiles1; i++)
                {
                    var m_Entry = new KfcEntry();

                    m_Entry.dwHashName = TDirStream.ReadUInt64();

                    m_EntryTable.Add(m_Entry);
                }

                for (Int32 i = 0; i < m_Header.dwTotalFiles1; i++)
                {
                    var m_Entry = m_EntryTable[i];

                    m_Entry.dwDecompressedSize = TDirStream.ReadInt32();
                    m_Entry.dwCompressedSize = TDirStream.ReadInt32();
                    m_Entry.dwFileNum = TDirStream.ReadInt32();
                    m_Entry.dwFlag = TDirStream.ReadInt32();
                }

                for (Int32 i = 0; i < m_Header.dwTotalFiles1; i++)
                {
                    var m_Entry = m_EntryTable[i];

                    m_Entry.dwOffset = TDirStream.ReadInt64();
                }

                TDirStream.Dispose();
            }

            String m_DataFile = Path.GetDirectoryName(m_DirFile) + @"\" + Path.GetFileNameWithoutExtension(m_DirFile) + ".kfc_data";
            using (FileStream TDataStream = File.OpenRead(m_DataFile))
            {
                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FileName = KfcList.iGetNameFromHashList(m_Entry.dwHashName);
                    String m_FullPath = m_DstFolder + m_FileName.Replace("/", @"\");

                    if (!File.Exists(m_FullPath))
                    {
                        Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                        Utils.iCreateDirectory(m_FullPath);

                        TDataStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);

                        var lpBuffer = TDataStream.ReadBytes(m_Entry.dwCompressedSize);

                        File.WriteAllBytes(m_FullPath, lpBuffer);
                    }
                }

                TDataStream.Dispose();
            }
        }
    }
}
