using System;
using System.Collections.Generic;
using System.IO;

namespace ImageToPDF.Converter
{
    class Powerpoint2Eps : ImageConverter
    {
        private readonly Powerpoint2Emf powerpoint2Emf;
        private readonly Emf2Eps emf2Eps;

        public Powerpoint2Eps(Powerpoint2Emf powerpoint2Emf, Emf2Eps emf2Eps)
        {
            this.powerpoint2Emf = powerpoint2Emf;
            this.emf2Eps = emf2Eps;
        }

        public override IEnumerable<ImageFileKind> GetApplicableImageFileKinds()
        {
            yield return ImageFileKind.Powerpoint;
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
            foreach (var savedEmf in this.powerpoint2Emf.ConvertThenSave(source, ImageFileKind.Emf, optionCollection, logger))
            {
                var sourceEmf = savedEmf.AsSourceFile();
                foreach (var savedEps in this.emf2Eps.ConvertThenSave(sourceEmf, destinationKind, optionCollection, logger))
                {
                    yield return savedEps;
                }

                // Delete tempolary EMF image
                try
                {
                    File.Delete(sourceEmf.Path);
                }
                catch (Exception e)
                {
                    logger.WriteLog($"Failed to delete tempolary emf file: {e.Message}", LogLevel.Error);
                    continue;
                }
            }
        }
    }
}
