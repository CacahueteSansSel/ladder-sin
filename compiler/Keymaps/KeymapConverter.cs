using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Compiler.Keymaps
{
    public static class KeymapConverter
    {
        private static Dictionary<char, char> azertyToQwertyDict = new Dictionary<char, char>();

        public static void Load()
        {
            foreach (string line in File.ReadAllLines("azerty-qwerty.txt"))
            {
                if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line)) continue;

                string azerty = line.Split('=')[0];
                string qwerty = line.Split('=')[1];

                if (azerty == "equals") azerty = "=";
                if (qwerty == "equals") qwerty = "=";
                
                azertyToQwertyDict.Add(azerty[0], qwerty[0]);
                if (azertyToQwertyDict.ContainsKey(char.ToUpper(azerty[0]))) continue;
                azertyToQwertyDict.Add(char.ToUpper(azerty[0]), char.ToUpper(qwerty[0]));
            }
        }

        public static char ConvertAZERTYToQWERTY(char c)
            => azertyToQwertyDict.ContainsKey(c) ? azertyToQwertyDict[c] : c;

        public static char ConvertQWERTYToAZERTY(char c)
            => azertyToQwertyDict.Values.Contains(c) ? azertyToQwertyDict.First(kv => kv.Value == c).Key : c;

        public static string ConvertStringAZERTYToQWERTY(string input)
        {
            string output = "";

            foreach (char c in input)
                output += ConvertAZERTYToQWERTY(c);

            return output;
        }
        
        public static string ConvertStringQWERTYToAZERTY(string input)
        {
            string output = "";

            foreach (char c in input)
                output += ConvertQWERTYToAZERTY(c);

            return output;
        }
    }
}