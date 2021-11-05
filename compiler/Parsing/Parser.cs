using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using compiler.Core;
using Compiler.Keymaps;

namespace Compiler.Parsing 
{
    public class Parser 
    {
        string[] SplitIntoLines(string rawText)
            => rawText.Split('\n');

        /// This method is used to parse input script files into tokens
        public ParserToken[] Parse(string rawFileText) 
        {
            string[] lines = SplitIntoLines(rawFileText);
            List<ParserToken> tokenList = new();
            bool inMacro = false;
            string macroName = null;
            string[] macroArgs = null;
            List<ParserToken> macroTokens = new List<ParserToken>();

            foreach (string line in lines) 
            {
                if (line.StartsWith("//") || line.StartsWith("#") || string.IsNullOrWhiteSpace(line.Replace("\r", ""))) continue;
                string[] lineToks = line.Replace("\r", "").Trim().Replace("\n", "").SplitQuotes(' ');
                string instName = lineToks[0];
                if (instName == "include")
                {
                    string libName = lineToks[1];
                    tokenList.AddRange(Parse(File.ReadAllText($"Include/{libName}")));
                    continue;
                }
                if (instName == "macro")
                {
                    // Reading a macro
                    macroName = lineToks[1];
                    macroArgs = lineToks.Skip(2).ToArray();
                    inMacro = true;
                    continue;
                }
                if (instName == "end")
                {
                    if (!inMacro)
                    {
                        CLI.Error("ladderc", "syntax error: found 'end' but no macro was started");
                        return tokenList.ToArray();
                    }

                    ParserToken token = new ParserToken()
                        { Text = macroName, Array = macroArgs, Type = TokenType.DefinitionMacro, Childs = new List<ParserToken>(macroTokens) };
                    tokenList.Add(token);
                    macroTokens.Clear();
                    inMacro = false;
                    macroArgs = null;
                    macroName = null;

                    continue;
                }
                ParserToken instToken = new();
                instToken.Text = instName;
                instToken.Type = TokenType.InstructionCall;

                foreach (string child in lineToks.Skip(1)) 
                {
                    if (inMacro && macroArgs.Contains(child))
                    {
                        instToken.Childs.Add(new ParserToken() {Text = child, Type = TokenType.ValueArgument});
                        continue;
                    }
                    
                    // Add the token into the instruction's token
                    instToken.Childs.Add(new(new StringParser(child)));
                }
                if (inMacro) macroTokens.Add(instToken);
                else tokenList.Add(instToken); // Add the instruction's token into the main list
            }

            return tokenList.ToArray();
        }
    }

    public class ParserToken 
    {
        public string Text { get; set; }
        public string[] Array { get; set; }
        public TokenType Type { get; set; }

        public List<ParserToken> Childs { get; set; } = new();

        public ParserToken()
        {
            
        }
        
        public ParserToken(ParserToken old)
        {
            Text = old.Text;
            Array = old.Array;
            Type = old.Type;
            Childs = new List<ParserToken>(old.Childs);
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
                if (parser.Expect('a'))
                {
                    Text = KeymapConverter.ConvertStringAZERTYToQWERTY(Text);
                    Console.WriteLine($"Converted string to QWERTY: {Text}");
                }
            } else if (parser.Expect('['))
            {
                parser.Consume();
                Text = parser.ConsumeTo(']');
                Type = TokenType.ValueKeycode;
            }
            else if (parser.ExpectNumber() || parser.Expect('-'))
            {
                Text = parser.ConsumeTo((c) => !(char.IsDigit(c) || c == '-'));
                Type = TokenType.ValueNumeric;
            } else
            {
                Text = parser.ReadToEnd();
                Type = TokenType.ValueUnknown;
            }
        }

        public ParserToken Clone() => (ParserToken)this.MemberwiseClone();

        public bool Contains(TokenType type) => Childs.Count(tok => tok.Type == type) > 0;
    }

    public enum TokenType 
    {
        InstructionCall,
        ValueNumeric,
        ValueCharacter,
        ValueKeycode,
        ValueString,
        ValueArgument,
        ValueUnknown,
        DefinitionMacro
    }
}