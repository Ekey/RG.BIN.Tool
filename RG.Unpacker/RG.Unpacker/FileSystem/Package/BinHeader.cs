using System;

namespace RG.Unpacker
{
    class BinHeader
    {
        public UInt32 dwMagic { get; set; } // 0x2E6E6962 (bin.)
        public Int32 dwHeaderSize { get; set; } // 32 
        public Int32 dwEntryDataOffset { get; set; } // + 16
        public Int64 dwTableDecompressedSize { get; set; }
        public Int64 dwTableCompressedSize { get; set; }
        public Int32 dwFlag { get; set; } // -1 (Compression flag??)
    }
}
