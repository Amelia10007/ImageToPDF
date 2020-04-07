using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageToPDF;

namespace UnitTest
{
    class LoggerImpl : ILogger
    {
        public string LastLog { get; private set; } = "";
        public LogLevel LogLevelOfLastLog { get; private set; }

        public LoggerImpl() { }

        public void SetLogLevel(LogLevel level) { }

        public void WriteLog(string log, LogLevel level)
        {
            this.LastLog = log;
            this.LogLevelOfLastLog = level;
        }
    }

    [TestClass]
    public class ArgumentOptionCollectionTest
    {
        [TestMethod]
        public void TestLoadArgumentOptionCollection_NoToken()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new ArgumentToken[0];
            var (optionCollection, remainningTokens) = ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger);

            Assert.IsFalse(optionCollection.AllowDestinationOverwrite);
            Assert.IsFalse(optionCollection.EnableRecursiveDirectorySearch);
            Assert.AreEqual(ImageFileKind.Pdf, optionCollection.DestinationImageFileKind);
            Assert.AreEqual(new Range<uint>(uint.MinValue, uint.MaxValue), optionCollection.PowerpointPageRange);
            Assert.IsFalse(optionCollection.EnableVersionInfoDisplay);

            Assert.AreEqual(0, remainningTokens.Count);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_OnlyFilePath()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("file1.bmp") };
            var (optionCollection, remainningTokens) = ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger);

            Assert.IsFalse(optionCollection.AllowDestinationOverwrite);
            Assert.IsFalse(optionCollection.EnableRecursiveDirectorySearch);
            Assert.AreEqual(ImageFileKind.Pdf, optionCollection.DestinationImageFileKind);
            Assert.AreEqual(new Range<uint>(uint.MinValue, uint.MaxValue), optionCollection.PowerpointPageRange);
            Assert.IsFalse(optionCollection.EnableVersionInfoDisplay);

            Assert.AreEqual(1, remainningTokens.Count);
            Assert.AreEqual(new ArgumentToken("file1.bmp"), remainningTokens[0]);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_AllowDestinationOverwrite_ShortOption()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("-w"), new ArgumentToken("file1.bmp") };
            var (optionCollection, remainningTokens) = ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger);

            Assert.IsTrue(optionCollection.AllowDestinationOverwrite);
            Assert.IsFalse(optionCollection.EnableRecursiveDirectorySearch);
            Assert.AreEqual(ImageFileKind.Pdf, optionCollection.DestinationImageFileKind);
            Assert.AreEqual(new Range<uint>(uint.MinValue, uint.MaxValue), optionCollection.PowerpointPageRange);
            Assert.IsFalse(optionCollection.EnableVersionInfoDisplay);

            Assert.AreEqual(1, remainningTokens.Count);
            Assert.AreEqual(new ArgumentToken("file1.bmp"), remainningTokens[0]);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_AllowDestinationOverwrite_LongOption()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("--allow-overwrite"), new ArgumentToken("file1.bmp") };
            var (optionCollection, remainningTokens) = ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger);

            Assert.IsTrue(optionCollection.AllowDestinationOverwrite);
            Assert.IsFalse(optionCollection.EnableRecursiveDirectorySearch);
            Assert.AreEqual(ImageFileKind.Pdf, optionCollection.DestinationImageFileKind);
            Assert.AreEqual(new Range<uint>(uint.MinValue, uint.MaxValue), optionCollection.PowerpointPageRange);
            Assert.IsFalse(optionCollection.EnableVersionInfoDisplay);

            Assert.AreEqual(1, remainningTokens.Count);
            Assert.AreEqual(new ArgumentToken("file1.bmp"), remainningTokens[0]);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_EnableRecursiveDirectorySearch_ShortOption()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("-r"), new ArgumentToken("file1.bmp") };
            var (optionCollection, remainningTokens) = ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger);

            Assert.IsFalse(optionCollection.AllowDestinationOverwrite);
            Assert.IsTrue(optionCollection.EnableRecursiveDirectorySearch);
            Assert.AreEqual(ImageFileKind.Pdf, optionCollection.DestinationImageFileKind);
            Assert.AreEqual(new Range<uint>(uint.MinValue, uint.MaxValue), optionCollection.PowerpointPageRange);
            Assert.IsFalse(optionCollection.EnableVersionInfoDisplay);

            Assert.AreEqual(1, remainningTokens.Count);
            Assert.AreEqual(new ArgumentToken("file1.bmp"), remainningTokens[0]);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_EnableRecursiveDirectorySearch_LongOption()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("--recursive"), new ArgumentToken("file1.bmp") };
            var (optionCollection, remainningTokens) = ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger);

            Assert.IsFalse(optionCollection.AllowDestinationOverwrite);
            Assert.IsTrue(optionCollection.EnableRecursiveDirectorySearch);
            Assert.AreEqual(ImageFileKind.Pdf, optionCollection.DestinationImageFileKind);
            Assert.AreEqual(new Range<uint>(uint.MinValue, uint.MaxValue), optionCollection.PowerpointPageRange);
            Assert.IsFalse(optionCollection.EnableVersionInfoDisplay);

            Assert.AreEqual(1, remainningTokens.Count);
            Assert.AreEqual(new ArgumentToken("file1.bmp"), remainningTokens[0]);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_DestinationImageFileKind_ShortOption()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("-t"), new ArgumentToken("eps"), new ArgumentToken("file1.bmp") };
            var (optionCollection, remainningTokens) = ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger);

            Assert.IsFalse(optionCollection.AllowDestinationOverwrite);
            Assert.IsFalse(optionCollection.EnableRecursiveDirectorySearch);
            Assert.AreEqual(ImageFileKind.Eps, optionCollection.DestinationImageFileKind);
            Assert.AreEqual(new Range<uint>(uint.MinValue, uint.MaxValue), optionCollection.PowerpointPageRange);
            Assert.IsFalse(optionCollection.EnableVersionInfoDisplay);

            Assert.AreEqual(1, remainningTokens.Count);
            Assert.AreEqual(new ArgumentToken("file1.bmp"), remainningTokens[0]);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_DestinationImageFileKind_LongOption()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("--output-type"), new ArgumentToken("eps"), new ArgumentToken("file1.bmp") };
            var (optionCollection, remainningTokens) = ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger);

            Assert.IsFalse(optionCollection.AllowDestinationOverwrite);
            Assert.IsFalse(optionCollection.EnableRecursiveDirectorySearch);
            Assert.AreEqual(ImageFileKind.Eps, optionCollection.DestinationImageFileKind);
            Assert.AreEqual(new Range<uint>(uint.MinValue, uint.MaxValue), optionCollection.PowerpointPageRange);
            Assert.IsFalse(optionCollection.EnableVersionInfoDisplay);

            Assert.AreEqual(1, remainningTokens.Count);
            Assert.AreEqual(new ArgumentToken("file1.bmp"), remainningTokens[0]);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_DestinationImageFileKind_InvalidType()
        {
            var logger = new LoggerImpl();
            // 'txt' file is invalid for output image type!
            var argumentTokens = new[] { new ArgumentToken("-t"), new ArgumentToken("txt"), new ArgumentToken("file1.bmp") };

            Assert.ThrowsException<ArgumentException>(delegate { ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger); });

            Assert.AreEqual(LogLevel.Error, logger.LogLevelOfLastLog);
            Assert.AreEqual(logger.LastLog, "Invalid output type: txt");
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_DestinationImageFileKind_NoTypeFound()
        {
            var logger = new LoggerImpl();
            // 'txt' file is invalid for output image type!
            var argumentTokens = new[] { new ArgumentToken("-t") };

            Assert.ThrowsException<ArgumentException>(delegate { ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger); });

            Assert.AreEqual(LogLevel.Error, logger.LogLevelOfLastLog);
            Assert.AreEqual(logger.LastLog, "Output type must be specified after \"-t\" or \"--output-type\" option.");
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_PowerpointPageRange_ShortOption()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("-p"), new ArgumentToken("3"), new ArgumentToken("5"), new ArgumentToken("slide.pptx") };
            var (optionCollection, remainningTokens) = ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger);

            Assert.IsFalse(optionCollection.AllowDestinationOverwrite);
            Assert.IsFalse(optionCollection.EnableRecursiveDirectorySearch);
            Assert.AreEqual(ImageFileKind.Pdf, optionCollection.DestinationImageFileKind);
            Assert.AreEqual(new Range<uint>(3, 5), optionCollection.PowerpointPageRange);
            Assert.IsFalse(optionCollection.EnableVersionInfoDisplay);

            Assert.AreEqual(1, remainningTokens.Count);
            Assert.AreEqual(new ArgumentToken("slide.pptx"), remainningTokens[0]);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_PowerpointPageRange_LongOption()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("--powerpoint-page-range"), new ArgumentToken("3"), new ArgumentToken("5"), new ArgumentToken("slide.pptx") };
            var (optionCollection, remainningTokens) = ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger);

            Assert.IsFalse(optionCollection.AllowDestinationOverwrite);
            Assert.IsFalse(optionCollection.EnableRecursiveDirectorySearch);
            Assert.AreEqual(ImageFileKind.Pdf, optionCollection.DestinationImageFileKind);
            Assert.AreEqual(new Range<uint>(3, 5), optionCollection.PowerpointPageRange);
            Assert.IsFalse(optionCollection.EnableVersionInfoDisplay);

            Assert.AreEqual(1, remainningTokens.Count);
            Assert.AreEqual(new ArgumentToken("slide.pptx"), remainningTokens[0]);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_PowerpointPageRange_OutOfRangePage()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("-p"), new ArgumentToken("3"), new ArgumentToken("2"), new ArgumentToken("slide.pptx") };

            Assert.ThrowsException<ArgumentException>(delegate { ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger); });

            Assert.AreEqual(LogLevel.Error, logger.LogLevelOfLastLog);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_PowerpointPageRange_NoPageSpecified()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("-p") };

            Assert.ThrowsException<ArgumentException>(delegate { ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger); });

            Assert.AreEqual(LogLevel.Error, logger.LogLevelOfLastLog);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_PowerpointPageRange_TooFewPageArgument()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("-p"), new ArgumentToken("5") };

            Assert.ThrowsException<ArgumentException>(delegate { ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger); });

            Assert.AreEqual(LogLevel.Error, logger.LogLevelOfLastLog);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_PowerpointPageRange_ParseFail()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("-p"), new ArgumentToken("5"), new ArgumentToken("aiueo") };

            Assert.ThrowsException<ArgumentException>(delegate { ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger); });

            Assert.AreEqual(LogLevel.Error, logger.LogLevelOfLastLog);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_EnableVersionInfoDisplay_ShortOption()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("-v") };
            var (optionCollection, remainningTokens) = ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger);

            Assert.IsFalse(optionCollection.AllowDestinationOverwrite);
            Assert.IsFalse(optionCollection.EnableRecursiveDirectorySearch);
            Assert.AreEqual(ImageFileKind.Pdf, optionCollection.DestinationImageFileKind);
            Assert.AreEqual(new Range<uint>(uint.MinValue, uint.MaxValue), optionCollection.PowerpointPageRange);
            Assert.IsTrue(optionCollection.EnableVersionInfoDisplay);

            Assert.AreEqual(0, remainningTokens.Count);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_EnableVersionInfoDisplay_LongOption()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("--version") };
            var (optionCollection, remainningTokens) = ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger);

            Assert.IsFalse(optionCollection.AllowDestinationOverwrite);
            Assert.IsFalse(optionCollection.EnableRecursiveDirectorySearch);
            Assert.AreEqual(ImageFileKind.Pdf, optionCollection.DestinationImageFileKind);
            Assert.AreEqual(new Range<uint>(uint.MinValue, uint.MaxValue), optionCollection.PowerpointPageRange);
            Assert.IsTrue(optionCollection.EnableVersionInfoDisplay);

            Assert.AreEqual(0, remainningTokens.Count);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_LogLevel_ShortOption()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("-L"), new ArgumentToken("debug"), new ArgumentToken("file1.png") };
            var (optionCollection, remainningTokens) = ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger);

            Assert.IsFalse(optionCollection.AllowDestinationOverwrite);
            Assert.IsFalse(optionCollection.EnableRecursiveDirectorySearch);
            Assert.AreEqual(ImageFileKind.Pdf, optionCollection.DestinationImageFileKind);
            Assert.AreEqual(new Range<uint>(uint.MinValue, uint.MaxValue), optionCollection.PowerpointPageRange);
            Assert.IsFalse(optionCollection.EnableVersionInfoDisplay);

            Assert.AreEqual(1, remainningTokens.Count);
            Assert.AreEqual(new ArgumentToken("file1.png"), remainningTokens[0]);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_LogLevel_LongOption()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("--log-level"), new ArgumentToken("debug"), new ArgumentToken("file1.png") };
            var (optionCollection, remainningTokens) = ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger);

            Assert.IsFalse(optionCollection.AllowDestinationOverwrite);
            Assert.IsFalse(optionCollection.EnableRecursiveDirectorySearch);
            Assert.AreEqual(ImageFileKind.Pdf, optionCollection.DestinationImageFileKind);
            Assert.AreEqual(new Range<uint>(uint.MinValue, uint.MaxValue), optionCollection.PowerpointPageRange);
            Assert.IsFalse(optionCollection.EnableVersionInfoDisplay);

            Assert.AreEqual(1, remainningTokens.Count);
            Assert.AreEqual(new ArgumentToken("file1.png"), remainningTokens[0]);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_LogLevel_InvalidLevel()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("--log-level"), new ArgumentToken("carropino"), new ArgumentToken("file1.png") };

            Assert.ThrowsException<ArgumentException>(delegate { ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger); });

            Assert.AreEqual(LogLevel.Error, logger.LogLevelOfLastLog);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_Combination()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] {
                new ArgumentToken("-w"),
                new ArgumentToken("-r"),
                new ArgumentToken("-t"),
                new ArgumentToken("eps"),
                new ArgumentToken("-p"),
                new ArgumentToken("5"),
                new ArgumentToken("10"),
                new ArgumentToken("-L"),
                new ArgumentToken("debug"),
                new ArgumentToken("slide.pptx") };
            var (optionCollection, remainningTokens) = ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger);

            Assert.IsTrue(optionCollection.AllowDestinationOverwrite);
            Assert.IsTrue(optionCollection.EnableRecursiveDirectorySearch);
            Assert.AreEqual(ImageFileKind.Eps, optionCollection.DestinationImageFileKind);
            Assert.AreEqual(new Range<uint>(5, 10), optionCollection.PowerpointPageRange);
            Assert.IsFalse(optionCollection.EnableVersionInfoDisplay);

            Assert.AreEqual(1, remainningTokens.Count);
            Assert.AreEqual(new ArgumentToken("slide.pptx"), remainningTokens[0]);
        }

        [TestMethod]
        public void TestLoadArgumentOptionCollection_UnexpectedOption()
        {
            var logger = new LoggerImpl();
            var argumentTokens = new[] { new ArgumentToken("--carro-pino"),  new ArgumentToken("file1.png") };

            Assert.ThrowsException<ArgumentException>(delegate { ArgumentOptionCollection.LoadArgumentOptionCollection(argumentTokens, logger); });

            Assert.AreEqual(LogLevel.Error, logger.LogLevelOfLastLog);
            Assert.AreEqual("Unexpected option \"--carro-pino\"", logger.LastLog);
        }
    }
}
