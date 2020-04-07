using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;

namespace ImageToPDF.Converter
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

    class Powerpoint2Emf : ImageConverter
    {
        public override IEnumerable<ImageFileKind> GetApplicableImageFileKinds()
        {
            yield return ImageFileKind.Powerpoint;
        }

        public override IEnumerable<ImageFileKind> GetAvailableDestinationImageFileKinds()
        {
            yield return ImageFileKind.Emf;
        }

        protected override IEnumerable<DestinationFile> ConvertApplicableImageThenSave(
            SourceFile source,
            ImageFileKind destinationKind,
            ArgumentOptionCollection optionCollection,
            ILogger logger)
        {
            var savedImageFiles = new List<DestinationFile>();

            try
            {
                using (var powerPointApplication = new PowerPointApplication(new Application()))
                {
                    var application = powerPointApplication.Application;
                    var presentation = application.Presentations;
                    using (var presenationFile = new PowerPointPresentation(presentation.Open(
                        FileName: source.Path,
                        ReadOnly: MsoTriState.msoTrue,
                        Untitled: MsoTriState.msoTrue,
                        WithWindow: MsoTriState.msoTrue)))
                    {
                        var file = presenationFile.Presentation;
                        foreach (var slide in EnumeratePowerPointSlides(file))
                        {
                            if (!optionCollection.PowerpointPageRange.Contains((uint)slide.SlideNumber))
                            {
                                continue;
                            }
                            slide.Select();
                            slide.Shapes.SelectAll();
                            var selection = application.ActiveWindow.Selection;
                            var shapes = selection.ShapeRange;

                            var destinationPathForSlide = $"{Path.GetDirectoryName(source.Path)}\\{Path.GetFileNameWithoutExtension(source.Path)}{slide.SlideNumber}{destinationKind.GetExtensionWithDot()}";
                            var destinationForSlide = new DestinationFile(destinationPathForSlide);
                            if (!DeleteExistingFileIfNecessary(destinationForSlide, optionCollection, logger))
                            {
                                continue;
                            }

                            //create emf
                            shapes.Export(destinationForSlide.Path, PpShapeFormat.ppShapeFormatEMF);
                            savedImageFiles.Add(destinationForSlide);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.WriteLog($"Failed to convert powerpoint slide to emf images: {e.Message}\n{e.StackTrace}", LogLevel.Error);
            }

            return savedImageFiles;
        }

        private static IEnumerable<Slide> EnumeratePowerPointSlides(Presentation presentation)
        {
            foreach (var count in Enumerable.Range(1, presentation.Slides.Count))
            {
                yield return presentation.Slides[count];
            }
        }
    }
}
