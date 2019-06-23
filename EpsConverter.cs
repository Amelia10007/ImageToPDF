using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ImageToPDF
{
    class EpsConverter : IPdfImageGenerator
    {
        public bool IsValidCommand(TaskCommand command) =>
            Path.GetExtension(command.SourceFilename).ToLower() == ".eps"
            && !command.Options.Any();

        public void SaveAsPdf(TaskCommand command)
        {
            if (!this.IsValidCommand(command))
            {
                throw new ArgumentException("Invaid command.");
            }
            var ghostScriptPath = ApplicationSetting.Of("EpsConverterPath");
            var input = command.SourceFilename;
            var output = command.GetOutputPath();
            var argument = new StringBuilder();
            argument.Append("-sDEVICE=pdfwrite");
            argument.Append(" -dEPSCrop");
            argument.Append($" -o {output} ");
            argument.Append(input);
            var info = new ProcessStartInfo()
            {
                FileName = ghostScriptPath,
                Arguments = argument.ToString(),
                WindowStyle = ProcessWindowStyle.Hidden,
            };
            Process.Start(info).WaitForExit();
        }
    }
}
