using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parsing {
    public class Parser {
        string[] SplitIntoLines(string rawText)
            => rawText.Split('\n');

        /// This method is used to parse .mck files into tokens
        public ParserToken[] Parse(string rawFileText) {
            string[] lines = SplitIntoLines(rawFileText);
            List<ParserToken> tokens = new List<ParserToken>();

            foreach (string line in lines) {
                if (line.StartsWith("//") || line.StartsWith("#")) continue;
                string[] lineToks = line.SplitQuotes(' ');
                string instName = lineToks[0];
                ParserToken instToken = new ParserToken();
                instToken.Text = instName;
                instToken.Type = TokenType.MethodCall;

                foreach (string child in lineToks.Skip(1)) {
                    // Add the token into the instruction's token
                    instToken.Childs.Add(new ParserToken(new StringParser(child)));
                }
                tokens.Add(instToken); // Add the instruction's token into the main list
            }

            return tokens.ToArray();
        }
    }

    public class ParserToken {
        public string Text {get;set;}
        public TokenType Type {get;set;}

        public List<ParserToken> Childs {get;} = new List<ParserToken>();

        public ParserToken()
        {
            
        }

        public ParserToken(StringParser parser)
        {
            
        }
    }

    public enum TokenType {
        MethodCall,
        ValueNumeric,
        ValueCharacter
    }
}