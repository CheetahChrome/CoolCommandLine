using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CoolCommandLine;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {

            (new CommandLineManager())
                                      .DisplayTitleAndVersion()
                                      .AddOption("L", "List out the operation.", (clm)=> Console.WriteLine("List out the operation"))
                                      .Execute(args)
                            ;

        }
    }
}
