﻿using System;
using System.IO;

namespace RG.Decoder
{
    class DatDecoder
    {
        public static Byte[] m_EncodingTable = new Byte[] {
            0x33, 0xFC, 0x27, 0xFE, 0xA1, 0xB1, 0x47, 0x22, 0xED, 0xE7, 0x99, 0x8E, 0xBE, 0x77, 0x24, 0x82,
            0x64, 0x40, 0xD7, 0x7F, 0x6B, 0x6E, 0xC0, 0xAC, 0x70, 0x78, 0x34, 0xF9, 0x81, 0xEF, 0x05, 0x51,
            0xB5, 0xA7, 0x14, 0xB6, 0xBB, 0xB3, 0x83, 0x16, 0xC5, 0x37, 0x56, 0xC9, 0x30, 0xC8, 0x98, 0xCD,
            0xEE, 0x3B, 0x72, 0xE6, 0xF4, 0x79, 0x08, 0xAA, 0xDC, 0x52, 0x23, 0x45, 0x92, 0x3D, 0x74, 0x5A,
            0x7E, 0x3F, 0x63, 0xF1, 0xCB, 0x60, 0xD4, 0x6D, 0xF2, 0x2E, 0xD8, 0x8D, 0xC3, 0xD0, 0x89, 0xF3,
            0x66, 0xA6, 0x10, 0x87, 0x90, 0xE4, 0x2C, 0x21, 0xE0, 0x38, 0x25, 0x76, 0x12, 0x2A, 0xAD, 0xA0,
            0x43, 0x49, 0x69, 0x9C, 0xFA, 0x1D, 0x55, 0xAE, 0x7D, 0x46, 0xEB, 0x6C, 0x48, 0x88, 0x4D, 0xFF,
            0xCE, 0xD2, 0x02, 0xA2, 0xEA, 0x80, 0x57, 0xAF, 0xF0, 0x1F, 0x3A, 0x8C, 0xB2, 0x9F, 0x4B, 0x20,
            0x41, 0x0A, 0x9A, 0x53, 0xE2, 0x62, 0xCA, 0x07, 0xD9, 0xCC, 0xFB, 0xDA, 0xE9, 0xA9, 0x96, 0x03,
            0x95, 0xB8, 0x0C, 0x36, 0x35, 0xF6, 0x6A, 0x18, 0x5B, 0x5C, 0xB7, 0xA4, 0x75, 0x2B, 0xBA, 0xE1,
            0x61, 0x4A, 0x50, 0xC1, 0xA8, 0xDF, 0xC6, 0x8B, 0x71, 0x7C, 0x2F, 0x4F, 0x93, 0x73, 0x8A, 0x0F,
            0x1E, 0x94, 0xB0, 0xAB, 0x44, 0x9B, 0x65, 0x26, 0x1C, 0xF8, 0x67, 0x9E, 0x15, 0x3E, 0xBD, 0xD3,
            0xDD, 0xB9, 0x19, 0x91, 0x13, 0x84, 0x6F, 0x7B, 0xD1, 0x06, 0xCF, 0xFD, 0x1B, 0x58, 0x0E, 0x86,
            0xC2, 0x4E, 0xD6, 0x0D, 0x09, 0x97, 0x39, 0x7A, 0x29, 0x28, 0xF7, 0x85, 0x00, 0x32, 0x42, 0x2D,
            0x31, 0xF5, 0xE8, 0x8F, 0xDB, 0x5D, 0x01, 0x5F, 0xC4, 0x5E, 0xD5, 0x54, 0xE3, 0x1A, 0xC7, 0x9D,
            0xB4, 0xBF, 0x0B, 0xE5, 0x04, 0xBC, 0x11, 0xDE, 0xA3, 0x59, 0x4C, 0x17, 0x3C, 0x68, 0xA5, 0xEC,
            0xDC, 0xE6, 0x72, 0x8F, 0xF4, 0x1E, 0xC9, 0x87, 0x36, 0xD4, 0x81, 0xF2, 0x92, 0xD3, 0xCE, 0xAF,
            0x52, 0xF6, 0x5C, 0xC4, 0x22, 0xBC, 0x27, 0xFB, 0x97, 0xC2, 0xED, 0xCC, 0xB8, 0x65, 0xB0, 0x79,
            0x7F, 0x57, 0x07, 0x3A, 0x0E, 0x5A, 0xB7, 0x02, 0xD9, 0xD8, 0x5D, 0x9D, 0x56, 0xDF, 0x49, 0xAA,
            0x2C, 0xE0, 0xDD, 0x00, 0x1A, 0x94, 0x93, 0x29, 0x59, 0xD6, 0x7A, 0x31, 0xFC, 0x3D, 0xBD, 0x41,
            0x11, 0x80, 0xDE, 0x60, 0xB4, 0x3B, 0x69, 0x06, 0x6C, 0x61, 0xA1, 0x7E, 0xFA, 0x6E, 0xD1, 0xAB,
            0xA2, 0x1F, 0x39, 0x83, 0xEB, 0x66, 0x2A, 0x76, 0xCD, 0xF9, 0x3F, 0x98, 0x99, 0xE5, 0xE9, 0xE7,
            0x45, 0xA0, 0x85, 0x42, 0x10, 0xB6, 0x50, 0xBA, 0xFD, 0x62, 0x96, 0x14, 0x6B, 0x47, 0x15, 0xC6,
            0x18, 0xA8, 0x32, 0xAD, 0x3E, 0x9C, 0x5B, 0x0D, 0x19, 0x35, 0xD7, 0xC7, 0xA9, 0x68, 0x40, 0x13,
            0x75, 0x1C, 0x0F, 0x26, 0xC5, 0xDB, 0xCF, 0x53, 0x6D, 0x4E, 0xAE, 0xA7, 0x7B, 0x4B, 0x0B, 0xE3,
            0x54, 0xC3, 0x3C, 0xAC, 0xB1, 0x90, 0x8E, 0xD5, 0x2E, 0x0A, 0x82, 0xB5, 0x63, 0xEF, 0xBB, 0x7D,
            0x5F, 0x04, 0x73, 0xF8, 0x9B, 0xFE, 0x51, 0x21, 0xA4, 0x8D, 0x37, 0xB3, 0x17, 0x5E, 0x67, 0x77,
            0xB2, 0x05, 0x7C, 0x25, 0xF0, 0x20, 0x23, 0x9A, 0x91, 0xC1, 0x9E, 0x24, 0xF5, 0xBE, 0x0C, 0xF1,
            0x16, 0xA3, 0xD0, 0x4C, 0xE8, 0x28, 0xA6, 0xEE, 0x2D, 0x2B, 0x86, 0x44, 0x89, 0x2F, 0x70, 0xCA,
            0x4D, 0xC8, 0x71, 0xBF, 0x46, 0xEA, 0xD2, 0x12, 0x4A, 0x88, 0x8B, 0xE4, 0x38, 0xC0, 0xF7, 0xA5,
            0x58, 0x9F, 0x84, 0xEC, 0x55, 0xF3, 0x33, 0x09, 0xE2, 0x8C, 0x74, 0x6A, 0xFF, 0x08, 0x30, 0x1D,
            0x78, 0x43, 0x48, 0x4F, 0x34, 0xE1, 0x95, 0xDA, 0xB9, 0x1B, 0x64, 0x8A, 0x01, 0xCB, 0x03, 0x6F
        };

