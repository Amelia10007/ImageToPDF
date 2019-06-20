using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;

namespace ImageToPDF
{
    class EmfConverter : PdfImageGenerator
    {
        public override bool IsValidFilename(string sourceFilename)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<Image> GetPdfImageOfCorrectFilename(string correctSourceFilename)
        {
            throw new NotImplementedException();
        }
    }
}
