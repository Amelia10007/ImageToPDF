using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageToPDF
{
    interface IImageConverter
    {
        IEnumerable<ImageFileKind> GetApplicableImageFileKinds();
        IEnumerable<ImageFileKind> GetAvailableDestinationImageFileKinds();
        DestinationFile ConvertThenSave(SourceFile source, ImageFileKind imageFileKind, ILogger logger);
    }
}
