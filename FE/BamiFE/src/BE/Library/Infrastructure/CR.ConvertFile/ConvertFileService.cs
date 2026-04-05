using System.Net;
using System.Net.Http.Headers;
using CR.ConvertFile.Configs;
using CR.ConvertFile.Constants;
using CR.ConvertFile.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CR.ConvertFile
{
    /// <summary>
    /// Xử lý convert file
    /// </summary>
    public class ConvertFileService : IConvertFileService
    {
        private readonly ILogger _logger;
        private readonly ConvertFileConfig _config;

        public ConvertFileService(
            ILogger<ConvertFileService> logger,
            IOptions<ConvertFileConfig> config
        )
        {
            _logger = logger;
            _config = config.Value;
        }

        public async Task<Stream> ConvertWordToPdf(Stream input, string fileName)
        {
            _logger.LogInformation($"{nameof(ConvertWordToPdf)}: fileName = {fileName}");
            input.Position = 0;
            using HttpClient client = new() { Timeout = TimeSpan.FromSeconds(30) };
            client.BaseAddress = new Uri(_config.BaseUrl);
            MultipartFormDataContent formData = [];
            var fileContent = new StreamContent(input);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "files",
                FileName = fileName,
            };
            formData.Add(fileContent);
            HttpResponseMessage response = await client.PostAsync(
                "/forms/libreoffice/convert",
                formData
            );
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    $"{nameof(ConvertWordToPdf)}: responseBody = {await response.Content.ReadAsStringAsync()}, responseStatusCode = {response.StatusCode}"
                );
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new ConvertFileException(
                        ConvertFileErrorCodes.InvalidFormatFileExtension
                    );
                }
                else if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    throw new ConvertFileException(
                        ConvertFileErrorCodes.ConvertFileServiceOutOfDuration
                    );
                }
                else
                {
                    throw new ConvertFileException(ConvertFileErrorCodes.InternalServerError);
                }
            }
            return await response.Content.ReadAsStreamAsync();
        }
    }
}
