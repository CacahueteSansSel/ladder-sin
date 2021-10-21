using System;
using compiler.Emit;
using Compiler.Parsing;

namespace compiler.Instructions
{
    public abstract class InstructionBase {
        public abstract string Name { get; }
        public abstract byte Opcode { get; }
        public abstract void Emit(CompilerEnvironment env, ParserToken srcToken);
    }
}