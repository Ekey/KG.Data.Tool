using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace KG.Packer
{
    class KfcPack
    {
        private static List<KfcEntry> m_EntryTable = new List<KfcEntry>();

        public static void iDoIt(String m_SrcFolder, String m_DstFile)
        {
            Int32 dwFileNum = 0;

            var m_Files = Directory.GetFiles(m_SrcFolder, "*.*", SearchOption.AllDirectories);

            m_EntryTable.Clear();
            using (BinaryWriter TDataStream = new BinaryWriter(File.Open(m_DstFile + ".kfc_data", FileMode.Create)))
            {
                foreach (var m_File in m_Files)
                {
                    String m_FileName = m_File.Replace(m_SrcFolder + @"\", "");

                    Utils.iSetInfo("[PACKING]: " + m_FileName);

                    var m_Entry = new KfcEntry();

                    if (!m_File.Contains("__Unknown"))
                    {
                        m_Entry.dwHashName = KfcHash.iGetHash(m_FileName);
                    }
                    else
                    {
                        m_Entry.dwHashName = Convert.ToUInt64(Path.GetFileNameWithoutExtension(m_FileName), 16);
                    }

                    var lpBuffer = File.ReadAllBytes(m_File);

                    m_Entry.dwDecompressedSize = lpBuffer.Length;
                    m_Entry.dwCompressedSize = lpBuffer.Length;
                    m_Entry.dwFileNum = dwFileNum++;
                    m_Entry.dwFlag = 1;
                    m_Entry.dwOffset = TDataStream.BaseStream.Position;

                    TDataStream.Write(lpBuffer);

                    m_EntryTable.Add(m_Entry);
                }

                m_EntryTable = m_EntryTable.OrderBy(m_Entry => m_Entry.dwHashName).ToList();

                using (BinaryWriter TDirStream = new BinaryWriter(File.Open(m_DstFile + ".kfc_dir", FileMode.Create)))
                {
                    var m_Header = new KfcHeader();

                    m_Header.dwMagic = 0x3043464B;
                    m_Header.dwTotalFiles1 = m_Files.Length;
                    m_Header.dwTotalFiles2 = m_Files.Length;
                    m_Header.dwReserved = 0;
                    m_Header.dwArchiveSize = TDataStream.BaseStream.Length;

                    TDirStream.Write(m_Header.dwMagic);
                    TDirStream.Write(m_Header.dwTotalFiles1);
                    TDirStream.Write(m_Header.dwTotalFiles2);
                    TDirStream.Write(m_Header.dwReserved);
                    TDirStream.Write(m_Header.dwArchiveSize);

                    foreach (var m_Entry in m_EntryTable)
                    {
                        TDirStream.Write(m_Entry.dwHashName);
                    }

                    foreach (var m_Entry in m_EntryTable)
                    {
                        TDirStream.Write(m_Entry.dwDecompressedSize);
                        TDirStream.Write(m_Entry.dwCompressedSize);
                        TDirStream.Write(m_Entry.dwFileNum);
                        TDirStream.Write(m_Entry.dwFlag);
                    }

                    foreach (var m_Entry in m_EntryTable)
                    {
                        TDirStream.Write(m_Entry.dwOffset);
                    }

                    TDirStream.Dispose();
                }

                TDataStream.Dispose();
            }
        }
    }
}
