using System;
using System.Linq;

namespace ImageToPDF
{
    static class PdfImageGeneratorCollection
    {
        private static readonly PdfImageGenerator[] generators = new PdfImageGenerator[]
        {
            new ImageFileConverter(),
            new PowerPointExtracter(),
            new PdfImageExtractor()
        };

        public static void WritePdfImage(string sourceFilename)
        {
            var validGenerator = generators.FirstOrDefault(generator => generator.IsValidFilename(sourceFilename))
                ?? throw new ArgumentException($"Cannot convert {sourceFilename} to PDF because of invalid format.");
            validGenerator.WritePdfImage(sourceFilename);
        }
    }
}