        public static UInt16 iDecodeUInt16(UInt16 wEncodedBytes, Byte bIndex)
        {
            Byte bByte1 = m_EncodingTable[(UInt32)(wEncodedBytes >> 8) + (UInt32)bIndex * 0x100];
            Byte bByte2 = m_EncodingTable[(Byte)wEncodedBytes + (UInt32)bIndex * 0x100];

            return (UInt16)(bByte2 + (bByte1 << 8));
        }

        public static Byte[] iDecodeData(Byte[] lpBuffer)
        {
            UInt32 dwOffset = 0;

            using (var TMemoryReader = new MemoryStream(lpBuffer))
            {
                Int32 dwDecodedSize = TMemoryReader.ReadInt32();
                Int32 dwBlocks = TMemoryReader.ReadInt32();

                Byte[] lpResult = new Byte[dwDecodedSize];

                if (dwBlocks <= 0)
                {
                    return lpBuffer;
                }

                Int32 i = 0;
                UInt16 wBlockParams = 0;

                while (i < dwBlocks)
                {
                    if (i % 16 == 0)
                    {
                        wBlockParams = TMemoryReader.ReadUInt16();
                    }

                    if (((1 << (i & 0xF)) & wBlockParams) != 0)
                    {
                        UInt16 wEncodedBytes = TMemoryReader.ReadUInt16();
                        UInt16 wDecodedBytes = (UInt16)((Int32)wEncodedBytes >> 7);
                        wDecodedBytes *= 2;

                        for (Int32 j = 0; j < (wEncodedBytes & 0x7F); ++j)
                        {
                            UInt16 wBackBytes = BitConverter.ToUInt16(lpResult, (Int32)dwOffset - wDecodedBytes);
                            lpResult[dwOffset + 0] = (Byte)wBackBytes;
                            lpResult[dwOffset + 1] = (Byte)(wBackBytes >> 8);

                            dwOffset += 2;
                        }
                    }
                    else
                    {
                        UInt16 wEncodedBytes = TMemoryReader.ReadUInt16();
                        UInt16 wDecodedBytes = iDecodeUInt16(wEncodedBytes, 1);

                        lpResult[dwOffset + 0] = (Byte)wDecodedBytes;
                        lpResult[dwOffset + 1] = (Byte)(wDecodedBytes >> 8);

                        dwOffset += 2;
                    }

                    ++i;
                }

                return lpResult;
            }
        }
    }
}