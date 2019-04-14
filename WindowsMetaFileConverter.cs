using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text;

namespace ImageToPDF
{
    class WindowsMetaFileConverter : PdfImageGenerator
    {
        public override bool IsValidFilename(string sourceFilename)
        {
            return Path.GetExtension(sourceFilename).ToLower() == ".wmf";
        }

        protected override IEnumerable<Image> GetPdfImageOfCorrectFilename(string correctSourceFilename)
        {
            yield return ImgWMF.GetInstance(correctSourceFilename);
        }
    }
}
