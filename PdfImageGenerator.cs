using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ImageToPDF
{
    abstract class PdfImageGenerator
    {
        private string curretSourceFilename;

        public abstract bool IsValidFilename(string sourceFilename);

        public void WritePdfImage(string sourceFilename)
        {
            if (!this.IsValidFilename(sourceFilename))
            {
                throw new InvalidOperationException($"Cannot convert {sourceFilename} to PDF.");
            }
            this.curretSourceFilename = sourceFilename;
            var images = this.GetPdfImageOfCorrectFilename(sourceFilename);
            foreach (var image in images)
            {
                this.SaveImageAsPdf(image);
            }
        }

        protected abstract IEnumerable<Image> GetPdfImageOfCorrectFilename(string correctSourceFilename);

        private void SaveImageAsPdf(Image image)
        {
            var pdfPath = this.GetIdentifiablePdfPath();
            using (var fileStream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write))
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

        private string GetIdentifiablePdfPath()
        {
            var directory = Path.GetDirectoryName(this.curretSourceFilename);
            var prefix = Path.GetFileNameWithoutExtension(this.curretSourceFilename);
            for (int i = 0; ; i++)
            {
                var suffix = (i == 0) ? "" : $"({i})";
                var candidate = $"{directory}/{prefix}{suffix}.pdf";
                if (!File.Exists(candidate)) return candidate;
            }
        }
    }
}
