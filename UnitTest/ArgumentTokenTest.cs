using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageToPDF;

namespace UnitTest
{
    [TestClass]
    public class ArgumentTokenTest
    {
        [TestMethod]
        public void TestEquality()
        {
            Assert.AreEqual(new ArgumentToken("file1.bmp"), new ArgumentToken("file1.bmp"));
            Assert.AreNotEqual(new ArgumentToken("file1.bmp"), new ArgumentToken("file2.bmp"));
        }

        [TestMethod]
        public void TestParseArgument()
        {
            var arguments = new[] { "--some-option", "file1.bmp", "file2.png" };
            var tokens = ArgumentToken.ParseArgument(arguments).ToArray();
            var expected = new[] { new ArgumentToken("--some-option"), new ArgumentToken("file1.bmp"), new ArgumentToken("file2.png") };

            CollectionAssert.AreEqual(expected, tokens);
        }

        [TestMethod]
        public void TestParseArgumentEmpty()
        {
            var tokens = ArgumentToken.ParseArgument(new string[0]).ToArray();
            Assert.IsFalse(tokens.Any());
        }
    }
}

