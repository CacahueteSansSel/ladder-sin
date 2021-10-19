using System;

namespace Compiler.Parsing
{
    public class StringParser {
        string value;
        public int Position {get;set;}

        public StringParser(string value)
        {
            this.value = value;
        }

        public string ConsumeTo(char c) {
            string buf = "";

            while (Position < value.Length) {
                if (buf[Position] == c) {
                    Position++; // Ignore that char
                    return buf;
                } else buf += c;
                Position++;
            }

            return buf;
        }

        public char Peek(int offset = 0)
            => value[Position + offset];
    }
}
