using System;

namespace RG.Unpacker
{
    class BinCipher
    {
        public static Byte[] iDecryptData(Byte[] lpBuffer, UInt32 dwKey = 0xFABACEDA, UInt32 dwOffset = 0)
        {
            Int32 dwBlocks = lpBuffer.Length >> 2;

            UInt32 dwTempA = (dwOffset + 1) * 0x100;
            UInt32 dwTempB = (dwOffset + 2) * 0x10000;
            UInt32 dwTempC = (dwOffset + 3) * 0x1000000;

            for (Int32 i = 0; i < dwBlocks; i++)
            {
                UInt32 dwData = BitConverter.ToUInt32(lpBuffer, (Int32)dwOffset);

                dwData ^= (dwTempB & 0xFF0000 | (dwTempA & 0xFF00) | dwTempC | (dwOffset & 0xFF)) ^ dwKey;

                lpBuffer[dwOffset + 0] = (Byte)dwData;
                lpBuffer[dwOffset + 1] = (Byte)(dwData >> 8);
                lpBuffer[dwOffset + 2] = (Byte)(dwData >> 16);
                lpBuffer[dwOffset + 3] = (Byte)(dwData >> 24);

                dwTempA += 0x400;
                dwTempB += 0x40000;
                dwTempC += 0x4000000;

                dwOffset += 4;
            }

            dwBlocks = lpBuffer.Length & 3;

            if (dwBlocks != 0)
            {
                for (Int32 i = 0; i < dwBlocks; i++)
                {
                    dwTempA = dwOffset & 3;

                    lpBuffer[i] ^= (Byte)(dwKey + dwTempA);
                    lpBuffer[i] ^= (Byte)(dwOffset);

                    dwOffset += 1;
                }
            }

            return lpBuffer;
        }
    }
}
