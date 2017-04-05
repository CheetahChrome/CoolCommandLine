using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoolCommandLine;

namespace TestCommandLine
{
    [TestClass]
    public class TestDescription
    {
        /// <summary>
        /// This test is not an assert test, but one to look at the output
        /// to see if it is ok.
        /// </summary>
        [TestMethod]
        public void TestBasicDescription()
        {
            CommandLineManager.Instantiation()
                              .DisplayTitleAndVersion()
                              .DisplayDescriptionOnNoOperation()
                              .AddOption("M", "Migrate the directory")
                              .AssociateWithSubOptions("D")
                              .AddOptionRequiresData("D", "Directory To Migrate")
                              .AddOption("L", "List the directory contents")
                              .AssociateWithSubOptions("D")
                              .Execute();
        }


        [TestMethod]
        public void TestMultipleOptions()
        {
            string info = "Operations to be performed: ";

            CommandLineManager.Instantiation()
                  .DisplayBeforeDescription((o) => Console.WriteLine($"{info}{Environment.NewLine}"))
                  .DisplayProductAndVersion()
                  .DisplayDescriptionOnNoOperation()
                  .AddOption("Q", "Queue the App into named pipe remote command mode", clm => Console.WriteLine("Queue Mode"))
                  .AddOption("U", "Upload a file to Azure", clm => Console.WriteLine("Uploading"))
                  .AddValidation(clm => true)
                  .AssociateWithSubOptions("F", "C", "B")
                  .AddOptionRequiresData("F", "File name of the file.")
                  .AddOptionRequiresData("C", "(Azure) Container to use or create in Azure.")
                  .AddOptionRequiresData("B", "(Azure) Blob name to use. If none specified, the filename will be used.")
                  .AddOption("D", "Download a file From Azure", clm => Console.WriteLine("Downloading"))
                  .AddValidation(clm => true)
                  .AssociateWithSubOptions("F", "C", "B")
                  .AddOption("S", "Show credentials to be used.",clm => Console.WriteLine("Showing"))
                  .Execute();





        }

    }
}
