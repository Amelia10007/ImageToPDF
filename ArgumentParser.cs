using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ImageToPDF
{
    static class ArgumentParser
    {
        private class ParsedArgument
        {
            public readonly string SourceName;
            public readonly bool IsFile;
            public readonly IReadOnlyCollection<string> Options;
            public ParsedArgument(string sourceName, bool isFile, IEnumerable<string> options)
            {
                this.SourceName = sourceName;
                this.IsFile = isFile;
                this.Options = options.ToArray();
            }
        }
        public static IEnumerable<TaskCommand> ParseArguments(IEnumerable<string> args)
        {
            var tempCommands = CreateTemporaryCommands(args);
            foreach (var tempCommand in tempCommands)
            {
                //file
                if (tempCommand.IsFile)
                {
                    yield return new TaskCommand(tempCommand.SourceName, tempCommand.Options);
                    continue;
                }
                //directory
                var searchOption = (tempCommand.Options.Any() && tempCommand.Options.First() == recursiveEnumerationOption)
                    ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                var files = Directory.EnumerateFiles(tempCommand.SourceName, "*", searchOption);
                var remainingOptions = tempCommand.Options.Skip(1).ToArray();
                foreach (var file in files)
                {
                    yield return new TaskCommand(file, remainingOptions);
                }
            }
        }
        private static readonly string recursiveEnumerationOption = "-r";
        private static IEnumerable<ParsedArgument> CreateTemporaryCommands(IEnumerable<string> args)
        {
            string tempSourceName = null;
            var tempOptions = new List<string>();
            foreach (var arg in args)
            {
                var hasSourceNameRead = tempSourceName != null;
                if (!hasSourceNameRead)
                {
                    tempSourceName = arg;
                    continue;
                }
                var isOption = !(File.Exists(arg) || Directory.Exists(arg));
                if (isOption)
                {
                    tempOptions.Add(arg);
                    continue;
                }
                var isFile = File.Exists(tempSourceName);
                yield return new ParsedArgument(tempSourceName, isFile, tempOptions);
                tempSourceName = null;
                tempOptions = new List<string>();
            }
            if (tempSourceName != null)
            {
                var isFile = File.Exists(tempSourceName);
                yield return new ParsedArgument(tempSourceName, isFile, tempOptions);
            }
        }
    }
}
