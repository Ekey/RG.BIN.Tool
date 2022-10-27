using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace RG.Unpacker
{
    class BinUnpack
    {
        static List<BinEntry> m_EntryTable = new List<BinEntry>();

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            BinHashList.iLoadProject();
            using (FileStream TBinStream = File.OpenRead(m_Archive))
            {
                var lpArchiveHeader = TBinStream.ReadBytes(32);
                lpArchiveHeader = BinCipher.iDecryptData(lpArchiveHeader);

                var m_Header = new BinHeader();
                using (var THeaderReader = new MemoryStream(lpArchiveHeader))
                {
                    m_Header.dwMagic = THeaderReader.ReadUInt32();
                    m_Header.dwHeaderSize = THeaderReader.ReadInt32();
                    m_Header.dwEntryDataOffset = THeaderReader.ReadInt32();
                    m_Header.dwTableCompressedSize = THeaderReader.ReadInt64();
                    m_Header.dwTableDecompressedSize = THeaderReader.ReadInt64();
                    m_Header.dwFlag = THeaderReader.ReadInt32();

                    THeaderReader.Dispose();
                }

                if (m_Header.dwMagic != 0x2E6E6962)
                {
                    Utils.iSetError("[ERROR]: Invalid magic of BIN archive file!");
                    return;
                }

                TBinStream.Seek(-32, SeekOrigin.End);

                var lpHashHeader = TBinStream.ReadBytes(32);
                lpHashHeader = BinCipher.iDecryptData(lpHashHeader);

                var m_HashHeader = new BinHashHeader();
                using (var THashHeaderReader = new MemoryStream(lpHashHeader))
                {
                    m_HashHeader.dwMagic = THashHeaderReader.ReadUInt32();
                    m_HashHeader.dwTotalBlocks = THashHeaderReader.ReadInt32();
                    m_HashHeader.dwBlockSize = THashHeaderReader.ReadInt32();
                    m_HashHeader.dwHashBlockTableSize = THashHeaderReader.ReadInt32();
                    m_HashHeader.dwHashBlockTableOffset = THashHeaderReader.ReadInt64();
                    m_HashHeader.dwFlag = THashHeaderReader.ReadInt32();
                    m_HashHeader.dwHash = THashHeaderReader.ReadUInt32();

                    THashHeaderReader.Dispose();
                }

                if (m_HashHeader.dwMagic != 0x68736168)
                {
                    Utils.iSetError("[ERROR]: Invalid magic of HASH table!");
                    return;
                }

                if (BinHash.iGetHashHeaderHash(lpHashHeader) != m_HashHeader.dwHash)
                {
                    Utils.iSetError("[ERROR]: Invalid crc of HASH table!");
                    return;
                }

                TBinStream.Seek(0, SeekOrigin.Begin);

                var lpEntryTable = TBinStream.ReadBytes((Int32)m_Header.dwTableDecompressedSize);
                lpEntryTable = BinCipher.iDecryptData(lpEntryTable);

                m_EntryTable.Clear();
                using (var TEntryReader = new MemoryStream(lpEntryTable))
                {
                    TEntryReader.Seek(m_Header.dwEntryDataOffset, SeekOrigin.Begin);

                    Int32 dwHashNamesTableSize = TEntryReader.ReadInt32();
                    Int32 dwTotalFiles = TEntryReader.ReadInt32();

                    TEntryReader.Seek(dwHashNamesTableSize + m_Header.dwEntryDataOffset, SeekOrigin.Begin);

                    for (Int32 i = 0; i < dwTotalFiles; i++)
                    {
                        var m_Entry = new BinEntry();

                        m_Entry.dwOffset = TEntryReader.ReadInt64();
                        m_Entry.dwOffset += m_Header.dwTableDecompressedSize;
                        m_Entry.dwHashName = TEntryReader.ReadUInt32();
                        m_Entry.dwDecompressedSize = TEntryReader.ReadInt32();
                        m_Entry.dwCompressedSize = TEntryReader.ReadInt32();
                        m_Entry.dwFileID = TEntryReader.ReadInt32();
                        m_Entry.wUnknown1 = TEntryReader.ReadUInt16();
                        m_Entry.wUnknown2 = TEntryReader.ReadUInt16();
                        m_Entry.dwFlag = TEntryReader.ReadInt32();

                        m_EntryTable.Add(m_Entry);
                    }

                    TEntryReader.Dispose();

                    foreach (var m_Entry in m_EntryTable)
                    {
                        String m_FileName = BinHashList.iGetNameFromHashList(m_Entry.dwHashName);
                        String m_FullPath = m_DstFolder + m_FileName;

                        Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                        Utils.iCreateDirectory(m_FullPath);

                        TBinStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
                        var lpBuffer = TBinStream.ReadBytes(m_Entry.dwDecompressedSize);
                        lpBuffer = BinCipher.iDecryptData(lpBuffer, m_Entry.dwHashName);

                        File.WriteAllBytes(m_FullPath, lpBuffer);
                    }

                    TBinStream.Dispose();
                }
            }
        }
    }
}
