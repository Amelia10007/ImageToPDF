using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace ImageToPDF.Converter
{
    class Emf2Eps : ImageConverter
    {
        private readonly string emfToEpsPath;

        public Emf2Eps(string emfToEpsPath) => this.emfToEpsPath = emfToEpsPath;

        public override IEnumerable<ImageFileKind> GetApplicableImageFileKinds()
        {
            yield return ImageFileKind.Emf;
        }

        public override IEnumerable<ImageFileKind> GetAvailableDestinationImageFileKinds()
        {
            yield return ImageFileKind.Eps;
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
            argument.Append(source.Path);
            argument.Append($" {destination.Path}");
            logger.WriteLog($"EmfToEps path: {this.emfToEpsPath}", LogLevel.Debug);
            logger.WriteLog($"EmfToEps arg: {argument.ToString()}", LogLevel.Debug);

            var info = new ProcessStartInfo()
            {
                FileName = this.emfToEpsPath,
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
