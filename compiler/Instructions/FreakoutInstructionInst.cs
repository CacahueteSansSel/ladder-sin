using compiler.Emit;
using Compiler.Parsing;

namespace compiler.Instructions
{
    public class FreakoutInstructionInst : InstructionBase
    {
        public override string Name => "freakout";
        public override byte Opcode => 0x0B;
        
        public override void Emit(CompilerEnvironment env, ParserToken srcToken)
        {
            // Nothing here
        }
    }
}