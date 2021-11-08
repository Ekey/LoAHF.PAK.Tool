using System;
using System.IO;

namespace LoAHF.Unpacker
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("League of Angle Heaven's Fury PAK Unpacker");
            Console.WriteLine("(c) 2021 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    LoAHF.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of PAK archive file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    LoAHF.Unpacker E:\\Games\\LoAHF\\data_1.pak D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_PakFile = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_PakFile))
            {
                Utils.iSetError("[ERROR]: Input PAK file -> " + m_PakFile + " <- does not exist");
                return;
            }

            PakUnpack.iDoIt(m_PakFile, m_Output);
        }
    }
}
