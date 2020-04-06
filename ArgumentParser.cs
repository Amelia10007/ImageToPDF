using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ImageToPDF
{


    class ArgumentParse
    {
    }
    static class ArgumentParser
    {
        public static IEnumerable<TaskCommand> ParseArguments(IEnumerable<string> args)
        {
            var directoryInfoExpansion = ExpandDirectoryInfo(args);
            var whetherOptions = AttachOptionAttribute(directoryInfoExpansion)
                .SkipWhile(item => item.isOption)
                .ToArray();
            while (whetherOptions.Any())
            {
                var filename = whetherOptions.First().arg;
                var options = whetherOptions
                    .Skip(1)
                    .TakeWhile(item => item.isOption)
                    .Select(item => item.arg)
                    .ToArray();
                yield return new TaskCommand(filename, options);
                whetherOptions = whetherOptions
                    .Skip(1 + options.Length)
                    .ToArray();
            }
        }
        private static readonly string recursiveEnumerationOption = "-r";
        private static IEnumerable<string> ExpandDirectoryInfo(IEnumerable<string> args)
        {
            foreach (var (current, hasNext, next) in args.WithNext())
            {
                if (current == recursiveEnumerationOption) continue;
                if (!Directory.Exists(current))
                {
                    yield return current;
                    continue;
                }
                var expandRecursively = hasNext && next == recursiveEnumerationOption;
                var searchOption = expandRecursively ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                var files = Directory.EnumerateFiles(current, "*", searchOption);
                foreach (var file in files)
                {
                    yield return file;
                }
            }
        }
        private static IEnumerable<(string arg, bool isOption)> AttachOptionAttribute(IEnumerable<string> args)
        {
            foreach (var arg in args)
            {
                var isOption = !File.Exists(arg);
                yield return (arg, isOption);
            }
        }
    }
}
