using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageToPDF;

namespace UnitTest
{
    [TestClass]
    public class RangeTest
    {
        [TestMethod]
        public void TestContains()
        {
            var range = new Range<int>(5, 7);
            Assert.IsTrue(range.Contains(5));
            Assert.IsTrue(range.Contains(6));
            Assert.IsTrue(range.Contains(7));
            Assert.IsFalse(range.Contains(4));
            Assert.IsFalse(range.Contains(8));
        }

        [TestMethod]
        public void TestContains_SameStartAndEnd()
        {
            var range = new Range<int>(5, 5);
            Assert.IsTrue(range.Contains(5));
            Assert.IsFalse(range.Contains(6));
            Assert.IsFalse(range.Contains(4));
        }

        [TestMethod]
        public void TestInstanciate_OutOfRange()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(delegate { new Range<int>(4, 3); });
        }
    }
}
