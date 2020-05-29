using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab14;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Program.col = new TestCollections(10);
            Program.Sample(4, Program.col);
            Program.Count(17, Program.col);
            Program.Variety(10, 6, Program.col);
            Program.Grouping(7, Program.col);
            Program.Aggregation(Program.col);
        }
    }
}
