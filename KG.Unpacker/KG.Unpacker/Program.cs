using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KG.Unpacker
{
    class Program
    {
        private static String m_Title = "Keen Games KFC_DIR / KFC_DATA Unpacker";

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
                Console.WriteLine("    KG.Unpacker <m_Kfc_File> <m_OutDirectory>");
                Console.WriteLine("    m_Kfc_File - Source of kfc_dir file");
                Console.WriteLine("    m_OutDirectory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    KG.Unpacker E:\\Games\\Enshrouded\\enshrouded.kfc_dir D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_Input = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            KfcUnpack.iDoIt(m_Input, m_Output);
        }
    }
}
