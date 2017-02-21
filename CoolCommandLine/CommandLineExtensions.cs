using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoolCommandLine
{
    public static class CommandLineExtensions
    {

        public static CommandLineManager Parse(this CommandLineManager manager, string[] args)
        {
            return manager.Parse(args);
        }

        public static void Execute(this CommandLineManager manager, string[] args)
        {
            manager.ExecuteOperation(args);
        }

        /// <summary>
        /// If a dash is not required, the an option can be designated without the dash such as `-L` can also be `L`. Hence it is "FreeForm".
        /// </summary>
        public static CommandLineManager AllowFreeForm(this CommandLineManager manager)
        {
            return manager?.AllowFreeForm();
        }

    }
}
