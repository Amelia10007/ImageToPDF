using System;
using System.Linq;

namespace ImageToPDF
{
    class ImageFileKind : IEquatable<ImageFileKind>
    {
        private readonly Id id;
        private readonly string[] applicableExtensions;

        public static readonly ImageFileKind Bmp = new ImageFileKind("bmp");
        public static readonly ImageFileKind Png = new ImageFileKind("png");
        public static readonly ImageFileKind Jpg = new ImageFileKind("jpg", "jpeg");
        public static readonly ImageFileKind Gif = new ImageFileKind("gif");
        public static readonly ImageFileKind Tif = new ImageFileKind("tif", "tiff");
        public static readonly ImageFileKind Wmf = new ImageFileKind("wmf");
        public static readonly ImageFileKind Emf = new ImageFileKind("emf");
        public static readonly ImageFileKind Eps = new ImageFileKind("eps");
        public static readonly ImageFileKind Pdf = new ImageFileKind("pdf");
        public static readonly ImageFileKind Powerpoint = new ImageFileKind("ppt", "pptx");

        public static readonly ImageFileKind[] Kinds = new[] { Bmp, Png, Jpg, Gif, Tif, Wmf, Emf, Eps, Pdf, Powerpoint };

        private ImageFileKind(params string[] applicableExtensions)
        {
            this.id = Id.GetNext();
            this.applicableExtensions = applicableExtensions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static ImageFileKind FromExtension(string extension) => Kinds.Single(kind => kind.applicableExtensions.Any(a => extension.Trim('.').ToLower() == a));

        public string GetExtensionWithDot() => $".{this.applicableExtensions.First().ToLower()}";

        public bool Equals(ImageFileKind other) => this.id.Equals(other.id);

        public override bool Equals(object obj) => obj is ImageFileKind other && this.Equals(other);

        public override int GetHashCode() => this.id.GetHashCode();
    }
}
