using System;

namespace RG.Unpacker
{
    class BinHashHeader
    {
        public UInt32 dwMagic { get; set; } // 0x68736168 (hash)
        public Int32 dwTotalBlocks { get; set; } // * 4
        public Int32 dwBlockSize { get; set; } // 1048576 bytes each block
        public Int32 dwHashBlockTableSize { get; set; } // Hash table + Hash header size
        public Int64 dwHashBlockTableOffset { get; set; }
        public Int32 dwFlag { get; set; } // 0
        public UInt32 dwHash { get; set; } // Hash of this header (look at BinHash.iGetHashHeaderHash function)
    }
}
