using compiler.Core;
using compiler.Instruction;
using Compiler.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compiler.Emit
{
    public class CompilerEnvironment
    {
        Stream targetStream;
        BinaryWriter writer;
        public List<ParserToken> Tokens { get; } = new List<ParserToken>();

        public BinaryWriter StreamWriter => writer;

        public CompilerEnvironment(Stream targetStream)
        {
            this.targetStream = targetStream;
            writer = new BinaryWriter(this.targetStream);
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

        public void Compile() 
        {
            foreach (ParserToken token in Tokens) 
            {
                if (token.Type != TokenType.InstructionCall) 
                {
                    CLI.Error("ladderc", $"found non-instruction token in root tokens");
                    return;
                }

                InstructionBase inst = InstructionLoader.Get(token.Text);
                if (inst == null)
                {
                    CLI.Error("ladderc", $"instruction '{token.Text}' not found");
                    return;
                }
                
                writer.Write(inst.Opcode); // Writes the instruction's opcode
                inst.Emit(this, token);
            }
        }
    }
}
