using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace compiler.Core {

    public static class Keycodes 
    {
        static Dictionary<string, byte> keycodes = new();

        public static byte Get(string name)
            => keycodes.First(k => k.Key == name).Value;

        public static void Load() 
        {
            if (!File.Exists("keycodes.txt"))
            {
                CLI.Error("ladderc", "cannot find the keycode file (keycodes.txt)");
                return;
            }

            foreach (string line in File.ReadAllLines("keycodes.txt")) 
            {
                if (line.StartsWith("#")) continue;

                string[] toks = line.Split('=');
                string name = toks[0];
                byte b = byte.Parse(toks[1].Replace("0x", ""), NumberStyles.HexNumber);

                keycodes.Add(name, b);
            }
        }
    }

}