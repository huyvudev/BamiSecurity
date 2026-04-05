using CR.InfrastructureBase.Files;
using CR.InfrastructureBase.Files.Dtos;
using Microsoft.AspNetCore.Http;

namespace CR.S3Bucket
{
    /// <summary>
    /// Quản lý file s3
    /// </summary>
    public interface IS3ManagerFile : IManagerFile
    {
        /// <summary>
        /// Đọc file
        /// </summary>
        /// <param name="s3Key"></param>
        /// <returns></returns>
        Task<Stream> ReadAsync(string s3Key);
        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="s3Key"></param>
        /// <returns></returns>
        Task<DownloadFileDto> DownloadAsync(string s3Key);
        /// <summary>
        /// Move file (chuyển từ tạm sang lưu thật)
        /// </summary>
        /// <param name="s3key"></param>
        /// <returns></returns>
        Task<List<ResponseUploadDto>> MoveAsync(params string[] s3key);
        /// <summary>
        /// Xóa file
        /// </summary>
        /// <param name="s3key"></param>
        /// <returns></returns>
        Task DeleteAsync(params string[] s3key);

        /// <summary>
        /// Upload file thẳng bỏ qua move, convert ảnh sang webp
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<ResponseUploadDto>> UploadFileAsync(params IFormFile[] input);
        /// <summary>
        /// Upload file thẳng bỏ qua move, không convert ảnh sang jpg
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<ResponseUploadDto>> UploadFileWithJpgAsync(params IFormFile[] input);
        /// <summary>
        /// tạo mới s3 key khi move
        /// </summary>
        /// <param name="tempS3Key"></param>
        /// <returns></returns>
        string GenerateNewMoveS3Key(string tempS3Key);
        /// <summary>
        /// Cấu hình life time của resource
        /// </summary>
        /// <returns></returns>
        Task ConfigureLifecyclePolicyAsync();
        /// <summary>
        /// tạo bucket cho tenant
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task CreateBucket(string bucketName);
        /// <summary>
        /// Xóa "temp/" trong s3key
        /// </summary>
        /// <param name="s3Key"></param>
        /// <returns></returns>
        string RemoveTempPrefix(string s3Key);
    }
}
