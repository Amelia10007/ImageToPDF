using System;
using System.Collections.Generic;
using System.IO;

namespace ImageToPDF.Converter
{
    class Powerpoint2Pdf : ImageConverter
    {
        private readonly Powerpoint2Eps powerpoint2Eps;
        private readonly Eps2Pdf eps2Pdf;

        public Powerpoint2Pdf(Powerpoint2Eps powerpoint2Eps, Eps2Pdf eps2Pdf)
        {
            this.powerpoint2Eps = powerpoint2Eps;
            this.eps2Pdf = eps2Pdf;
        }

        public override IEnumerable<ImageFileKind> GetApplicableImageFileKinds()
        {
            yield return ImageFileKind.Powerpoint;
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
            foreach (var savedEps in this.powerpoint2Eps.ConvertThenSave(source, ImageFileKind.Eps, optionCollection, logger))
            {
                var sourceEps = savedEps.AsSourceFile();
                foreach (var savedPdf in this.eps2Pdf.ConvertThenSave(sourceEps, destinationKind, optionCollection, logger))
                {
                    yield return savedPdf;
                }

                // Delete tempolary EPS image
                try
                {
                    File.Delete(sourceEps.Path);
                }
                catch (Exception e)
                {
                    logger.WriteLog($"Failed to delete tempolary eps file: {e.Message}", LogLevel.Error);
                    continue;
                }
            }
        }
    }
}
