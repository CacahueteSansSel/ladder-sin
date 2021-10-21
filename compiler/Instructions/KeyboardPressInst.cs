using System;
using compiler.Core;
using compiler.Emit;
using Compiler.Parsing;

namespace compiler.Instruction 
{
    public class KeyboardPressInst : InstructionBase 
    {
        public override byte Opcode => 0x01;
        public override string Name => "keyboard_press";

        public KeyboardPressInst()
        {

        }

        public override void Emit(CompilerEnvironment env, ParserToken srcToken)
        {
            // Write the keycode
            if (srcToken.Childs[0].Type != TokenType.ValueKeycode) 
            {
                CLI.Error("ladderc", $"{Name}: expected keycode as argument");
                return;
            }
            
            // Fetch the keycode from his name, and write it
            byte keycode = Keycodes.Get(srcToken.Childs[0].Text);

            env.StreamWriter.Write(keycode);
        }
    }
}