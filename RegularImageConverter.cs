using System;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ImageToPDF
{
    class RegularImageConverter : IPdfImageGenerator
    {
        public RegularImageConverter() { }

        public bool IsValidCommand(TaskCommand command) =>
            validExtensions.Contains(Path.GetExtension(command.SourceFilename).ToLower())
            && !command.Options.Any();

        public void SaveAsPdf(TaskCommand command)
        {
            if (!this.IsValidCommand(command))
            {
                throw new ArgumentException("Invalid command.");
            }
            var image = Image.GetInstance(command.SourceFilename);
            var destination = command.GetOutputPath();
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

        private static readonly string[] validExtensions = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tif", ".tiff", ".wmf" };
    }
}
