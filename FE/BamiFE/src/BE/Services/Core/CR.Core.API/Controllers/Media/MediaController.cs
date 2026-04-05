using CR.S3Bucket;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Media;

/// <summary>
/// Quản lý file media
/// </summary>
//[Authorize]
[Route("api/media")]
public class MediaController : ApiControllerBase
{
    private readonly IS3ManagerFile _s3ManagerFile;

    public MediaController(ILogger<MediaController> logger, IS3ManagerFile s3ManagerFile)
        : base(logger)
    {
        _s3ManagerFile = s3ManagerFile;
    }

    /// <summary>
    /// Upload 
    /// </summary>
    [HttpPost("upload")]
    public async Task<ApiResponse> UploadAsync(params IFormFile[] input)
    {
        try
        {
            return new(await _s3ManagerFile.UploadAsync(input));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Chuyển từ lưu tạm sang lưu thật
    /// </summary>
    [HttpPost("move")]
    public async Task<ApiResponse> MoveAsync(params string[] s3Key)
    {
        try
        {
            return new(await _s3ManagerFile.MoveAsync(s3Key));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Xóa file
    /// </summary>
    [HttpDelete("delete")]
    public async Task<ApiResponse> DeleteAsync(params string[] s3Key)
    {
        try
        {
            await _s3ManagerFile.DeleteAsync(s3Key);
            return new();
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }
}
