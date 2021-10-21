using System;
using compiler.Core;
using compiler.Emit;
using Compiler.Parsing;

namespace compiler.Instruction
{
    public class ExitInstruction : InstructionBase
    {
        public override byte Opcode => 0x07;
        public override string Name => "exit";

        public ExitInstruction()
        {

        }

        public override void Emit(CompilerEnvironment env, ParserToken srcToken)
        {
            // Writes nothing
        }
    }
}