using System;
using System.Collections.Generic;

namespace Compiler.Parsing
{
    public static class ParsingExts 
    {
        public static string[] SplitQuotes(this string text, char splitter) 
        {
            string buf = "";
            bool inQuotes = false;
            List<string> list = new();

            foreach (char c in text) 
            {
                if (c == splitter && !inQuotes) 
                {
                    list.Add(buf);
                    buf = "";
                    continue;
                }

                if (c == '"') inQuotes = !inQuotes;

                buf += c;
            }
            list.Add(buf);

            return list.ToArray();
        }
    }
}