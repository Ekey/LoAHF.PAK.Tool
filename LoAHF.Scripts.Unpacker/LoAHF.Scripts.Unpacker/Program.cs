using System;
using System.IO;

namespace LoAHF.Scripts.Unpacker
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("League of Angle Heaven's Fury Scripts Unpacker");
            Console.WriteLine("(c) 2021 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    LoAHF.Scripts.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of cache scripts archive file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    LoAHF.Scripts.Unpacker E:\\Games\\LoAHF\\client_script_list.txt D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_CacheFile = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_CacheFile))
            {
                Utils.iSetError("[ERROR]: Input cache file -> " + m_CacheFile + " <- does not exist");
                return;
            }

            ScriptsUnpack.iDoIt(m_CacheFile, m_Output);
        }
    }
}
