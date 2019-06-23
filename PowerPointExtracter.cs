using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;

namespace ImageToPDF
{
    class PowerPointApplication : IDisposable
    {
        public readonly Application Application;
        public PowerPointApplication(Application application) => this.Application = application;
        public void Dispose() => this.Application?.Quit();
    }

    class PowerPointPresentation : IDisposable
    {
        public readonly Presentation Presentation;
        public PowerPointPresentation(Presentation presentation) => this.Presentation = presentation;
        public void Dispose() => this.Presentation?.Close();
    }

    class PowerPointExtracter : IPdfImageGenerator
    {
        public PowerPointExtracter(EmfConverter emfConverter) => this.emfConverter = emfConverter;
        public bool IsValidCommand(TaskCommand command)
        {
            var extensionCondition = validExtensions.Contains(Path.GetExtension(command.SourceFilename).ToLower());
            var optionCondition = this.ParseOption(command.Options).valid;
            return extensionCondition && optionCondition;
        }

        public void SaveAsPdf(TaskCommand command)
        {
            if (!this.IsValidCommand(command))
            {
                throw new ArgumentException("Invalid command.");
            }
            var (_, startPage, endPage) = this.ParseOption(command.Options);

            using (var powerPointApplication = new PowerPointApplication(new Application()))
            {
                var application = powerPointApplication.Application;
                var presentation = application.Presentations;
                using (var presenationFile = new PowerPointPresentation(presentation.Open(
                    FileName: command.SourceFilename,
                    ReadOnly: MsoTriState.msoTrue,
                    Untitled: MsoTriState.msoTrue,
                    WithWindow: MsoTriState.msoTrue)))
                {
                    var file = presenationFile.Presentation;
                    foreach (var slide in EnumeratePowerPointSlides(file))
                    {
                        if (slide.SlideNumber < startPage) continue;
                        if (slide.SlideNumber > endPage) break;
                        slide.Select();
                        slide.Shapes.SelectAll();
                        var selection = application.ActiveWindow.Selection;
                        var shapes = selection.ShapeRange;
                        //create emf
                        var emfFilename = Path.GetTempFileName() + ".emf";
                        shapes.Export(emfFilename, PpShapeFormat.ppShapeFormatEMF);
                        //convert emf to pdf
                        var emfConvertionCommand = new TaskCommand(emfFilename);
                        this.emfConverter.SaveAsPdf(emfConvertionCommand);
                        //move the pdf file
                        var pdfPath = emfConvertionCommand.GetOutputPath();
                        File.Move(pdfPath, command.GetOutputPath(suffix: slide.SlideNumber.ToString()));
                        //delete emf
                        File.Delete(emfFilename);
                    }
                }
            }
        }

        private (bool valid, int startPage, int endPage) ParseOption(IReadOnlyCollection<string> options)
        {
            switch (options.Count)
            {
                case 0:
                    return (true, 1, int.MaxValue);
                case 1:
                    if (int.TryParse(options.First(), out var page))
                        return (true, page, page);
                    else
                        return (false, 0, 0);
                case 2:
                    if (int.TryParse(options.First(), out var start)
                        && int.TryParse(options.Last(), out var end))
                        return (true, start, end);
                    else
                        return (false, 0, 0);
                default:
                    return (false, 0, 0);
            }
        }

        private static IEnumerable<Slide> EnumeratePowerPointSlides(Presentation presentation)
        {
            foreach (var count in Enumerable.Range(1, presentation.Slides.Count))
            {
                yield return presentation.Slides[count];
            }
        }

        private readonly EmfConverter emfConverter;
        private static readonly string[] validExtensions = new[] { ".ppt", ".pptx" };
    }
}
