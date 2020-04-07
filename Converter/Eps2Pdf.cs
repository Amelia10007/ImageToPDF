using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace ImageToPDF.Converter
{
    class Eps2Pdf : ImageConverter
    {
        private readonly string ghostScriptPath;

        public Eps2Pdf(string ghostScriptPath) => this.ghostScriptPath = ghostScriptPath;

        public override IEnumerable<ImageFileKind> GetApplicableImageFileKinds()
        {
            yield return ImageFileKind.Eps;
        }

        public override IEnumerable<ImageFileKind> GetAvailableDestinationImageFileKinds()
        {
            yield return ImageFileKind.Pdf;
        }

        protected override IEnumerable<DestinationFile> ConvertApplicableImageThenSave(
            SourceFile source,
            ImageFileKind destinationKind,
            ArgumentOptionCollection optionCollection,
            ILogger logger)
        {
            var destination = source.GetConvertionDestination(destinationKind);

            if (!DeleteExistingFileIfNecessary(destination, optionCollection, logger))
            {
                yield break;
            }

            var argument = new StringBuilder();
            argument.Append("-sDEVICE=pdfwrite");
            argument.Append(" -dEPSCrop");
            argument.Append($" -o {destination.Path} ");
            argument.Append(source.Path);
            logger.WriteLog($"Ghostscript path: {this.ghostScriptPath}", LogLevel.Debug);
            logger.WriteLog($"Ghostscript arg: {argument.ToString()}", LogLevel.Debug);

            var info = new ProcessStartInfo()
            {
                FileName = ghostScriptPath,
                Arguments = argument.ToString(),
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            try
            {
                Process.Start(info).WaitForExit();
            }
            catch (Exception e)
            {
                logger.WriteLog($"Failed to convert an image: {e.Message}", LogLevel.Error);
                yield break;
            }

            yield return destination;
        }
    }
}
