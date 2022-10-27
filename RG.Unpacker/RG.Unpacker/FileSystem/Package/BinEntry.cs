using System;

namespace RG.Unpacker
{
    class BinEntry
    {
        public Int64 dwOffset { get; set; }
        public UInt32 dwHashName { get; set; }
        public Int32 dwDecompressedSize { get; set; }
        public Int32 dwCompressedSize { get; set; } // always 0
        public Int32 dwFileID { get; set; }
        public UInt16 wUnknown1 { get; set; }
        public UInt16 wUnknown2 { get; set; }
        public Int32 dwFlag { get; set; } // -1
    }
}
