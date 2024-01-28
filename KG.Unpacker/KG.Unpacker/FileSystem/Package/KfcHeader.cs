using System;

namespace KG.Unpacker
{
    class KfcHeader
    {
        public UInt32 dwMagic { get; set; } //0x3043464B (KFC0)
        public Int32 dwTotalFiles1 { get; set; }
        public Int32 dwTotalFiles2 { get; set; } // same as dwTotalFiles1
        public Int32 dwReserved { get; set; } // 0
        public Int64 dwArchiveSize { get; set; }
    }
}
