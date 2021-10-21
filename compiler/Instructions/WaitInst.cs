using System;
using compiler.Core;
using compiler.Emit;
using Compiler.Parsing;

namespace compiler.Instructions
{
    public class WaitInstruction : InstructionBase
    {
        public override byte Opcode => 0x06;
        public override string Name => "wait";

        public WaitInstruction()
        {

        }

        public override void Emit(CompilerEnvironment env, ParserToken srcToken)
        {
            // Write the keycode
            if (srcToken.Childs[0].Type != TokenType.ValueNumeric)
            {
                CLI.Error("ladderc", $"{Name}: expected integer as argument");
                return;
            }
            
            int delayMs = int.Parse(srcToken.Childs[0].Text);
            if (delayMs <= 0)
            {
                CLI.Error("ladderc", $"{Name}: passed invalid delay: negative or equal to zero");
                return;
            }

            env.StreamWriter.Write((uint)delayMs);
        }
    }
}