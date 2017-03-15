using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoolCommandLine;

namespace TestCommandLine
{
    [TestClass]
    public class TestOptionOperations
    {
        [TestMethod]
        public void Titling()
        {
            bool titleOp   = false;
            bool lExecuted = false;
            var clm = new CommandLineManager();

            clm.ReportTitle(cml => titleOp = true)
               .AddOptionRequiresData("LD", "List out the operation.", (clmanager) => lExecuted = true)
               .Execute();

            Assert.IsTrue(titleOp);
            Assert.IsFalse(lExecuted);

            // Reset for alternate test.
            titleOp = false;
            clm.Parse(new[] { "-LD", "Data" })
               .Execute();

            Assert.IsFalse(titleOp);
            Assert.IsTrue(lExecuted);
        }
    }
}
