namespace ImageToPDF
{
    class SourceFile
    {
        public readonly ImageFileKind ImageFileKind;
        public readonly string Path;

        public SourceFile(string path)
        {
            this.ImageFileKind = ImageFileKind.FromExtension(System.IO.Path.GetExtension(path));
            this.Path = path;
        }
    }
}
