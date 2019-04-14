using System;
using System.Linq;

namespace ImageToPDF
{
    static class Program
    {
        static void Main(string[] args)
        {
            var pdfGenerators = new PdfImageGenerator[] {
                new ImageFileConverter(),
                new PdfImageExtractor(),
            };
            foreach (var path in args)
            {
                try
                {
                    var validGenerator = pdfGenerators.First(generator => generator.IsValidFilename(path));
                    validGenerator.WritePdfImage(path);
                    Console.WriteLine($"Succeeded to convert {path} to PDF.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to convert {path} to PDF:{e.Message}");
                }
            }
            Console.WriteLine("All tasks finished.");
        }
    }
}
