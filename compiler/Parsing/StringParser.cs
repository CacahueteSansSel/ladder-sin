using System;

namespace Compiler.Parsing
{
    public class StringParser 
    {
        string value;
        public int Position { get; set; }

        public delegate bool CharacterProcessor(char c);

        public StringParser(string value)
        {
            this.value = value;
        }

        public string ConsumeTo(CharacterProcessor proc)
        {
            string buf = "";

            while (Position < value.Length)
            {
                char curChar = value[Position];

                if (proc(curChar))
                {
                    Position++; // Ignore that char
                    return buf;
                }
                else buf += curChar;
                Position++;
            }

            return buf;
        }

        public string ConsumeTo(char c) 
        {
            string buf = "";

            while (Position < value.Length)
            {
                char curChar = value[Position];

                if (curChar == c)
                {
                    Position++; // Ignore that char
                    return buf;
                }
                else buf += curChar;
                Position++;
            }

            return buf;
        }

        public char Consume()
        {
            char c = value[Position];
            Position++;

            return c;
        }

        public string ReadToEnd()
            => value.Substring(Position);

        public char Peek(int offset = 0)
            => value[Position + offset];

        public bool Expect(char c)
            => Peek(0) == c;

        public bool ConsumeExpect(char c)
            => Consume() == c;

        public bool ExpectNumber()
            => char.IsDigit(Peek(0));

        public bool ExpectAlphanumeric()
            => char.IsLetterOrDigit(Peek(0));
    }
}
