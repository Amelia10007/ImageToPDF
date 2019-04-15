using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageToPDF
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Images to PDF");
            var pdfGenerators = new PdfImageGenerator[] {
                new ImageFileConverter(),
                new PdfImageExtractor(),
            };
            var filenames = getTargetFilenames(args);
            var index = 1;
            foreach (var path in filenames)
            {
                Console.WriteLine($"[{index++}/{args.Length}]: {path}");
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
        static IEnumerable<string> getTargetFilenames(IEnumerable<string> args)
        {
            foreach (var arg in args)
            {
                if (Directory.Exists(arg))
                {
                    var filenames = Directory.EnumerateFiles(arg, "*", SearchOption.AllDirectories);
                    foreach (var filename in filenames) yield return filename;
                }
                else
                {
                    yield return arg;
                }
            }
        }
    }
}
