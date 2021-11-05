using compiler.Core;
using compiler.Emit;
using Compiler.Parsing;

namespace compiler.Instructions
{
    public class KeyboardReleaseAllInst : InstructionBase
    {
        public override string Name => "keyboard_release_all";
        public override byte Opcode => 0x03;

        public KeyboardReleaseAllInst()
        {
            
        }

        public override void Emit(CompilerEnvironment env, ParserToken srcToken)
        {
            // Ignore
        }
    }
}