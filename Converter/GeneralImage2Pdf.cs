using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ImageToPDF.Converter
{
    class GeneralImage2Pdf : ImageConverter
    {
        public GeneralImage2Pdf() { }

        public override IEnumerable<ImageFileKind> GetApplicableImageFileKinds()
        {
            yield return ImageFileKind.Bmp;
            yield return ImageFileKind.Png;
            yield return ImageFileKind.Jpg;
            yield return ImageFileKind.Gif;
            yield return ImageFileKind.Tif;
            yield return ImageFileKind.Wmf;
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
            var image = Image.GetInstance(source.Path);
            var destination = source.GetConvertionDestination(destinationKind);

            if (!DeleteExistingFileIfNecessary(destination, optionCollection, logger))
            {
                yield break;
            }

            try
            {
                using (var fileStream = new FileStream(destination.Path, FileMode.Create, FileAccess.Write))
                {
                    var document = new Document(
                        pageSize: new Rectangle(image.Width, image.Height),
                        marginLeft: 0f,
                        marginRight: 0f,
                        marginTop: 0f,
                        marginBottom: 0f);
                    image.SetAbsolutePosition(0f, 0f);
                    PdfWriter.GetInstance(document, fileStream);
                    document.Open();
                    document.Add(image);
                    document.Close();
                }
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
