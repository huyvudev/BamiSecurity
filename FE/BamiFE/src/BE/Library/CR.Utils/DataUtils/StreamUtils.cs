namespace CR.Utils.DataUtils
{
    public static class StreamUtils
    {
        /// <summary>
        /// Copy ra stream khác
        /// </summary>
        /// <param name="streamFile"></param>
        /// <returns></returns>
        public static async Task<Stream> CopyToMemoryStream(this Stream streamFile)
        {
            var ms = new MemoryStream();
            streamFile.Position = 0;
            await streamFile.CopyToAsync(ms);
            return ms;
        }
    }
}
