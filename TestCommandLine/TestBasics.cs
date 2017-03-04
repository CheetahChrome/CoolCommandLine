using System;
using CoolCommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCommandLine
{
    [TestClass]
    public class TestBasics
    {
        [TestMethod]
        public void TestBasic()
        {
            bool lExecuted = false;
            bool zExecuted = false;

            var clm = new CommandLineManager();

            clm.AddOption("L", "List out the operation.", (clmanager) => lExecuted = true);
            clm.AddOption("S,Save", "Save the operation.");
            clm.AddOption("Zebra", "Zero out the operation.", (clmanager) => zExecuted = true);

            clm.Execute(new[] { "-L", "Data", "-Zebra", "-Save" });

            
            Assert.IsTrue(clm.L);
            Assert.IsTrue(clm.IsMultipleLetterOptionFound("L"));
            Assert.IsFalse(clm.IsMultipleLetterOptionFound("Data"));
            Assert.IsTrue(clm.S);
            Assert.IsFalse(clm.D);
            Assert.IsTrue(lExecuted);

            Assert.IsTrue(clm.IsMultipleLetterOptionFound("Zebra"));

            Assert.IsFalse(clm.Z);
            Assert.IsTrue(zExecuted);

        }

        [TestMethod]
        public void TestData()
        {
            bool lExecuted = false;
            bool zExecuted = false;

            var clm = new CommandLineManager();

            clm.AddOptionRequiresData("L", "List out the operation.", (clmanager) => lExecuted = true);
            clm.AddOption("S,Save", "Save the operation.");
            clm.AddOption("Zebra", "Zero out the operation.", (clmanager) => zExecuted = true);

            clm.Execute(new[] { "-L", "Data", "-Zebra", "-Save", "Unknown"});


            Assert.IsTrue(clm.L);
            Assert.IsTrue(clm["L"].Equals("Data"));

            Assert.IsTrue(string.IsNullOrEmpty(clm["Z"]));
            Assert.IsTrue(clm.S);
            Assert.IsTrue(string.IsNullOrEmpty(clm["S"]));
            Assert.IsFalse(clm.D);
            Assert.IsTrue(string.IsNullOrEmpty(clm["D"]));
            Assert.IsTrue(lExecuted);
            Assert.IsFalse(clm.Z);
            Assert.IsTrue(zExecuted);

        }



        /// <summary>
        /// We expect to be able to have a double letter as an option and have its data accessed like a single.
        /// </summary>
        [TestMethod]
        public void TestDataDoubleLetter()
        {
            bool lExecuted = false;

            var clm = new CommandLineManager();

            clm.AddOptionRequiresData("LD", "List out the operation.", (clmanager) => lExecuted = true);

            clm.Execute(new[] { "-LD", "Data" });
            Assert.IsTrue(clm["LD"].Equals("Data"));
            Assert.IsTrue(lExecuted);


        }


        [TestMethod]
        public void TestValidation()
        {

            bool lExecuted = false;

            var clm = new CommandLineManager();

            clm.AddOptionRequiresData("L", "List out the operation.", (clmanager) => lExecuted = true);

            clm.AddValidation((manager) => false);

            clm.Execute(new[] { "-L", "Data" });

            Assert.IsFalse(lExecuted);

            // Change it to true. We can do this because the `LastOption` is still set.
            clm.AddValidation((manager) => true);

            clm.Execute(new[] { "-L", "Data" });


            Assert.IsTrue(lExecuted);

        }
        //[TestMethod]
        //public void TestBasic_FreeFormAllowed()
        //{
        //    bool lExecuted = false;
        //    bool zExecuted = false;
        //    bool sExecuted = false;

        //    var clm = new CommandLineManager();

        //    clm.AddOption("L", "List out the operation.", (clmanager) => lExecuted = true)
        //       .AddOption("S,Save", "Save the operation.", (clmanager) => sExecuted = true)
        //       .AddOption("Zebra", "Zero out the operation.", (clmanager) => zExecuted = true)
        //       .AllowFreeForm()
        //       .Execute(new[] { "L","Data", "-Zebra", "-Save" });

        //    Assert.IsTrue(lExecuted);
        //    Assert.IsTrue(clm.L);
        //    Assert.IsTrue(clm.S);
        //    Assert.IsFalse(clm.D);
        //    Assert.IsTrue(lExecuted);
        //    Assert.IsTrue(zExecuted);
        //    Assert.IsTrue(sExecuted);

        //}


        [TestMethod]
        public void TestArgument()
        {

            var argOption = new Argument() {Text = "-L"};

            Assert.IsTrue(argOption.IsArgument);
            Assert.IsFalse(argOption.IsData);
            Assert.IsTrue(argOption.TextWithoutDash.Equals("L"));

            var argData = new Argument() { Text = @"C:\Temp" };

            Assert.IsFalse(argData.IsArgument);
            Assert.IsTrue(argData.IsData);

        }

    }
}
