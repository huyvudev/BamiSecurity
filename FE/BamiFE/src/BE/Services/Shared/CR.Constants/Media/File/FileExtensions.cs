namespace CR.Constants.Media.File
{
    public static class FileExtensions
    {
        // Video
        public const string Mp4 = ".mp4";
        public const string Mov = ".mov";

        // Hình ảnh
        public const string Jpg = ".jpg";
        public const string Jpeg = ".jpeg";
        public const string Png = ".png";
        public const string Bmp = ".bmp";
        public const string Svg = ".svg";
        public const string Webp = ".webp";
        public const string Jfif = ".jfif";

        // Văn bản
        public const string Txt = ".txt";
        public const string Doc = ".doc";
        public const string Docx = ".docx";
        public const string Pdf = ".pdf";

        // Bảng tính
        public const string Xls = ".xls";
        public const string Xlsx = ".xlsx";

        public static readonly string[] ValidExtensions = new string[]
        {
            Mp4,
            Mov,
            Jpg,
            Jpeg,
            Png,
            Webp,
            Bmp,
            Svg,
            Jfif,
            Txt,
            Doc,
            Docx,
            Pdf,
            Xls,
            Xlsx,
        };

        public static readonly string[] VideoExtensions = new string[] { Mp4, Mov, };

        public static readonly string[] ImageExtensions = new string[]
        {
            Jpg,
            Jpeg,
            Png,
            Webp,
            Bmp,
            Svg,
            Jfif
        };

        public static readonly string[] DocumentExtensions = new string[]
        {
            Txt,
            Doc,
            Docx,
            Pdf,
            Xls,
            Xlsx,
        };
    }
}
