using System;

namespace RG.Unpacker
{
    class BinHash
    {
        public static UInt32 iGetHash(String m_String)
        {
            UInt32 dwHash = 0;

            for (Int32 i = 0; i < m_String.Length; i++)
            {
                dwHash = (Byte)(m_String[i] & 0xDF) + 131 * dwHash;
            }

            return (Byte)(m_String.Length & 0xF) | (16 * (dwHash & 0x7FFFFFF));
        }

        public static UInt32 iGetBlockHash(Byte[] lpBuffer)
        {
            Int32 dwOffset = 0;
            Int32 dwBlocks = (lpBuffer.Length) >> 2;

            UInt32 dwHash = 0;

            for (Int32 i = 0; i < dwBlocks; i++, dwOffset += 4)
            {
                UInt32 dwData = BitConverter.ToUInt32(lpBuffer, dwOffset) & 0xDFDFDFDF;
                dwHash = dwData + 131 * dwHash;
            }

            return (Byte)(dwBlocks & 0xF) | (16 * (dwHash & 0x7FFFFFF));
        }

        public static UInt32 iGetHashHeaderHash(Byte[] lpBuffer)
        {
            UInt32 dwHash = 0;

            UInt32 dwData = BitConverter.ToUInt32(lpBuffer, 8);
            dwData &= 0xDFDFDFDF;
            dwHash += dwData;
            dwHash *= 131;

            dwData = BitConverter.ToUInt32(lpBuffer, 12);
            dwData &= 0xDFDFDFDF;
            dwHash += dwData;
            dwHash *= 131;

            dwData = BitConverter.ToUInt32(lpBuffer, 16);
            dwData &= 0xDFDFDFDF;
            dwHash += dwData;
            dwHash *= 131;

            dwData = BitConverter.ToUInt32(lpBuffer, 20);
            dwData &= 0xDFDFDFDF;
            dwHash += dwData;
            dwHash *= 131;

            dwData = BitConverter.ToUInt32(lpBuffer, 0);
            dwData &= 0xDFDFDFDF;
            dwData *= 0x4B4D6427;
            dwHash -= dwData;

            dwData = BitConverter.ToUInt32(lpBuffer, 4);
            dwData &= 0xDFDFDFDF;
            dwData *= 0x47BB48D;
            dwHash -= dwData;

            dwData = BitConverter.ToUInt32(lpBuffer, 24);
            dwData &= 0xDFDFDFDF;
            dwHash += dwData;

            return (dwHash & 0x7FFFFFF) << 4 | 7;
        }
    }
}
