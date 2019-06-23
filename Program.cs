using System;
using System.Linq;

namespace ImageToPDF
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Images to PDF");
            var commands = ArgumentParser.ParseArguments(args).WithIndex().ToArray();
            foreach (var (command, index) in commands)
            {
                Console.WriteLine($"[{index + 1}/{commands.Length}]: {command}");
                try
                {
                    PdfImageGeneratorCollection.WritePdfImage(command);
                    Console.WriteLine($"Succeeded to convert {command.SourceFilename} to PDF.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to convert {command.SourceFilename} to PDF:\n{e.Message}\n\n{e.StackTrace}");
                }
            }
            Console.WriteLine("All tasks finished.");
        }
    }
}
