using Compiler.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compiler.Emit
{
    public class CompilerEnvironment
    {
        public List<ParserToken> Tokens { get; } = new List<ParserToken>();

        public CompilerEnvironment()
        {

        }

        public void Include(params ParserToken[] tokens)
            => Tokens.AddRange(tokens);

        public void Dump()
        {
            foreach (ParserToken tok in Tokens)
                PrintToken(0, tok);
        }

        static void PrintToken(int indent, ParserToken token)
        {
            for (int i = 0; i < indent; i++) Console.Write("  ");
            Console.WriteLine($"[{token.Type}] {token.Text}");
            foreach (ParserToken child in token.Childs)
            {
                PrintToken(indent + 1, child);
            }
        }
    }
}
