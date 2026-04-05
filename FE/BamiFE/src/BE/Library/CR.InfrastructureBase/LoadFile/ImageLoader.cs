using CR.Constants.ErrorCodes;
using CR.DtoBase;
using CR.Utils.DataUtils;
using Microsoft.Extensions.Logging;

namespace CR.InfrastructureBase.LoadFile;

public class ImageLoader : IImageLoader
{
    /// <summary>
    /// Kích cỡ file tối đa cho phép là 5MB
    /// </summary>
    private const int FileMaxLength = 1024 * 1024 * 5;
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public ImageLoader(ILogger<ImageLoader> logger)
    {
        _httpClient = new HttpClient();
        _logger = logger;
    }

    public async Task<Result<Stream>> LoadImageFromUrlAsync(string url)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(LoadImageFromUrlAsync),
            nameof(url),
            url
        );

        if (string.IsNullOrEmpty(url))
        {
            _logger.LogInformation("Invalid url: {Url}", url);
            return Result<Stream>.Failure(
                ErrorCode.ImageLoaderInvalidUrl,
                this.GetCurrentMethodInfo(),
                listParam: url
            );
        }

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(url);
        }
        catch (HttpRequestException e)
        {
            _logger.LogWarning(e, "Error when load image from url: {Url}", url);
            return Result<Stream>.Failure(
                ErrorCode.ImageLoaderHttpRequestError,
                this.GetCurrentMethodInfo()
            );
        }

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning(
                "Error when load image from url: {Url}, status code: {StatusCode}",
                url,
                response.StatusCode
            );
            return Result<Stream>.Failure(
                ErrorCode.ImageLoaderHttpRequestError,
                this.GetCurrentMethodInfo()
            );
        }

        Stream imageStream;
        try
        {
            imageStream = await response.Content.ReadAsStreamAsync();
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error when read image stream from response");
            return Result<Stream>.Failure(
                ErrorCode.ImageLoaderHttpRequestError,
                this.GetCurrentMethodInfo()
            );
        }
        if (imageStream.Length > FileMaxLength)
        {
            _logger.LogWarning(
                "Image size is too large: {ImageSize}",
                imageStream.Length
            );
            return Result<Stream>.Failure(
                ErrorCode.ImageLoaderInvalidImageSize,
                this.GetCurrentMethodInfo(),
                listParam: imageStream.Length.FormatFileSize()
            );
        }

        return Result.Success(imageStream);
    }
}
