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
            var filenames = GetTargetFilenames(args).ToArray();
            Console.WriteLine($"Detect {filenames.Length} files from arguments.");
            foreach (var tuple in filenames.WithIndex())
            {
                var path = tuple.Item1;
                var index = tuple.Item2;
                Console.WriteLine($"[{index + 1}/{filenames.Length}]: {path}");
                try
                {
                    PdfImageGeneratorCollection.WritePdfImage(path);
                    Console.WriteLine($"Succeeded to convert {path} to PDF.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to convert {path} to PDF\n\n{e.Message}\n\n{e.StackTrace}");
                }
            }
            Console.WriteLine("All tasks finished.");
        }

        static IEnumerable<string> GetTargetFilenames(IEnumerable<string> args)
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
