using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageToPDF
{
    class ArgumentOptionCollection
    {
        public bool AllowDestinationOverwrite { get; private set; } = false;
        public bool EnableRecursiveDirectorySearch { get; private set; } = false;
        public ImageFileKind DestinationImageFileKind { get; private set; } = ImageFileKind.Pdf;
        public Range<uint> PowerpointPageRange { get; private set; } = new Range<uint>(0, uint.MaxValue);
        public bool EnableVersionInfoDisplay { get; private set; } = false;
        public LogLevel LogLevel { get; private set; } = LogLevel.Info;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argumentTokens"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static (ArgumentOptionCollection argumentOptionCollection, IReadOnlyList<ArgumentToken> remainingTokens) LoadArgumentOptionCollection(IEnumerable<ArgumentToken> argumentTokens, ILogger logger)
        {
            var argumentOptionCollection = new ArgumentOptionCollection();
            var remainingTokens = argumentTokens.ToList();

            while (remainingTokens.Any() && IsOptionDeclarationToken(remainingTokens[0]))
            {
                switch (remainingTokens[0].Token)
                {
                    case "-w":
                    case "--allow-overwrite":
                        argumentOptionCollection.AllowDestinationOverwrite = true;
                        remainingTokens.RemoveAt(0);
                        break;
                    case "-r":
                    case "--recursive":
                        argumentOptionCollection.EnableRecursiveDirectorySearch = true;
                        remainingTokens.RemoveAt(0);
                        break;
                    case "-t":
                    case "--output-type":
                        if (remainingTokens.Count < 2)
                        {
                            logger.WriteLog("Output type must be specified after \"-t\" or \"--output-type\" option.", LogLevel.Error);
                            goto err;
                        }
                        try
                        {
                            argumentOptionCollection.DestinationImageFileKind = ImageFileKind.FromExtension(remainingTokens[1].Token);
                        }
                        catch (InvalidOperationException)
                        {
                            logger.WriteLog($"Invalid output type: {remainingTokens[1].Token}", LogLevel.Error);
                            goto err;
                        }
                        remainingTokens.RemoveRange(0, 2);
                        break;
                    case "-p":
                    case "--powerpoint-page-range":
                        if (remainingTokens.Count < 3)
                        {
                            logger.WriteLog("Start and end page num must be specified after \"-p\" or \"--powerpoint-page-range\" option.", LogLevel.Error);
                            goto err;
                        }
                        try
                        {
                            var startPageNum = uint.Parse(remainingTokens[1].Token);
                            var endPageNum = uint.Parse(remainingTokens[2].Token);
                            var range = new Range<uint>(startPageNum, endPageNum);
                            argumentOptionCollection.PowerpointPageRange = range;
                        }
                        catch (Exception)
                        {
                            logger.WriteLog($"Invalid page num specified: {remainingTokens[1].Token}, {remainingTokens[2].Token}", LogLevel.Error);
                            goto err;
                        }
                        remainingTokens.RemoveRange(0, 3);
                        break;
                    case "-v":
                    case "--version":
                        argumentOptionCollection.EnableVersionInfoDisplay = true;
                        remainingTokens.RemoveAt(0);
                        break;
                    case "-L":
                    case "--log-level":
                        if (remainingTokens.Count < 2)
                        {
                            logger.WriteLog("Logger level must be specified after \"-L\" or \"--show-log\" option.", LogLevel.Error);
                            goto err;
                        }
                        try
                        {
                            var logLevel = Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>().Single(level => level.ToString().ToLower() == remainingTokens[1].Token);
                            argumentOptionCollection.LogLevel = logLevel;
                        }
                        catch (InvalidOperationException)
                        {
                            logger.WriteLog($"Invalid output type: {remainingTokens[1].Token}", LogLevel.Error);
                            goto err;
                        }
                        remainingTokens.RemoveRange(0, 2);
                        break;
                    default:
                        logger.WriteLog($"Unexpected option \"{remainingTokens[0].Token}\"", LogLevel.Error);
                        goto err;
                }
            }

            return (argumentOptionCollection, remainingTokens);

        err:
            throw new ArgumentException("Incorrect option argument. See log in detail.");
        }

        private static bool IsOptionDeclarationToken(ArgumentToken token) => token.Token.StartsWith("-");
    }
}
