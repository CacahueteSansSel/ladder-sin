using System;
using System.IO;
using Compiler.Parsing;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileContents = File.ReadAllText("Examples/test.nu");
            Parser parser = new();
            ParserToken[] tokens = parser.Parse(fileContents);
            foreach (ParserToken token in tokens)
            {
                PrintToken(0, token);
            }
        }

        static void PrintToken(int indent, ParserToken token) 
        {
            for (int i = 0; i < indent; i++) Console.Write(" ");
            Console.WriteLine($"[{token.Type}] {token.Text}");
            foreach (ParserToken child in token.Childs) 
            {
                PrintToken(indent+1, child);
            }
        }
    }
}
