using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageToPDF
{
    class TaskCommand
    {
        public readonly string SourceFilename;
        public readonly IReadOnlyCollection<string> Options;
        private TaskCommand(string source, IEnumerable<string> options)
        {
            this.SourceFilename = source;
            this.Options = options.ToArray();
        }
        public static IEnumerable<TaskCommand> ToCommands(string[] args)
        {
            var filenameIndexes = args.AllIndexesOf(arg => IsFilename(arg)).ToArray();
            foreach (var (index, hasNext, nextIndex) in filenameIndexes.WithNext())
            {
                var source = args[index];
                var optionFrom = index + 1;
                var optionTo = hasNext ? (nextIndex - 1) : (args.Length - 1);
                var options = args.FromTo(optionFrom, optionTo);
                yield return new TaskCommand(source, options);
            }
        }
        private static bool IsFilename(string arg)
        {
            var index = arg.LastIndexOf('.');
            //ファイル名と拡張子が1文字以上あれば，有効なファイル名であると判定
            return index > 1 && index < arg.Length - 1;
        }
    }
}
