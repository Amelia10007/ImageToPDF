using System;
using System.Linq;

namespace ImageToPDF
{
    static class Program
    {
        static void Main(string[] args)
        {
            var imageConverterImpls = new IImageConverter[] { };

            var logger = new StdoutLogger();
            ArgumentParse parsedArgument;

            try
            {
                parsedArgument = new ArgumentParse(args, logger);
            }
            catch (ArgumentException e)
            {
                logger.WriteLog(e.Message, LogLevel.Error);
                return;
            }

            // バージョン情報の表示を指定された場合は、それを表示してプログラム終了
            if (parsedArgument.OptionCollection.EnableVersionInfoDisplay)
            {
                PrintVersionInfo();
                return;
            }

            foreach (var sourceFile in parsedArgument.SourceFiles)
            {
                logger.WriteLog($"Set source image file: {sourceFile.Path}", LogLevel.Debug);
            }

            foreach (var sourceFile in parsedArgument.SourceFiles)
            {
                logger.WriteLog($"Source image file: {sourceFile.Path}", LogLevel.Info);
                var destinationFile = sourceFile.GetConvertionDestination(parsedArgument.OptionCollection.DestinationImageFileKind);
                logger.WriteLog($"Destination image file: {destinationFile.Path}", LogLevel.Debug);


                var imageConverter = imageConverterImpls.SingleOrDefault(impl =>
                      impl.GetApplicableImageFileKinds().Contains(sourceFile.ImageFileKind)
                      && impl.GetAvailableDestinationImageFileKinds().Contains(destinationFile.ImageFileKind));
                if (imageConverter == null)
                {
                    logger.WriteLog($"No suitable converter found. source image kind: {sourceFile.ImageFileKind}, destination image kind: {destinationFile.ImageFileKind}", LogLevel.Error);
                    continue;
                }
                else
                {
                    logger.WriteLog("Found a converter", LogLevel.Debug);
                }

                imageConverter.ConvertThenSave(sourceFile, destinationFile, logger);
                logger.WriteLog($"Successfully converted an image to {destinationFile.Path}", LogLevel.Info);
            }
        }

        private static void PrintVersionInfo()
        {
            Console.WriteLine("1.1");
        }
    }
}
