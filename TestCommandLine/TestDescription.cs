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
    }
}
