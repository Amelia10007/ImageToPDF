using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageToPDF;

namespace UnitTest
{
    [TestClass]
    public class IdTest
    {
        [TestMethod]
        public void TestGetNext()
        {
            var former = Id.GetNext();
            var latter = Id.GetNext();

            Assert.AreNotEqual(former, latter);
            Assert.IsTrue(former.CompareTo(latter) < 0);
        }
    }
}
