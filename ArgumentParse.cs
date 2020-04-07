using System;
using System.Collections.Generic;
using System.IO;

namespace ImageToPDF
{
    class ArgumentParse
    {
        public readonly ArgumentOptionCollection OptionCollection;
        public readonly IReadOnlyList<SourceFile> SourceFiles;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentException"></exception>
        public ArgumentParse(string[] arguments, ILogger logger)
        {
            var tokens = ArgumentToken.ParseArgument(arguments);

            logger.WriteLog("Detecting options from arguments...", LogLevel.Debug);
            var (optionCollection, remainingTokens) = ArgumentOptionCollection.LoadArgumentOptionCollection(tokens, logger);

            logger.WriteLog("Searching source image files from arguments...", LogLevel.Debug);
            var sourceFiles = SearchSourceFiles(optionCollection, remainingTokens, logger);

            this.OptionCollection = optionCollection;
            this.SourceFiles = sourceFiles;

            logger.WriteLog("Finished parsing arguments", LogLevel.Debug);
        }

        private static IReadOnlyList<SourceFile> SearchSourceFiles(
            ArgumentOptionCollection optionCollection,
            IEnumerable<ArgumentToken> fileAndDirectoryToken,
            ILogger logger)
        {
            var sourceFiles = new List<SourceFile>();
            foreach (var token in fileAndDirectoryToken)
            {
                var path = token.Token;
                if (File.Exists(path))
                {
                    AddSourceFileToList(sourceFiles, path, logger);
                }
                else if (Directory.Exists(path))
                {
                    logger.WriteLog($"Searching directory {path}...", LogLevel.Debug);
                    var searchOption = optionCollection.EnableRecursiveDirectorySearch ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                    foreach (var filePath in Directory.EnumerateFiles(path, "*", searchOption))
                    {
                        AddSourceFileToList(sourceFiles, filePath, logger);
                    }
                }
                else
                {
                    logger.WriteLog($"\"{path}\": No such file or directory.", LogLevel.Error);
                }
            }

            return sourceFiles;
        }

        private static void AddSourceFileToList(IList<SourceFile> destination, string path, ILogger logger)
        {
            try
            {
                destination.Add(new SourceFile(path));
            }
            catch (InvalidOperationException e)
            {
                logger.WriteLog(e.Message, LogLevel.Error);
            }

            logger.WriteLog($"Add source image file \"{path}\"", LogLevel.Debug);
        }
    }
}
