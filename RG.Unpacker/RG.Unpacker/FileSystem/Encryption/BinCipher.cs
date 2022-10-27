using System;
using System.IO;

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

            Int32 dwRemainedSize = lpBuffer.Length & 3;

            if (dwRemainedSize != 0)
            {
                Byte[] lpKey = BitConverter.GetBytes(dwKey);

                for (Int32 i = 0; i < dwRemainedSize; i++)
                {
                    dwTempA = dwOffset & 3;

                    lpBuffer[dwOffset] ^= lpKey[i];
                    lpBuffer[dwOffset] ^= (Byte)(dwOffset);

                    ++dwOffset;
                }
            }

            return lpBuffer;
        }
    }
}
