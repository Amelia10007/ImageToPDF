using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text;

namespace ImageToPDF
{
    class ImageFileConverter : PdfImageGenerator
    {
        public ImageFileConverter() { }

        public override bool IsValidFilename(string sourceFilename)
        {
            var extension = Path.GetExtension(sourceFilename).ToLower();
            return validExtensions.Contains(extension);
        }

        protected override IEnumerable<Image> GetPdfImageOfCorrectFilename(string correctSourceFilename)
        {
            yield return Image.GetInstance(correctSourceFilename);
        }

        private static readonly string[] validExtensions = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".wmf" };
    }
}
