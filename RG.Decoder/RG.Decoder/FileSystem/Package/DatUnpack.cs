using System;
using System.IO;

namespace RG.Decoder
{
    class DatUnpack
    {
        public static void iDoIt(String m_File, String m_DstFolder)
        {
            using (FileStream TFileStream = File.OpenRead(m_File))
            {
                Int32 dwBlockNum = 0;
                do
                {
                    UInt16 wBlockID = TFileStream.ReadUInt16();
                    UInt16 wBlockCMD = TFileStream.ReadUInt16();

                    UInt32 dwEncodedSize = TFileStream.ReadUInt32();
                    dwEncodedSize = (dwEncodedSize + 3) & 0xFFFFFFFC;

                    var lpSrcBlock = TFileStream.ReadBytes((Int32)dwEncodedSize);
                    var lpDstBlock = DatDecoder.iDecodeData(lpSrcBlock);

                    String m_FileName = String.Format("Out\\Block_{0}.dat", dwBlockNum++);

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FileName);

                    File.WriteAllBytes(m_FileName, lpDstBlock);
                }
                while (TFileStream.Position != TFileStream.Length);

                TFileStream.Dispose();
            }
        }
    }
}
