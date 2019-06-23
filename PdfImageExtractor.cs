using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ImageToPDF
{
    class PdfImageExtractor : IPdfImageGenerator
    {
        public bool IsValidCommand(TaskCommand command) =>
            Path.GetExtension(command.SourceFilename).ToLower() == ".pdf"
            && !command.Options.Any();

        public void SaveAsPdf(TaskCommand command)
        {
            if (!this.IsValidCommand(command))
            {
                throw new ArgumentException("Invalid command.");
            }
            var images = this.GetPdfImages(command).WithIndex();
            foreach (var (image, index) in images)
            {
                var destination = command.GetOutputPath(suffix: (index + 1).ToString());
                using (var fileStream = new FileStream(destination, FileMode.Create, FileAccess.Write))
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
        }

        private IEnumerable<Image> GetPdfImages(TaskCommand command)
        {
            var reader = new PdfReader(command.SourceFilename);
            foreach (var page in GetPdfPages(reader))
            {
                var objects = this.GetPdfObjectsInPage(page);
                if (objects == null) continue;
                var imagesInPdf = this.GetImagesInPdf(objects);
                foreach (var imageInPdf in imagesInPdf)
                {
                    var bytes = this.GetRawBytesOfImage(reader, imageInPdf);
                    var image = Image.GetInstance(bytes);
                    yield return image;
                }
            }
        }

        private static IEnumerable<PdfDictionary> GetPdfPages(PdfReader reader)
        {
            foreach (var page in Enumerable.Range(1, reader.NumberOfPages))
                yield return reader.GetPageN(page);
        }

        private PdfDictionary GetPdfObjectsInPage(PdfDictionary pdfPage)
        {
            var resource = PdfReader.GetPdfObject(pdfPage.Get(PdfName.RESOURCES)) as PdfDictionary;
            return PdfReader.GetPdfObject(resource.Get(PdfName.XOBJECT)) as PdfDictionary;
        }

        private IEnumerable<PdfObject> GetImagesInPdf(PdfDictionary pdfDictionary)
        {
            foreach (var pdfObject in pdfDictionary)
            {
                var obj = pdfDictionary.Get(pdfObject.Key);
                if (!obj.IsIndirect()) continue;
                var tg = PdfReader.GetPdfObject(obj) as PdfDictionary;
                var type = PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE)) as PdfName;
                if (!type.Equals(PdfName.IMAGE)) continue;
                yield return obj;
            }
        }

        private byte[] GetRawBytesOfImage(PdfReader reader, PdfObject pdfObject)
        {
            var index = Convert.ToInt32((pdfObject as PdfIndirectReference).Number.ToString());
            var stream = reader.GetPdfObject(index) as PRStream;
            return PdfReader.GetStreamBytesRaw(stream);
        }
    }
}
