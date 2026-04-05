namespace CR.Utils.DataUtils
{
    public static class FileUtils
    {
        /// <summary>
        /// Formats the given file size into a human-readable string.
        /// </summary>
        /// <param name="fileSize">The file size in bytes.</param>
        /// <returns>A formatted string representing the file size.</returns>
        public static string FormatFileSize(this long fileSize)
        {
            const int kilobyte = 1024;
            const int megabyte = kilobyte * kilobyte;

            if (fileSize < kilobyte)
            {
                return $"{fileSize} Byte";
            }
            else if (fileSize < megabyte)
            {
                double sizeInKB = (double)fileSize / kilobyte;
                return $"{sizeInKB:N0} KB";
            }
            else
            {
                double sizeInMB = (double)fileSize / megabyte;
                return $"{sizeInMB:N0} MB";
            }
        }
    }
}
