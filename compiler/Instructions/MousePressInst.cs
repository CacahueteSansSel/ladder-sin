using compiler.Core;
using compiler.Emit;
using Compiler.Parsing;

namespace compiler.Instructions
{
    public class MousePressInst : InstructionBase
    {
        public override string Name => "mouse_press";
        public override byte Opcode => 0x03;

        public MousePressInst()
        {
            
        }

        public override void Emit(CompilerEnvironment env, ParserToken srcToken)
        {
            if (srcToken.Childs[0].Type != TokenType.ValueNumeric)
            {
                CLI.Error("ladderc", $"{Name}: expected integer as argument");
                return;
            }

            byte mouseBtn = byte.Parse(srcToken.Childs[0].Text);
            env.StreamWriter.Write(mouseBtn);
        }
    }
}