using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Log
    {
        private static object lockObj = "";

        private static void WriteLine(ConsoleColor _c, string _prefix, string _text) {
            lock (lockObj) {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("[");
                Console.ForegroundColor = _c;
                Console.Write(DateTime.Now.ToShortTimeString());
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] [");
                Console.ForegroundColor = _c;
                Console.Write(_prefix);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] :: ");
                Console.ForegroundColor = _c;
                Console.WriteLine(_text);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void WritePlain(string _prefix, string _text)
        {
            WriteLine(ConsoleColor.Yellow, _prefix, _text);
        }

        public static void WriteError(string _text)
        {
            WriteLine(ConsoleColor.Red, "ERROR", _text);
        }

        public static void WriteNetwork(string _text)
        {
            WriteLine(ConsoleColor.Cyan, "NETWORK", _text);
        }

        public static void WriteDebug(string _text)
        {
            WriteLine(ConsoleColor.Magenta, "DEBUG", _text);
        }
    }
}
