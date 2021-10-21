using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using compiler.Emit;

namespace compiler.Instruction 
{
    public static class InstructionLoader 
    {
        public static List<InstructionBase> LoadedInstructions { get; } = new();
    
        public static InstructionBase Get(string name)
            => LoadedInstructions.FirstOrDefault(inst => inst.Name == name);

        public static void Load() 
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()) 
            {
                if (t.IsSubclassOf(typeof(InstructionBase)))
                    LoadedInstructions.Add((InstructionBase)Activator.CreateInstance(t));
            }
        }
    }
}