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
            Assert.IsTrue(clm.Z);
            Assert.IsTrue(clm.S);
            Assert.IsFalse(clm.D);
            Assert.IsTrue(lExecuted);
            Assert.IsTrue(zExecuted);

        }

        [TestMethod]
        public void TestBasic_FreeFormAllowed()
        {
            bool lExecuted = false;
            bool zExecuted = false;
            bool sExecuted = false;

            var clm = new CommandLineManager();

            clm.AddOption("L", "List out the operation.", (clmanager) => lExecuted = true)
               .AddOption("S,Save", "Save the operation.", (clmanager) => sExecuted = true)
               .AddOption("Zebra", "Zero out the operation.", (clmanager) => zExecuted = true)
               .AllowFreeForm()
               .Execute(new[] { "L","Data", "-Zebra", "-Save" });

            Assert.IsTrue(lExecuted);
            Assert.IsTrue(clm.L);
            Assert.IsTrue(clm.S);
            Assert.IsFalse(clm.D);
            Assert.IsTrue(lExecuted);
            Assert.IsTrue(zExecuted);
            Assert.IsTrue(sExecuted);

        }

    }
}
