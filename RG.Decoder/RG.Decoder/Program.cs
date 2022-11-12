using System;
using System.IO;

namespace RG.Decoder
{
    class Program
    {
        private static String m_Title = "Ray Gigant DAT Decoder";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2022 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    RG.Scripts.Decoder <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of DAT file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    RG.Scripts.Decoder E:\\Games\\RG\\data\\scripts.dat D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_DatFile = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_DatFile))
            {
                Utils.iSetError("[ERROR]: Input DAT file -> " + m_DatFile + " <- does not exist");
                return;
            }

            DatUnpack.iDoIt(m_DatFile, m_Output);
        }
    }
}
