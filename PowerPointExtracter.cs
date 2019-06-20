using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using iTextSharp.text;
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

    class PowerPointExtracter : PdfImageGenerator
    {
        public override bool IsValidFilename(string sourceFilename)
        {
            var extension = Path.GetExtension(sourceFilename).ToLower();
            return validExtensions.Contains(extension);
        }

        protected override IEnumerable<Image> GetPdfImageOfCorrectFilename(string correctSourceFilename)
        {
            using (var powerPointApplication = new PowerPointApplication(new Application()))
            {
                var application = powerPointApplication.Application;
                var presentation = application.Presentations;
                using (var presenationFile = new PowerPointPresentation(presentation.Open(
                    FileName: correctSourceFilename,
                    ReadOnly: MsoTriState.msoTrue,
                    Untitled: MsoTriState.msoTrue,
                    WithWindow: MsoTriState.msoTrue)))
                {
                    var file = presenationFile.Presentation;
                    foreach (var slide in EnumeratePowerPointSlides(file))
                    {
                        slide.Select();
                        slide.Shapes.SelectAll();
                        var selection = application.ActiveWindow.Selection;
                        var shapes = selection.ShapeRange;
                        var tempFilename = Path.GetTempFileName();
                        shapes.Export(tempFilename, PpShapeFormat.ppShapeFormatWMF);
                        yield return Image.GetInstance(tempFilename);
                        File.Delete(tempFilename);
                    }
                }
            }
        }

        private static IEnumerable<Slide> EnumeratePowerPointSlides(Presentation presentation)
        {
            foreach (var count in Enumerable.Range(1, presentation.Slides.Count))
            {
                yield return presentation.Slides[count];
            }
        }

        public override void SaveAsPdf(TaskCommand command)
        {
            int startPage, endPage;
            switch (command.Options.Count)
            {
                case 0:
                    startPage = 1;
                    endPage = int.MaxValue;
                    break;
                case 1:
                    startPage = endPage = int.Parse(command.Options.First());
                    break;
                case 2:
                    startPage = int.Parse(command.Options.First());
                    endPage = int.Parse(command.Options.Last());
                    break;
                default: throw new ArgumentException("Invalid argument.");
            }
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
                        var tempFilename = Path.GetTempFileName();
                        shapes.Export(tempFilename, PpShapeFormat.ppShapeFormatEMF);
                        var destination = $"{Path.GetFileName(command.SourceFilename)}-{slide.SlideNumber}.pdf";
                        File.Delete(tempFilename);
                    }
                }
            }
        }

        private static readonly string[] validExtensions = new[] { ".ppt", ".pptx" };
    }
}
