using System.IO;

namespace Compiler.Formatting
{
    public static class EmbedCppFormatter
    {
        public static void FormatCodeResource(byte[] buffer, StreamWriter cppFile, StreamWriter headerFile)
        {
            cppFile.NewLine = "\n";
            headerFile.NewLine = "\n";
            
            cppFile.WriteLine($"#include \"code.h\"\n\nuint8_t CODE_BUFFER[{buffer.Length}] = \n{{");
            headerFile.WriteLine($"#pragma once\n#include <Arduino.h>\n\nextern uint8_t CODE_BUFFER[{buffer.Length}];");

            foreach (byte b in buffer)
            {
                cppFile.WriteLine($"    0x{b:x2}, ");
            }
            cppFile.WriteLine("};");
        }
    }
}