using compiler.Core;
using compiler.Emit;
using Compiler.Parsing;

namespace compiler.Instructions
{
    public class MouseMoveInst : InstructionBase
    {
        public override string Name => "mouse_move";
        public override byte Opcode => 0x06;

        public MouseMoveInst()
        {
            
        }
        
        public override void Emit(CompilerEnvironment env, ParserToken srcToken)
        {
            ParserToken moveDX = srcToken.Childs[0];
            ParserToken moveDY = srcToken.Childs[1];

            if (moveDX.Type != TokenType.ValueNumeric || moveDY.Type != TokenType.ValueNumeric)
            {
                CLI.Error("ladderc", $"{Name}: expected two integers as arguments");
                return;
            }
            
            int dx = int.Parse(moveDX.Text);
            int dy = int.Parse(moveDY.Text);
            
            if (dx > 512 || dx < -512) CLI.Error("ladderc", $"{Name}: first argument must be between -512 and 512");
            if (dy > 512 || dy < -512) CLI.Error("ladderc", $"{Name}: second argument must be between -512 and 512");

            env.StreamWriter.Write((ushort)(512 + dx));
            env.StreamWriter.Write((ushort)(512 + dy));
        }
    }
}