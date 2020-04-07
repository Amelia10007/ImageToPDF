using System;

namespace ImageToPDF
{
    class SourceFile
    {
        public readonly ImageFileKind ImageFileKind;
        public readonly string Path;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public SourceFile(string path)
        {
            this.ImageFileKind = ImageFileKind.FromExtension(System.IO.Path.GetExtension(path));
            this.Path = path;
        }

        public DestinationFile GetConvertionDestination(ImageFileKind destinationKind)
        {
            var path = System.IO.Path.GetFileNameWithoutExtension(this.Path);
            var extension = destinationKind.GetExtensionWithDot();
            var destinationPath = $"{path}{extension}";

            return new DestinationFile(destinationPath);
        }
    }
}
