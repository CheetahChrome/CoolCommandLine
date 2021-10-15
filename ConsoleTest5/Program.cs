using CoolCommandLine;
using System;

namespace ConsoleTest5
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var info = "Take a data file and create templated SQL";

            var cml = new CommandLineManager();

            cml.DisplayProductAndVersion()  // Take the Product name and version from AssemblyInfo.cs
               .DisplayBeforeDescription((o) => Console.WriteLine($"{info}{Environment.NewLine}"))
               .DisplayDescriptionOnNoOperation()
               .AddOption("f", "Pathed data file.", ProcessFile)
               .Execute(new[] { "-f", "C:\\Temp\\abc.text" });
        }

        static void ProcessFile(CommandLineManager clm)
        {
            var file = clm["f"];

            Console.WriteLine($"Processing File {file}");

            Console.WriteLine(Environment.NewLine);
        }

    }



}
