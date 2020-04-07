using System;
using System.IO;

namespace ImageToPDF.Converter
{
    static class Common
    {
        public static bool DeleteExistingFileIfNecessary(DestinationFile destination, ArgumentOptionCollection optionCollection, ILogger logger)
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
