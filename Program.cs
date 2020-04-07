using System;
using System.Collections.Generic;
using System.Linq;
using ImageToPDF.Converter;

namespace ImageToPDF
{
    static class Program
    {
        static void Main(string[] args)
        {
            var imageConverterImpls = InstantiateImageConverterImpls();

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
            // 変換元のファイルやディレクトリがひとつも指定されなかった場合、このプログラムの使いかたを表示して終了
            if (!parsedArgument.SourceFiles.Any())
            {
                PrintUsage();
                return;
            }

            foreach (var sourceFile in parsedArgument.SourceFiles)
            {
                logger.WriteLog($"Set source image file: {sourceFile.Path}", LogLevel.Debug);
            }

            var destinationKind = parsedArgument.OptionCollection.DestinationImageFileKind;

            foreach (var sourceFile in parsedArgument.SourceFiles)
            {
                logger.WriteLog($"Source image file: {sourceFile.Path}", LogLevel.Info);

                var imageConverter = imageConverterImpls.SingleOrDefault(impl =>
                      impl.GetApplicableImageFileKinds().Contains(sourceFile.ImageFileKind)
                      && impl.GetAvailableDestinationImageFileKinds().Contains(destinationKind));
                if (imageConverter == null)
                {
                    logger.WriteLog($"No suitable converter found. source image kind: {sourceFile.ImageFileKind}, destination image kind: {destinationKind}", LogLevel.Error);
                    continue;
                }
                else
                {
                    logger.WriteLog("Found a converter.", LogLevel.Debug);
                }

                foreach (var savedImage in imageConverter.ConvertThenSave(sourceFile, destinationKind, parsedArgument.OptionCollection, logger))
                {
                    logger.WriteLog($"Convertion {savedImage.Path} done.", LogLevel.Info);
                }
            }
        }

        private static void PrintVersionInfo()
        {
            Console.WriteLine("1.1");
        }

        private static void PrintUsage()
        {
            Console.WriteLine("ImageToPDF");
            PrintVersionInfo();

            Console.WriteLine("Usage: ImageToPDF.exe [-w|-r|-t type|-p start end|-v|-L loglevel] [FILES|DIRECTORIES]");
        }

        private static IReadOnlyCollection<ImageConverter> InstantiateImageConverterImpls()
        {
            var ghostScriptPath = ApplicationSetting.Of("EpsConverterPath");
            var emfToEpsPath = ApplicationSetting.Of("EmfConverterPath");

            var generalImage2pdf = new GeneralImage2Pdf();
            var eps2pdf = new Eps2Pdf(ghostScriptPath);
            var emf2eps = new Emf2Eps(emfToEpsPath);
            var emf2pdf = new Emf2Pdf(emf2eps, eps2pdf);
            var powerpoint2emf = new Powerpoint2Emf();
            var powerpoint2eps = new Powerpoint2Eps(powerpoint2emf, emf2eps);
            var powerpoint2pdf = new Powerpoint2Pdf(powerpoint2eps, eps2pdf);

            return new ImageConverter[] { generalImage2pdf, eps2pdf, emf2eps, emf2pdf, powerpoint2emf, powerpoint2eps, powerpoint2pdf };
        }
    }
}
