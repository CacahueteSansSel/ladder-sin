using compiler.Core;
using compiler.Emit;
using Compiler.Parsing;

namespace compiler.Instructions
{
    public class MouseMoveInst : InstructionBase
    {
        public override string Name => "mouse_move";
        public override byte Opcode => 0x05;

        public MouseMoveInst()
        {
            
        }
        
        public override void Emit(CompilerEnvironment env, ParserToken srcToken)
        {
            ParserToken moveDX = srcToken.Childs[0];
            ParserToken moveDY = srcToken.Childs[1];

            if (moveDX.Type != TokenType.ValueNumeric || moveDX.Type != TokenType.ValueNumeric)
            {
                CLI.Error("ladderc", $"{Name}: expected two integers as arguments");
                return;
            }

            int dx = int.Parse(moveDX.Text);
            int dy = int.Parse(moveDY.Text);
            env.StreamWriter.Write(dx);
            env.StreamWriter.Write(dy);
        }
    }
}