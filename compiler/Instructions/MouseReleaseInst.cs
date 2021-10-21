using compiler.Emit;
using Compiler.Parsing;

namespace compiler.Instructions
{
    public class MouseReleaseInst : InstructionBase
    {
        public override string Name => "mouse_release";
        public override byte Opcode => 0x04;

        public MouseReleaseInst()
        {
            
        }
        
        public override void Emit(CompilerEnvironment env, ParserToken srcToken)
        {
            // Ignore shit
        }
    }
}