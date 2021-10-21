using compiler.Emit;
using Compiler.Parsing;

namespace compiler.Instructions
{
    public class KeyboardReleaseInst : InstructionBase
    {
        public override string Name => "keyboard_release";
        public override byte Opcode => 0x02;

        public KeyboardReleaseInst()
        {
            
        }

        public override void Emit(CompilerEnvironment env, ParserToken srcToken)
        {
            // Ignore
        }
    }
}