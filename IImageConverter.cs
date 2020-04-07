using System.Collections.Generic;

namespace ImageToPDF
{
    interface IImageConverter
    {
        IEnumerable<ImageFileKind> GetApplicableImageFileKinds();
        IEnumerable<ImageFileKind> GetAvailableDestinationImageFileKinds();
        DestinationFile ConvertThenSave(SourceFile source, DestinationFile destination, ILogger logger);
    }
}
