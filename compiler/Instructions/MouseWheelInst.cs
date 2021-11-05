using compiler.Core;
using compiler.Emit;
using Compiler.Parsing;

namespace compiler.Instructions
{
    public class MouseWheelInst : InstructionBase
    {
        public override string Name => "mouse_wheel";
        public override byte Opcode => 0x07;
        
        public override void Emit(CompilerEnvironment env, ParserToken srcToken)
        {
            // Write the keycode
            if (srcToken.Childs[0].Type != TokenType.ValueNumeric)
            {
                CLI.Error("ladderc", $"{Name}: expected integer as argument");
                return;
            }
            
            int delta = int.Parse(srcToken.Childs[0].Text);
            
            if (delta > 512 || delta < -512) CLI.Error("ladderc", $"{Name}: argument must be between -512 and 512");

            env.StreamWriter.Write((ushort)(512 + delta));
        }
    }
}