using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageToPDF;

namespace UnitTest
{
    [TestClass]
    public class ImageFileKindTest
    {
        [TestMethod]
        public void TestEquality()
        {
            var kinds = ImageFileKind.Kinds;
            for (var x = 0; x < kinds.Length; x++)
            {
                for (var y = 0; y < kinds.Length; y++)
                {
                    if (x == y)
                    {
                        Assert.AreEqual(kinds[x], kinds[y]);
                    }
                    else
                    {
                        Assert.AreNotEqual(kinds[x], kinds[y]);
                    }
                }
            }
        }

        [TestMethod]
        public void TestFromExtension()
        {
            Assert.AreEqual(ImageFileKind.Bmp, ImageFileKind.FromExtension(".bmp"));
            Assert.AreEqual(ImageFileKind.Png, ImageFileKind.FromExtension("png"));
            Assert.AreEqual(ImageFileKind.Jpg, ImageFileKind.FromExtension(".jpg"));
            Assert.AreEqual(ImageFileKind.Jpg, ImageFileKind.FromExtension(".jpeg"));
            Assert.AreEqual(ImageFileKind.Gif, ImageFileKind.FromExtension("Gif"));
            Assert.AreEqual(ImageFileKind.Tif, ImageFileKind.FromExtension("TIF"));
            Assert.AreEqual(ImageFileKind.Tif, ImageFileKind.FromExtension(".TIFF"));
            Assert.AreEqual(ImageFileKind.Wmf, ImageFileKind.FromExtension(".Wmf"));
            Assert.AreEqual(ImageFileKind.Emf, ImageFileKind.FromExtension("emf"));
            Assert.AreEqual(ImageFileKind.Eps, ImageFileKind.FromExtension("eps"));
            Assert.AreEqual(ImageFileKind.Pdf, ImageFileKind.FromExtension(".pdf"));
            Assert.AreEqual(ImageFileKind.Powerpoint, ImageFileKind.FromExtension(".ppt"));
            Assert.AreEqual(ImageFileKind.Powerpoint, ImageFileKind.FromExtension(".pptx"));
        }

        [TestMethod]
        public void TestFromExtension_UnexpectedExtension()
        {
            Assert.ThrowsException<InvalidOperationException>(delegate { ImageFileKind.FromExtension(".txt"); });
        }

        [TestMethod]
        public void TestFromExtension_GetExtensionWithDot()
        {
            Assert.AreEqual(".pdf", ImageFileKind.Pdf.GetExtensionWithDot());
            Assert.AreEqual(".eps", ImageFileKind.Eps.GetExtensionWithDot());
        }
    }
}
