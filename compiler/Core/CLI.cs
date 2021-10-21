using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compiler.Core
{
    public static class CLI
    {
        public static void Error(string origin, string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{origin}: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"fatal error: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);

            Environment.Exit(1);
        }

        public static void Warning(string origin, string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{origin}: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"fatal error: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);
        }
    }
}
