using System;
using System.Linq;

namespace ImageToPDF
{
    static class PdfImageGeneratorCollection
    {
        private static readonly IPdfImageGenerator[] generators;

        static PdfImageGeneratorCollection()
        {
            var regular = new RegularImageConverter();
            var eps = new EpsConverter();
            var emf = new EmfConverter(eps);
            var powerpoint = new PowerPointExtracter(emf);
            var pdf = new PdfImageExtractor();
            generators = new IPdfImageGenerator[] { regular, eps, emf, powerpoint, pdf };
        }

        public static void WritePdfImage(TaskCommand command)
        {
            var validGenerator = generators.FirstOrDefault(generator => generator.IsValidCommand(command))
                ?? throw new ArgumentException($"Cannot execute {command} because of invalid format.");
            validGenerator.SaveAsPdf(command);
        }
    }
}
