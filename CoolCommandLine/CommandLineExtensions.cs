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


        /// <summary>
        /// Execute the arguments, pass in arguments if <see cref="Parse"/> has not been called with arguments.
        /// </summary>
        /// <param name="manager">Target manager for the fluent chaining flow.</param>
        /// <param name="args">Optionals arguments when arguments have not been passed in.</param>
        public static void Execute(this CommandLineManager manager, string[] args = null)
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
