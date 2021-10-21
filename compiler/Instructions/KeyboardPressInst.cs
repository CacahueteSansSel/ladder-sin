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
            if (srcToken.Childs[0].Type != TokenType.ValueCharacter && srcToken.Childs[0].Type != TokenType.ValueNumeric) 
            {
                CLI.Error("ladderc", $"{Name}: expected character as argument");
                return;
            }
            ushort character = char.Parse(srcToken.Childs[0].Text);

            env.StreamWriter.Write(character);
        }
    }
}