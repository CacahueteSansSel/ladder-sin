using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using compiler.Core;
using compiler.Emit;
using Compiler.Formatting;
using compiler.Instructions;
using Compiler.Keymaps;
using Compiler.Parsing;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Gray;

            ArgumentReader argReader = new(args);
            string[] inputFiles = argReader.GetAloneArguments();
            string outputFile = argReader.NextTo("-o");
            string firmPath = argReader.Contains("-firm") ? argReader.NextTo("-firm") : null;

            if (inputFiles.Length == 0)
            {
                CLI.Error("ladderc", "no input files");
                return;
            }

            if (outputFile == null)
            {
                CLI.Error("ladderc", "no output file specified");
                return;
            }

            if (File.Exists(outputFile)) File.Delete(outputFile);
            
            // Load all instructions
            InstructionLoader.Load();
            Keycodes.Load();
            KeymapConverter.Load();
            FileStream outputFileStrm = File.OpenWrite(outputFile);
            CompilerEnvironment env = new(outputFileStrm);
            Parser parser = new();
            foreach (string file in inputFiles)
            {
                try
                {
                    string fileRaw = File.ReadAllText(file);
                    env.Include(parser.Parse(fileRaw));
                } 
                catch (Exception e)
                {
                    CLI.Error("ladderc", $"{file}: {e.Message}");
                }
            }
            
            //env.Dump();
            env.Compile();
            outputFileStrm.Close();

            if (firmPath == null) return;
            firmPath = firmPath.TrimEnd('\\').TrimEnd('/');

            string sourcePath = $"{firmPath}/src";
            StreamWriter codeCppSW = new StreamWriter($"{sourcePath}/code.cpp");
            StreamWriter codeHeaderSW = new StreamWriter($"{sourcePath}/code.h");
            
            EmbedCppFormatter.FormatCodeResource(File.ReadAllBytes(outputFile), codeCppSW, codeHeaderSW);
            
            codeCppSW.Close();
            codeHeaderSW.Close();

            Process cmd = Process.Start(new ProcessStartInfo()
            {
                FileName = "cmd",
                Arguments = "/c \"platformio run\"",
                WorkingDirectory = firmPath,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true
            });

            while (!cmd.HasExited)
            {
                string line = cmd.StandardOutput.ReadLine();
                CLI.Note("platformio", line);
            }

            if (cmd.ExitCode != 0)
            {
                CLI.Error("platformio", $"exited with code {cmd.ExitCode}");
                return;
            }

            string deviceRoot = null;
            while (true)
            {
                deviceRoot = GetRPIDevicePrefix();
                if (deviceRoot != null) break;
                Console.WriteLine("Connect the Raspberry Pi Pico to the computer while holding the BOOTSEL button and press any key...");
                Console.ReadKey();
            }
            
            Console.WriteLine("Copying...");
            string uf2Path = $"{firmPath}/.pio/build/pico/firmware.uf2";
            File.Copy(uf2Path, $"{deviceRoot}/firmware.uf2");
            Console.WriteLine("Done !");
        }

        static void PrintToken(int indent, ParserToken token) 
        {
            for (int i = 0; i < indent; i++) Console.Write(" ");
            Console.WriteLine($"[{token.Type}] {token.Text}");
            foreach (ParserToken child in token.Childs) 
            {
                PrintToken(indent+1, child);
            }
        }

        static string GetRPIDevicePrefix()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (!drive.IsReady) continue;
                
                if (drive.VolumeLabel == "RPI-RP2")
                    return drive.RootDirectory.FullName;
            }

            return null;
        }
    }
}
