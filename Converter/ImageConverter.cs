using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ImageToPDF.Converter
{
    abstract class ImageConverter
    {
        public abstract IEnumerable<ImageFileKind> GetApplicableImageFileKinds();

        public abstract IEnumerable<ImageFileKind> GetAvailableDestinationImageFileKinds();

        public IEnumerable<DestinationFile> ConvertThenSave(SourceFile source, ImageFileKind destinationKind, ArgumentOptionCollection optionCollection, ILogger logger)
        {
            if (!this.GetApplicableImageFileKinds().Contains(source.ImageFileKind))
            {
                logger.WriteLog($"Unsuitable source image kind: {source.ImageFileKind}", LogLevel.Error);
                yield break;
            }
            if (!this.GetAvailableDestinationImageFileKinds().Contains(destinationKind))
            {
                logger.WriteLog($"Unsuitable source image kind: {destinationKind}", LogLevel.Error);
                yield break;
            }

            foreach (var savedDestination in this.ConvertApplicableImageThenSave(source, destinationKind, optionCollection, logger)){
                yield return savedDestination;
            }
        }

        protected abstract IEnumerable<DestinationFile> ConvertApplicableImageThenSave(
            SourceFile source,
            ImageFileKind destinationKind,
            ArgumentOptionCollection optionCollection,
            ILogger logger);

        protected static bool DeleteExistingFileIfNecessary(DestinationFile destination, ArgumentOptionCollection optionCollection, ILogger logger)
        {
            try
            {
                if (File.Exists(destination.Path))
                {
                    if (optionCollection.AllowDestinationOverwrite)
                    {
                        File.Delete(destination.Path);
                        logger.WriteLog($"A destination image {destination.Path} has existed so been deleted.", LogLevel.Warn);
                    }
                    else
                    {
                        logger.WriteLog($"A destination image {destination.Path} has existed.", LogLevel.Error);
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                logger.WriteLog($"Cannot convert an image: {e.Message}", LogLevel.Error);
                return false;
            }

            return true;
        }
    }
}
