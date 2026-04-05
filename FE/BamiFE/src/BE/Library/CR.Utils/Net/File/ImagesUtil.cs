using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace CR.Utils.Net.File
{
    /// <summary>
    /// Các hàm tiện ích cho ảnh
    /// </summary>
    public class ImageUtils
    {
        /// <summary>
        /// Độ rộng tối đa của ảnh
        /// </summary>
        public const int MAX_WIDTH = 1920;

        /// <summary>
        /// Resize ảnh và lưu theo đường dẫn
        /// </summary>
        /// <param name="file"></param>
        /// <param name="resultPath">Đường dẫn lưu</param>
        public static void ResizeImage(IFormFile file, string resultPath)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            ms.Position = 0;
            var image = Image.Load(ms);
            int originalWidth = Math.Max(image.Width, image.Height);

            var jpegEncoder = new JpegEncoder { Quality = 75 }; // Điều chỉnh mức độ nén nếu là ảnh jpeg
            if (originalWidth <= MAX_WIDTH)
            {
                //image.Save(resultPath, jpegEncoder);
                image.SaveAsWebp(resultPath);
                return;
            }
            double resizeRatio = (double)MAX_WIDTH / originalWidth;
            image.Mutate(x =>
                x.Resize((int)(image.Width * resizeRatio), (int)(image.Height * resizeRatio))
            );
            //image.Save(resultPath, jpegEncoder);
            image.SaveAsWebp(resultPath);
        }

        /// <summary>
        /// Resize ảnh trực tiếp và trả về mảng bytes
        /// </summary>
        /// <param name="sourceByte"></param>
        /// <returns></returns>
        public static byte[] ResizeImage(byte[] sourceByte)
        {
            var image = Image.Load(sourceByte);
            int originalWidth = Math.Max(image.Width, image.Height);
            var jpegEncoder = new JpegEncoder { Quality = 75 }; // Điều chỉnh mức độ nén nếu là ảnh jpeg
            if (originalWidth <= MAX_WIDTH)
            {
                return sourceByte;
            }
            double resizeRatio = (double)MAX_WIDTH / originalWidth;
            image.Mutate(x =>
                x.Resize((int)(image.Width * resizeRatio), (int)(image.Height * resizeRatio))
            );

            var memoryStream = new MemoryStream();
            image.Save(memoryStream, jpegEncoder);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Resize ảnh bằng stream
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async Task<Stream> ResizeImage(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;
            var image = await Image.LoadAsync(ms);
            int originalWidth = Math.Max(image.Width, image.Height);

            var resultStream = new MemoryStream();
            var encoder = new WebpEncoder { Quality = 70 };

            if (originalWidth <= MAX_WIDTH)
            {
                await image.SaveAsWebpAsync(resultStream);
                resultStream.Position = 0; // Đặt lại vị trí con trỏ của stream về đầu
                return resultStream;
            }

            double resizeRatio = (double)MAX_WIDTH / originalWidth;
            image.Mutate(x =>
                x.Resize((int)(image.Width * resizeRatio), (int)(image.Height * resizeRatio))
            );
            await image.SaveAsWebpAsync(resultStream, encoder);
            resultStream.Position = 0; // Đặt lại vị trí con trỏ của stream về đầu
            return resultStream;
        }

        public static async Task<Stream> ResizeImageJpg(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;
            var image = await Image.LoadAsync(ms);
            int originalWidth = Math.Max(image.Width, image.Height);

            var resultStream = new MemoryStream();
            var jpegEncoder = new JpegEncoder { Quality = 75 }; // Điều chỉnh mức độ nén nếu là ảnh jpeg

            if (originalWidth <= MAX_WIDTH)
            {
                await image.SaveAsJpegAsync(resultStream, jpegEncoder);
                resultStream.Position = 0; // Đặt lại vị trí con trỏ của stream về đầu
                return resultStream;
            }

            double resizeRatio = (double)MAX_WIDTH / originalWidth;
            image.Mutate(x =>
                x.Resize((int)(image.Width * resizeRatio), (int)(image.Height * resizeRatio))
            );
            await image.SaveAsJpegAsync(resultStream, jpegEncoder);
            resultStream.Position = 0; // Đặt lại vị trí con trỏ của stream về đầu
            return resultStream;
        }

        /// <summary>
        /// Thử resize ảnh, thành công lưu file và trả về true, thất bại trả về false
        /// </summary>
        /// <param name="file"></param>
        /// <param name="resultPath">Đường dẫn lưu</param>
        /// <returns></returns>
        public static bool TryResizeImage(IFormFile file, string resultPath)
        {
            try
            {
                ResizeImage(file, resultPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Thử resize ảnh, thành công trả về true, thất bại trả về false
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async Task<Stream?> TryResizeImage(IFormFile file)
        {
            try
            {
                return await ResizeImage(file);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Thử resize ảnh, thành công trả về true, thất bại trả về false
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async Task<Stream?> TryResizeImageJpg(IFormFile file)
        {
            try
            {
                return await ResizeImageJpg(file);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Hàm chuyển đổi hình ảnh thành base64
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ConvertImageToBase64(Image image, IImageFormat format)
        {
            using var memoryStream = new MemoryStream();
            image.Save(memoryStream, format);

            var imageBytes = memoryStream.ToArray();
            return Convert.ToBase64String(imageBytes);
        }

        /// <summary>
        /// Hàm chuyển ảnh dạng IFormFile thành Image
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public static Image ConvertIFormFileToImage(IFormFile formFile)
        {
            using var stream = formFile.OpenReadStream();
            return Image.Load(stream);
        }

        public static string ConvertIFormFileToImageBase64(IFormFile formFile)
        {
            var image = ConvertIFormFileToImage(formFile);
            using var memoryStream = new MemoryStream();
            image.Save(memoryStream, JpegFormat.Instance);

            var imageBytes = memoryStream.ToArray();
            return Convert.ToBase64String(imageBytes);
        }

        public static IFormFile ConvertStreamToIFormFile(
            Stream stream,
            string fileName,
            string contentType
        )
        {
            // Create a MemoryStream to copy the content of the original stream
            var memoryStream = new MemoryStream();

            stream.CopyTo(memoryStream);

            // Reset the position of the MemoryStream to the beginning
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Create a FormFile using the MemoryStream content
            return new FormFile(memoryStream, 0, memoryStream.Length, null, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
    }
}
