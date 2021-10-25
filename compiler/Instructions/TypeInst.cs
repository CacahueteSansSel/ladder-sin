using compiler.Core;
using compiler.Emit;
using Compiler.Parsing;

namespace compiler.Instructions
{
    public class TypeInst : InstructionBase
    {
        public override string Name => "type";
        public override byte Opcode => 0x07;
        
        public override void Emit(CompilerEnvironment env, ParserToken srcToken)
        {
            ParserToken text = srcToken.Childs[0];
            if (text.Type != TokenType.ValueString)
            {
                CLI.Error("ladderc", $"{Name}: expected a string as an argument");
                return;
            }

            env.StreamWriter.Write(text.Text);
        }
    }
}