using System;
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

        public override void SaveAsPdf(TaskCommand command)
        {
            if (command.Options.Any())
            {
                throw new ArgumentException("Invalid argument.");
            }
            var image = Image.GetInstance(command.SourceFilename);
            var destination = $"{Path.GetFileNameWithoutExtension(command.SourceFilename)}.pdf";
            this.SaveImageAsPdf(image, destination);
        }

        private static readonly string[] validExtensions = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".wmf" };
    }
}
