using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoolCommandLine;

namespace TestCommandLine
{
    [TestClass]
    public class TestOptionOperations
    {
        [TestMethod]
        public void TitlingAndDescription()
        {
            bool otherOp = false;

            CommandLineManager.Instantiation()
                              .ReportTitleAndOptionInfoWhenNoAction(cml => otherOp = true)
                              .Execute(null);

            Assert.IsTrue(otherOp);

        }
    }
}
