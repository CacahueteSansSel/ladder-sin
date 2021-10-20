using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compiler.Core
{
    public class ArgumentReader
    {
        string[] args;

        public ArgumentReader(string[] args)
        {
            this.args = args;
        }

        public bool Contains(string flag)
            => args.Contains(flag);

        public string NextTo(string flag)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == flag && i != args.Length-1)
                {
                    return args[i + 1];
                }
            }

            return null;
        }

        public string[] GetAloneArguments()
        {
            List<string> argList = new List<string>();

            foreach (string arg in args)
            {
                if (arg.StartsWith("-"))
                {
                    // Argument is a flag, finishing here
                    return argList.ToArray();
                }

                argList.Add(arg);
            }

            return argList.ToArray();
        }
    }
}
