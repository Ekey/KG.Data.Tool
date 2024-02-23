using System;

namespace KG.Packer
{
    class Program
    {
        private static String m_Title = "Keen Games KFC_DIR / KFC_DATA Packer";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2024 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    KG.Packer <m_Input_Directory> <m_Output_File>");
                Console.WriteLine("    m_Input_Directory - Source of directory");
                Console.WriteLine("    m_Output_File - Destination of archive name file (without extension)\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    KG.Packer D:\\Unpacked E:\\Games\\Enshrouded\\super_archive");
                Console.ResetColor();
                return;
            }

            String m_Input = args[0];
            String m_Output = args[1];

            KfcPack.iDoIt(m_Input, m_Output);
        }
    }
}
