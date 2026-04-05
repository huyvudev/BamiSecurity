using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using CR.Constants.Media.File;
using CR.InfrastructureBase.Files.Dtos;
using CR.S3Bucket.Configs;
using CR.S3Bucket.Constants;
using CR.S3Bucket.Dtos.Media;
using CR.S3Bucket.Exceptions;
using CR.Utils.DataUtils;
using CR.Utils.Net.File;
using CR.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CR.S3Bucket
{
    public class S3ManagerFile : IS3ManagerFile
    {
        private readonly ILogger _logger;
        private readonly S3Config _config;
        private readonly IAmazonS3 _s3Client;

        private readonly string BucketName;
        public const string TempPrefix = "temp/";

        public S3ManagerFile(
            ILogger<S3ManagerFile> logger,
            IOptions<S3Config> config,
            IS3BucketName bucketName
        )
        {
            _logger = logger;
            _config = config.Value;
            var credential = new BasicAWSCredentials(_config.AccessKey, _config.SecretKey);
            _s3Client = new AmazonS3Client(
                credential,
                new AmazonS3Config
                {
                    ServiceURL = _config.ServiceUrl,
                    ForcePathStyle = true, // Yêu cầu để làm việc với MinIO,
                }
            );
            //BucketName = bucketName.GetTenantBucketName();
            //ConfigureLifecyclePolicyAsync().GetAwaiter().GetResult();
            BucketName = "pf-bucket";
        }

        /// <summary>
        /// Xóa bucket name của s3 key
        /// </summary>
        /// <param name="s3Key"></param>
        /// <returns></returns>
        private string HandleS3KeyRemoveBucketName(string s3Key)
        {
            return s3Key.Replace($"{BucketName}/", string.Empty);
        }

        /// <summary>
        /// Lấy file name từ path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetFileNameFromPath(string path)
        {
            int lastSlashIndex = path.LastIndexOf('/');
            if (lastSlashIndex == -1)
            {
                return path;
            }
            else
            {
                return path.Substring(lastSlashIndex + 1);
            }
        }

        private static string FormatFileSize(long byteCount)
        {
            const long kilobyte = 1024;
            const long megabyte = kilobyte * 1024;

            if (byteCount < kilobyte)
            {
                return $"{byteCount} B";
            }
            else if (byteCount < megabyte)
            {
                double sizeInKB = Math.Round((double)byteCount / kilobyte, 2);
                return $"{sizeInKB} KB";
            }
            else
            {
                double sizeInMB = Math.Round((double)byteCount / megabyte, 2);
                return $"{sizeInMB} MB";
            }
        }

        /// <summary>
        /// Map response
        /// </summary>
        /// <param name="s3Key"></param>
        /// <param name="contentType"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private ResponseUploadDto MapResponse(
            string oldS3Key,
            string s3Key,
            string contentType,
            string fileName,
            long size = 0
        )
        {
            var result = new ResponseUploadDto
            {
                Uri = $"{BucketName}/{s3Key}",
                S3Key = $"{BucketName}/{s3Key}",
                OldS3Key = $"{BucketName}/{oldS3Key}",
                MimeType = contentType,
                Name = fileName,
                Size = size
            };
            return result;
        }

        /// <summary>
        /// Cấu hình lifetime của file (tự động xóa sau 1 ngày)
        /// </summary>
        /// <returns></returns>
        public async Task ConfigureLifecyclePolicyAsync()
        {
            var lifecycleConfiguration = new LifecycleConfiguration
            {
                Rules = new List<LifecycleRule>
                {
                    new LifecycleRule
                    {
                        Id = "ConfigureLifecyclePolicyAsync",
                        Filter = new LifecycleFilter
                        {
                            LifecycleFilterPredicate = new LifecyclePrefixPredicate
                            {
                                Prefix = TempPrefix
                            }
                        },
                        Status = LifecycleRuleStatus.Enabled,
                        Expiration = new LifecycleRuleExpiration
                        {
                            // Số ngày sau đó đối tượng sẽ bị xóa
                            Days = 1
                        }
                    },
                }
            };

            var putLifecycleRequest = new PutLifecycleConfigurationRequest
            {
                BucketName = S3Config.BucketName,
                Configuration = lifecycleConfiguration
            };

            try
            {
                await _s3Client.PutLifecycleConfigurationAsync(putLifecycleRequest);
                _logger.LogInformation(
                    $"{ConfigureLifecyclePolicyAsync}: Lifecycle policy đã được thiết lập thành công."
                );
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(
                    e,
                    $"{ConfigureLifecyclePolicyAsync}: Error encountered on server. Message:'{e.Message}' when writing an object"
                );
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    $"{ConfigureLifecyclePolicyAsync}: Unknown encountered on server. Message:'{e.Message}' when writing an object"
                );
            }
        }

        /// <summary>
        /// Check xem bucket có tồn tại không, nếu không sẽ tạo mới
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        /// <exception cref="S3BucketException"></exception>
        private async Task DoesS3BucketExistAsync(string bucketName)
        {
            var result = false;
            var response = await _s3Client.ListBucketsAsync();
            foreach (var bucket in response.Buckets)
            {
                if (bucket.BucketName == bucketName)
                {
                    result = true;
                }
            }
            if (!result)
            {
                try
                {
                    //tạo bucket
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true,
                    };
                    await _s3Client.PutBucketAsync(putBucketRequest);

                    //Access policy của bucket
                    string bucketPolicy =
                        @"{
                        ""Version"": ""2012-10-17"",
                        ""Statement"": [
                            {
                                ""Effect"": ""Allow"",
                                ""Principal"": ""*"",
                                ""Action"": ""s3:GetObject"",
                                ""Resource"": ""arn:aws:s3:::"
                        + bucketName
                        + @"/*""
                            }
                        ]
                    }";

                    var putPolicyRequest = new PutBucketPolicyRequest
                    {
                        BucketName = bucketName,
                        Policy = bucketPolicy
                    };
                    await _s3Client.PutBucketPolicyAsync(putPolicyRequest);
                }
                catch (AmazonS3Exception e)
                {
                    _logger.LogError(
                        e,
                        $"{DoesS3BucketExistAsync}: Error encountered on server. Message:'{e.Message}' when writing an object"
                    );
                    throw new S3BucketException(S3ManagerFileErrorCode.CreatBucketError);
                }
                catch (Exception e)
                {
                    _logger.LogError(
                        e,
                        $"{DoesS3BucketExistAsync}: Error encountered on server. Message:'{e.Message}' when writing an object"
                    );
                    throw new S3BucketException(
                        S3ManagerFileErrorCode.ErrorMessage,
                        listParam: e.Message
                    );
                }
            }
        }

        /// <summary>
        /// Tạo tiền tố của s3key
        /// </summary>
        /// <returns></returns>
        public string GenerateObjectName()
        {
            DateTime currentDate = DateTime.Now;
            string year = currentDate.Year.ToString();
            string month = currentDate.Month.ToString("00"); // Đảm bảo định dạng 2 chữ số
            string day = currentDate.Day.ToString("00"); // Đảm bảo định dạng 2 chữ số

            // Tạo object name theo định dạng: yyyy/MM/dd
            string objectName = $"{year}/{month}/{day}";

            return objectName;
        }

        /// <summary>
        /// tạo mới s3 key khi move
        /// </summary>
        /// <param name="tempS3Key"></param>
        /// <returns></returns>
        public string GenerateNewMoveS3Key(string tempS3Key)
        {
            return HandleS3KeyRemoveBucketName(tempS3Key).Replace(TempPrefix, string.Empty);
        }

        public async Task DeleteAsync(params string[] s3key)
        {
            _logger.LogInformation(
                $"{nameof(DeleteAsync)}: input = {JsonSerializer.Serialize(s3key)}"
            );
            if (s3key == null || !s3key.Any())
            {
                return;
            }
            try
            {
                foreach (var item in s3key.Where(s => !string.IsNullOrWhiteSpace(s)))
                {
                    string key = HandleS3KeyRemoveBucketName(item);
                    var deleteObjectRequest = new DeleteObjectRequest
                    {
                        BucketName = BucketName,
                        Key = key
                    };
                    await _s3Client.DeleteObjectAsync(deleteObjectRequest);
                }
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(
                    e,
                    $"{DeleteAsync}: Error encountered on server. Message:'{e.Message}' when deleting an object"
                );
                throw new S3BucketException(S3ManagerFileErrorCode.DeleteMediaBadRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    $"{DeleteAsync}: Error encountered on server. Message:'{e.Message}' when deleting an object"
                );
                throw new S3BucketException(S3ManagerFileErrorCode.DeleteMediaBadRequest);
            }
        }

        public async Task<DownloadFileDto> DownloadAsync(string s3Key)
        {
            _logger.LogInformation($"{nameof(DownloadAsync)}: s3Key = {s3Key}");

            var result = new DownloadFileDto();
            await DoesS3BucketExistAsync(BucketName);
            try
            {
                var getRequest = new GetObjectRequest { BucketName = BucketName, Key = s3Key };

                var response = await _s3Client.GetObjectAsync(getRequest);

                var fileName = GetFileNameFromPath(response.Key);
                var responseStream = response.ResponseStream;

                // Đọc metadata và headers
                string title = response.Metadata["x-amz-meta-title"];
                string contentType = response.Headers["Content-Type"];

                result.Stream = responseStream;
                result.ContentType = contentType;
                result.FileName = fileName;
                return result;
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(
                    e,
                    $"{DownloadAsync}: Lỗi gặp phải khi tải xuống tệp: {e.Message}"
                );
                throw new S3BucketException(S3ManagerFileErrorCode.ReadMediaError);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    $"{DownloadAsync}: Lỗi gặp phải khi tải xuống tệp: {e.Message}"
                );
                throw new S3BucketException(S3ManagerFileErrorCode.ReadMediaError);
            }
        }

        public async Task<List<ResponseUploadDto>> MoveAsync(params string[] s3key)
        {
            _logger.LogInformation($"{nameof(MoveAsync)}: s3Key = {s3key}");
            if (s3key == null || !s3key.Any())
            {
                return new();
            }

            var result = new List<ResponseUploadDto>();
            try
            {
                foreach (var item in s3key.Where(s => !string.IsNullOrWhiteSpace(s)))
                {
                    var oldS3Key = HandleS3KeyRemoveBucketName(item);
                    var newS3Key = GenerateNewMoveS3Key(item);
                    var contentType = string.Empty;

                    // Copy the object from the source to the destination
                    var copyRequest = new CopyObjectRequest
                    {
                        SourceBucket = BucketName,
                        SourceKey = oldS3Key,
                        DestinationBucket = BucketName,
                        DestinationKey = newS3Key
                    };

                    CopyObjectResponse copyResponse = await _s3Client.CopyObjectAsync(copyRequest);

                    // Delete the original object
                    var deleteRequest = new DeleteObjectRequest
                    {
                        BucketName = BucketName,
                        Key = oldS3Key
                    };

                    DeleteObjectResponse deleteResponse = await _s3Client.DeleteObjectAsync(
                        deleteRequest
                    );

                    result.Add(
                        MapResponse(oldS3Key, newS3Key, contentType, GetFileNameFromPath(newS3Key))
                    );
                }
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, $"{MoveAsync}: Lỗi S3 khi move file: {e.Message}");
                throw new S3BucketException(S3ManagerFileErrorCode.MoveFileError);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{MoveAsync}: Lỗi khi move file: {e.Message}");
                throw new S3BucketException(S3ManagerFileErrorCode.MoveFileBadRequest);
            }
            return result;
        }

        public async Task<Stream> ReadAsync(string s3Key)
        {
            _logger.LogInformation($"{nameof(ReadAsync)}: s3Key = {s3Key}");
            using HttpClient httpClient = new HttpClient();
            var uri = new Uri($"{_config.ViewMediaUrl}/{s3Key}");
            var response = await httpClient.GetAsync(uri);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage = await response.Content.ReadFromJsonAsync<ResponseContentDto>();
                //Xử lý các ngoại lệ lúc đọc file từ server
                _logger.LogError(
                    $"{nameof(ReadAsync)}: responseBody = {await response.Content.ReadAsStringAsync()}, responseStatusCode = {response.StatusCode}"
                );
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new S3BucketException(S3ManagerFileErrorCode.ReadMediaNotFound);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    if (errorMessage != null && errorMessage.Message != null)
                    {
                        throw new S3BucketException(errorMessage.Message);
                    }
                    throw new S3BucketException(S3ManagerFileErrorCode.ReadMediaBadRequest);
                }
                else
                {
                    throw new S3BucketException(S3ManagerFileErrorCode.ReadMediaError);
                }
            }
            return await response.Content.ReadAsStreamAsync();
        }

        private async Task<List<ResponseUploadDto>> UploadBaseAsync(
            string methodName,
            string prefix,
            bool isResize,
            params IFormFile[] input
        )
        {
            _logger.LogInformation("{MethodName}:", methodName);

            var result = new List<ResponseUploadDto>();
            foreach (var item in input)
            {
                try
                {
                    string fileName = item.FileName;
                    var contentType = item.ContentType;
                    var extension = Path.GetExtension(fileName);

                    // Loại bỏ phần mở rộng của file
                    string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

                    fileName = StringUtils
                        .RemoveDiacritics(nameWithoutExtension)
                        .ToLower()
                        .Replace(" ", string.Empty);

                    string s3Key =
                        $"{prefix}{GenerateObjectName()}/{fileName}-{DateTime.Now.ToFileTime()}{extension}";

                    await DoesS3BucketExistAsync(BucketName);
                    Stream stream = item.OpenReadStream();
                    long size = item.Length;
                    if (isResize && FileExtensions.ImageExtensions.Contains(extension))
                    {
                        var streamResult = await ImageUtils.TryResizeImage(item);
                        if (streamResult != null)
                        {
                            stream = streamResult;
                            size = streamResult.Length;
                            contentType = MimeTypeNames.ImageWebp;
                            s3Key = $"{s3Key.Replace(extension, string.Empty)}{FileTypes.WEBP}";
                        }
                    }

                    var putRequest = new PutObjectRequest
                    {
                        BucketName = BucketName,
                        Key = s3Key,
                        InputStream = stream,
                        ContentType = contentType
                    };
                    await _s3Client.PutObjectAsync(putRequest);
                    result.Add(
                        MapResponse(s3Key, s3Key, contentType, GetFileNameFromPath(s3Key), size)
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "{MethodName}: Lỗi khi upload file: {ExMessage}",
                        methodName,
                        ex.Message
                    );
                    throw new S3BucketException(S3ManagerFileErrorCode.UploadMediaError);
                }
            }
            return result;
        }

        public async Task<List<ResponseUploadDto>> UploadAsync(params IFormFile[] input)
        {
            return await UploadBaseAsync(nameof(UploadAsync), TempPrefix, false, input);
        }

        public async Task<List<ResponseUploadDto>> UploadFileAsync(params IFormFile[] input)
        {
            return await UploadBaseAsync(nameof(UploadAsync), string.Empty, false, input);
        }

        public async Task<List<ResponseUploadDto>> UploadFileWithJpgAsync(params IFormFile[] input)
        {
            _logger.LogInformation($"{nameof(UploadFileWithJpgAsync)}:");

            var result = new List<ResponseUploadDto>();
            foreach (var item in input)
            {
                try
                {
                    string fileName = item.FileName;
                    var contentType = item.ContentType;
                    var extension = Path.GetExtension(fileName);

                    // Loại bỏ phần mở rộng của file
                    string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

                    fileName = StringUtils
                        .RemoveDiacritics(nameWithoutExtension)
                        .ToLower()
                        .Replace(" ", string.Empty);

                    string s3Key = $"{GenerateObjectName()}/{fileName}-{DateTime.Now.ToFileTime()}";

                    await DoesS3BucketExistAsync(BucketName);
                    s3Key = $"{s3Key}{Path.GetExtension(item.FileName)}";
                    Stream stream = item.OpenReadStream();
                    long size = item.Length;
                    if (FileExtensions.ImageExtensions.Contains(extension))
                    {
                        var streamResult = await ImageUtils.TryResizeImageJpg(item);
                        if (streamResult != null)
                        {
                            stream = streamResult;
                            size = streamResult.Length;
                            contentType = MimeTypeNames.ImageJpeg;
                            s3Key = $"{Path.GetFileNameWithoutExtension(s3Key)}{FileTypes.JPG}";
                        }
                    }

                    var putRequest = new PutObjectRequest
                    {
                        BucketName = BucketName,
                        Key = s3Key,
                        InputStream = stream,
                        ContentType = contentType,
                    };
                    await _s3Client.PutObjectAsync(putRequest);
                    result.Add(
                        MapResponse(s3Key, s3Key, contentType, GetFileNameFromPath(s3Key), size)
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "{MethodName}: Lỗi khi upload file: {ExMessage}",
                        nameof(UploadFileWithJpgAsync),
                        ex.Message
                    );
                    throw new S3BucketException(S3ManagerFileErrorCode.UploadMediaError);
                }
            }
            return result;
        }

        public async Task CreateBucket(string bucketName)
        {
            await DoesS3BucketExistAsync(bucketName);
        }

        public string RemoveTempPrefix(string s3Key)
        {
            return s3Key.Replace(TempPrefix, string.Empty);
        }
    }
}
