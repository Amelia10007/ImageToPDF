using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ImageToPDF
{
    abstract class PdfImageGenerator
    {
        public abstract bool IsValidFilename(string sourceFilename);

        public abstract void SaveAsPdf(TaskCommand command);

        protected abstract IEnumerable<Image> GetPdfImageOfCorrectFilename(string correctSourceFilename);

        protected void SaveImageAsPdf(Image image, string destination)
        {
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
}
