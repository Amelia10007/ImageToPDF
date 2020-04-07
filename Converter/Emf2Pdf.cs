using System.Collections.Generic;

namespace ImageToPDF.Converter
{
    class Emf2Pdf : ImageConverter
    {
        private readonly Emf2Eps emf2Eps;
        private readonly Eps2Pdf eps2Pdf;

        public Emf2Pdf(Emf2Eps emf2Eps, Eps2Pdf eps2Pdf)
        {
            this.emf2Eps = emf2Eps;
            this.eps2Pdf = eps2Pdf;
        }

        public override IEnumerable<ImageFileKind> GetApplicableImageFileKinds()
        {
            yield return ImageFileKind.Emf;
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
            foreach (var tempEps in this.emf2Eps.ConvertThenSave(source, ImageFileKind.Eps, optionCollection, logger))
            {
                var tempEpsSource = tempEps.AsSourceFile();

                foreach (var savedImage in this.eps2Pdf.ConvertThenSave(tempEpsSource, destinationKind, optionCollection, logger))
                {
                    yield return savedImage;
                }
            }
        }
    }
}
