using CR.ApplicationBase.Localization;
using CR.Constants.ErrorCodes;
using CR.ConvertFile;
using CR.ConvertFile.Exceptions;
using CR.ConvertFile.Localization;
using CR.Core.Infrastructure.Exceptions;
using CR.DtoBase;
using CR.InfrastructureBase.Exceptions;
using CR.LLM;
using CR.LLM.Exceptions;
using CR.LLM.Localization;
using CR.S3Bucket;
using CR.S3Bucket.Exceptions;
using CR.S3Bucket.Localization;
using CR.Utils.Net.File;
using CR.Utils.Net.MimeTypes;
using CR.Utils.Net.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Text.Json;

namespace CR.WebAPIBase.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected ILogger _logger;

        public ApiControllerBase(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Return file với stream file
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [NonAction]
        protected FileStreamResult FileByStream(Stream fileStream, string fileName)
        {
            string? ext = Path.GetExtension(fileName)?.ToLower();

            return ext switch
            {
                FileTypes.JPG
                or FileTypes.JPEG
                or FileTypes.JFIF
                    => File(fileStream, MimeTypeNames.ImageJpeg),
                FileTypes.PNG => File(fileStream, MimeTypeNames.ImagePng),
                FileTypes.SVG => File(fileStream, MimeTypeNames.ImageSvgXml),
                FileTypes.GIF => File(fileStream, MimeTypeNames.ImageGif),
                FileTypes.MP4 => File(fileStream, MimeTypeNames.VideoMp4),
                FileTypes.PDF => File(fileStream, MimeTypeNames.ApplicationPdf),
                FileTypes.WEBP => File(fileStream, MimeTypeNames.ImageWebp),
                _ => File(fileStream, MimeTypeNames.ApplicationOctetStream, fileName),
            };
        }

        /// <summary>
        /// Return file với byte file
        /// </summary>
        /// <param name="fileByte"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [NonAction]
        protected FileContentResult FileByFormat(byte[] fileByte, string fileName)
        {
            string? ext = Path.GetExtension(fileName)?.ToLower();

            return ext switch
            {
                FileTypes.JPG
                or FileTypes.JPEG
                or FileTypes.JFIF
                    => File(fileByte, MimeTypeNames.ImageJpeg),
                FileTypes.PNG => File(fileByte, MimeTypeNames.ImagePng),
                FileTypes.SVG => File(fileByte, MimeTypeNames.ImageSvgXml),
                FileTypes.GIF => File(fileByte, MimeTypeNames.ImageGif),
                FileTypes.MP4 => File(fileByte, MimeTypeNames.VideoMp4),
                FileTypes.PDF => File(fileByte, MimeTypeNames.ApplicationPdf),
                FileTypes.WEBP => File(fileByte, MimeTypeNames.ImageWebp),
                _ => File(fileByte, MimeTypeNames.ApplicationOctetStream, fileName),
            };
        }

        [NonAction]
        public ApiResponse OkException(Exception ex)
        {
            var mapErrorCode = HttpContext.RequestServices.GetRequiredService<ICoreMapErrorCode>();
            var request = HttpContext.Request;
            string errStr =
                $"Path = {request.Path}, Query = {JsonSerializer.Serialize(request.Query)}";
            int errorCode;
            string message = ex.Message;
            object? data = null;
            if (ex is UserFriendlyException userFriendlyException)
            {
                var coreLocalization =
                    HttpContext.RequestServices.GetRequiredService<ICoreLocalization>();
                errorCode = userFriendlyException.ErrorCode;
                data = userFriendlyException.DataValue;
                try
                {
                    if (!string.IsNullOrWhiteSpace(userFriendlyException.ErrorMessage))
                    {
                        message = userFriendlyException.ErrorMessage;
                    }
                    else if (
                        userFriendlyException.ListParam is not null
                        && userFriendlyException.ListParam.Length > 0
                    )
                    {
                        message = coreLocalization.Localize(
                            mapErrorCode.GetErrorMessageKey(errorCode),
                            userFriendlyException.ListParam
                        );
                    }
                    else
                    {
                        message = !string.IsNullOrWhiteSpace(userFriendlyException.MessageLocalize)
                            ? coreLocalization.Localize(userFriendlyException.MessageLocalize)
                            : mapErrorCode.GetErrorMessage(userFriendlyException.ErrorCode);
                    }
                }
                catch (Exception exc)
                {
                    message = exc.Message;
                }
            }
            else if (ex is DbUpdateException dbUpdateException)
            {
                errorCode = ErrorCode.InternalServerError;
                if (dbUpdateException.InnerException != null)
                {
                    message = dbUpdateException.InnerException.Message;
                }
                else
                {
                    message = dbUpdateException.Message;
                }
                _logger?.LogInformation(
                    ex,
                    $"{ex.GetType()}: {errStr}, ErrorCode = {errorCode}, Message = {message}"
                );
                message = "Internal server error";
            }
            else if (ex is S3BucketException s3BucketException)
            {
                errorCode = s3BucketException.ErrorCode;
                try
                {
                    var s3MapErrorCode =
                        HttpContext.RequestServices.GetRequiredService<IS3MapErrorCode>();
                    var s3Localization =
                        HttpContext.RequestServices.GetRequiredService<IS3Localization>();

                    if (!string.IsNullOrWhiteSpace(s3BucketException.ErrorMessage))
                    {
                        message = s3BucketException.ErrorMessage;
                    }
                    else if (
                        s3BucketException.ListParam is not null
                        && s3BucketException.ListParam.Length > 0
                    )
                    {
                        message = s3Localization.Localize(
                            mapErrorCode.GetErrorMessageKey(errorCode),
                            s3BucketException.ListParam
                        );
                    }
                    else
                    {
                        message = !string.IsNullOrWhiteSpace(s3BucketException.MessageLocalize)
                            ? s3Localization.Localize(s3BucketException.MessageLocalize)
                            : s3MapErrorCode.GetErrorMessage(s3BucketException.ErrorCode);
                    }
                }
                catch (Exception exc)
                {
                    message = exc.Message;
                }
            }
            else if (ex is ConvertFileException convertFileException)
            {
                errorCode = convertFileException.ErrorCode;
                try
                {
                    var convertFileMapError =
                        HttpContext.RequestServices.GetRequiredService<IConvertFileMapErrorCode>();
                    var convertFileLocalization =
                        HttpContext.RequestServices.GetRequiredService<IConvertFileLocalization>();

                    if (!string.IsNullOrWhiteSpace(convertFileException.ErrorMessage))
                    {
                        message = convertFileException.ErrorMessage;
                    }
                    else if (
                        convertFileException.ListParam is not null
                        && convertFileException.ListParam.Length > 0
                    )
                    {
                        message = convertFileLocalization.Localize(
                            mapErrorCode.GetErrorMessageKey(errorCode),
                            convertFileException.ListParam
                        );
                    }
                    else
                    {
                        message = !string.IsNullOrWhiteSpace(convertFileException.MessageLocalize)
                            ? convertFileLocalization.Localize(convertFileException.MessageLocalize)
                            : convertFileMapError.GetErrorMessage(convertFileException.ErrorCode);
                    }
                }
                catch (Exception exc)
                {
                    message = exc.Message;
                }
            }
            else if (ex is SmtpException smtpException)
            {
                errorCode = ErrorCode.InternalServerError;
                message =
                    smtpException.InnerException != null
                        ? smtpException.InnerException.Message
                        : smtpException.Message;
            }
            else if (ex is LlmException llmException)
            {
                errorCode = llmException.ErrorCode;
                try
                {
                    var llmMapError =
                        HttpContext.RequestServices.GetRequiredService<ILlmMapErrorCode>();
                    var llmLocalization =
                        HttpContext.RequestServices.GetRequiredService<ILlmLocalization>();

                    if (!string.IsNullOrWhiteSpace(llmException.ErrorMessage))
                    {
                        message = llmException.ErrorMessage;
                    }
                    else if (
                        llmException.ListParam is not null
                        && llmException.ListParam.Length > 0
                    )
                    {
                        message = llmLocalization.Localize(
                            mapErrorCode.GetErrorMessageKey(errorCode),
                            llmException.ListParam
                        );
                    }
                    else
                    {
                        message = !string.IsNullOrWhiteSpace(llmException.MessageLocalize)
                            ? llmLocalization.Localize(llmException.MessageLocalize)
                            : llmMapError.GetErrorMessage(llmException.ErrorCode);
                    }
                }
                catch (Exception exc)
                {
                    message = exc.Message;
                }
            }
            else
            {
                errorCode = ErrorCode.InternalServerError;
                _logger?.LogError(
                    ex,
                    $"{ex.GetType()}: {errStr}, ErrorCode = {errorCode}, Message = {message}"
                );
                message = "Internal server error";
                return new ApiResponse(
                    Utils.Net.Request.StatusCode.Error,
                    data,
                    errorCode,
                    message
                );
            }
            _logger?.LogInformation(ex, $"{errStr}, ErrorCode = {errorCode}, Message = {message}");
            return new ApiResponse(Utils.Net.Request.StatusCode.Error, data, errorCode, message);
        }

        /// <summary>
        /// Danh sách các service map error code
        /// </summary>
        private static readonly List<Type> ListMapErrorCode =
            new()
            {
                typeof(ICoreMapErrorCode),
                typeof(IS3MapErrorCode),
                typeof(IConvertFileMapErrorCode),
                typeof(ILlmMapErrorCode),
            };

        [NonAction]
        private ApiResponse OkReturnResult(Result result)
        {
            string message = string.Empty;

            foreach (var type in ListMapErrorCode)
            {
                var mapErrorCodeService = (IMapErrorCode)
                    HttpContext.RequestServices.GetRequiredService(type);
                string? messageGet = mapErrorCodeService.TryGetErrorMessage(result.ErrorCode);
                if (messageGet != null)
                {
                    message = messageGet;
                    break;
                }
            }

            if (result.ListParam != null && result.ListParam.Length > 0)
            {
                message = string.Format(message, result.ListParam);
            }

            _logger?.LogInformation(
                "{MethodName}: isSuccess = {IsSuccess}, errorCode = {ErrorCodeValue}, message = {MessageValue}, stackTrace = {StackTraceValue}",
                nameof(OkReturnResult),
                result.IsSuccess,
                result.ErrorCode,
                message,
                result.StackTrace
            );

            return new ApiResponse(
                result.IsFailure
                    ? Utils.Net.Request.StatusCode.Error
                    : Utils.Net.Request.StatusCode.Success,
                result.OtherData,
                result.ErrorCode,
                message
            );
        }

        [NonAction]
        public ApiResponse OkResult(Result result)
        {
            if (result.IsSuccess)
            {
                return new ApiResponse();
            }
            return OkReturnResult(result);
        }

        [NonAction]
        public ApiResponse OkResult<T>(Result<T> result)
            where T : class
        {
            if (result.IsSuccess)
            {
                return new ApiResponse(result.Value);
            }
            return OkReturnResult(result);
        }
    }
}
