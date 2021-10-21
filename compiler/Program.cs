using System;
using System.IO;
using System.Linq;
using compiler.Core;
using compiler.Emit;
using compiler.Instruction;
using Compiler.Parsing;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Gray;

            ArgumentReader argReader = new(args);
            string[] inputFiles = argReader.GetAloneArguments();
            string outputFile = argReader.NextTo("-o");

            if (inputFiles.Length == 0)
            {
                CLI.Error("ladderc", "no input files");
                return;
            }

            if (outputFile == null)
            {
                CLI.Error("ladderc", "no output file specified");
                return;
            }

            // Load all instructions
            FileStream outputFileStrm = File.OpenWrite(outputFile);
            InstructionLoader.Load();
            CompilerEnvironment env = new(outputFileStrm);
            Parser parser = new();
            foreach (string file in inputFiles)
            {
                try
                {
                    string fileRaw = File.ReadAllText(file);
                    env.Include(parser.Parse(fileRaw));
                } 
                catch (Exception e)
                {
                    CLI.Error("ladderc", $"{file}: {e.Message}");
                }
            }
            
            outputFileStrm.Dispose();
            env.Dump();
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
