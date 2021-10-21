using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parsing 
{
    public class Parser 
    {
        string[] SplitIntoLines(string rawText)
            => rawText.Split('\n');

        /// This method is used to parse .mck files into tokens
        public ParserToken[] Parse(string rawFileText) 
        {
            string[] lines = SplitIntoLines(rawFileText);
            List<ParserToken> tokenList = new();

            foreach (string line in lines) 
            {
                if (line.StartsWith("//") || line.StartsWith("#")) continue;
                string[] lineToks = line.Replace("\r", "").Replace("\n", "").SplitQuotes(' ');
                string instName = lineToks[0];
                ParserToken instToken = new();
                instToken.Text = instName;
                instToken.Type = TokenType.InstructionCall;

                foreach (string child in lineToks.Skip(1)) 
                {
                    // Add the token into the instruction's token
                    instToken.Childs.Add(new(new(child)));
                }
                tokenList.Add(instToken); // Add the instruction's token into the main list
            }

            return tokenList.ToArray();
        }
    }

    public class ParserToken 
    {
        public string Text { get; set; }
        public TokenType Type { get; set; }

        public List<ParserToken> Childs { get; } = new();

        public ParserToken()
        {
            
        }

        public ParserToken(StringParser parser)
        {
            // Detect the token type
            if (parser.Expect('\''))
            {
                parser.Consume();
                Text = parser.ConsumeTo('\'');
                Type = TokenType.ValueCharacter;
            } else if (parser.Expect('\"'))
            {
                parser.Consume();
                Text = parser.ConsumeTo('\"');
                Type = TokenType.ValueString;
            } else if (parser.Expect('['))
            {
                parser.Consume();
                Text = parser.ConsumeTo(']');
                Type = TokenType.ValueKeycode;
            }
            else if (parser.ExpectAlphanumeric())
            {
                Text = parser.ConsumeTo((c) => !char.IsDigit(c));
                Type = TokenType.ValueNumeric;
            } else
            {
                Text = parser.ReadToEnd();
                Type = TokenType.ValueUnknown;
            }
        }
    }

    public enum TokenType 
    {
        InstructionCall,
        ValueNumeric,
        ValueCharacter,
        ValueKeycode,
        ValueString,
        ValueUnknown
    }
}