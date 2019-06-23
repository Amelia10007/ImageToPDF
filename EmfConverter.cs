using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ImageToPDF
{
    class EmfConverter : IPdfImageGenerator
    {
        public EmfConverter(EpsConverter epsConverter) => this.epsConverter = epsConverter;
        public bool IsValidCommand(TaskCommand command) =>
            Path.GetExtension(command.SourceFilename).ToLower() == ".emf"
            && !command.Options.Any();

        public void SaveAsPdf(TaskCommand command)
        {
            if (!this.IsValidCommand(command))
            {
                throw new ArgumentException("Invaid command.");
            }
            //create eps from emf
            var epsDestination = Path.GetTempFileName() + ".eps";
            var input = command.SourceFilename;
            var argument = new StringBuilder();
            argument.Append(input);
            argument.Append($" {epsDestination}");
            var info = new ProcessStartInfo()
            {
                FileName = ApplicationSetting.Of("EmfConverterPath"),
                Arguments = argument.ToString(),
                WindowStyle = ProcessWindowStyle.Hidden,
            };
            Process.Start(info).WaitForExit();
            //create pdf from eps
            var epsConvertionCommand = new TaskCommand(epsDestination);
            this.epsConverter.SaveAsPdf(epsConvertionCommand);
            var pdfPath = epsConvertionCommand.GetOutputPath();
            //move the pdf file
            File.Move(pdfPath, command.GetOutputPath());
            //delete eps
            File.Delete(epsDestination);
        }

        private readonly EpsConverter epsConverter;
    }
}
