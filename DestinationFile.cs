namespace ImageToPDF
{
    class DestinationFile
    {
        public readonly ImageFileKind ImageFileKind;
        public readonly string Path;

        public DestinationFile(string path)
        {
            this.ImageFileKind = ImageFileKind.FromExtension(System.IO.Path.GetExtension(path));
            this.Path = path;
        }
    }
}
