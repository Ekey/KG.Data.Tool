using System;

namespace KG.Packer
{
    class KfcEntry
    {
        public UInt64 dwHashName { get; set; }
        public Int32 dwDecompressedSize { get; set; }
        public Int32 dwCompressedSize { get; set; }
        public Int32 dwFileNum { get; set; }
        public Int32 dwFlag { get; set; } // 1,2
        public Int64 dwOffset { get; set; }
    }
}
