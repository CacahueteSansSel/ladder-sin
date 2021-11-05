using compiler.Core;
using compiler.Instructions;
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
        
        List<ParserToken> macros = new List<ParserToken>();
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

        void ProcessToken(ParserToken token)
        {
            if (token.Type == TokenType.DefinitionMacro)
            {
                macros.Add(token);
                return;
            }
            if (token.Type != TokenType.InstructionCall) 
            {
                CLI.Error("ladderc", $"found non-instruction token in root tokens");
                return;
            }

            var macro = macros.FirstOrDefault(tok => tok.Text == token.Text);
            if (macro != null)
            {
                // We call a macro
                // We need to replace each ValueArgument-typed tokens to their appropriate argument
                // in the tokens' childs
                List<ParserToken> args = token.Childs;
                string[] argsNames = macro.Array;
                foreach (ParserToken macroToken in macro.Childs)
                {
                    ParserToken newToken = new ParserToken(macroToken);
                    
                    if (newToken.Contains(TokenType.ValueArgument))
                    {
                        for (int i = 0; i < newToken.Childs.Count; i++)
                        {
                            if (newToken.Childs[i].Type != TokenType.ValueArgument) continue;

                            int idx = argsNames.IndexOf(newToken.Childs[i]
                                .Text); // Retrieving the index of the passed argument
                            newToken.Childs[i] = args[idx]; // Injecting the passed argument in the macro call
                        }
                    }

                    ProcessToken(newToken);
                }

                return;
            }
            
            CompileInstruction(token);
        }
        
        public void Compile()
        {
            foreach (ParserToken token in Tokens) 
                ProcessToken(token);
        }
        

        void CompileInstruction(ParserToken token)
        {
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
