using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace ImageToPDF
{
    class TaskCommand
    {
        public readonly string SourceFilename;
        public readonly IReadOnlyCollection<string> Options;
        public TaskCommand(string sourceFilename) : this(sourceFilename, Enumerable.Empty<string>()) { }
        public TaskCommand(string sourceFilename, IEnumerable<string> options)
        {
            this.SourceFilename = sourceFilename;
            this.Options = options.ToArray();
        }
        public string GetOutputPath(string extension = "pdf", string suffix = "")
        {
            var directory = Path.GetDirectoryName(this.SourceFilename);
            var file = $"{Path.GetFileNameWithoutExtension(this.SourceFilename)}{suffix}.{extension}";
            return $@"{directory}\{file}";
        }
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(this.SourceFilename);
            foreach (var option in this.Options)
                builder.Append($" {option}");
            return builder.ToString();
        }
    }
}
