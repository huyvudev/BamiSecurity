using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CR.DtoBase;
using CR.LLM.Abstracts;
using CR.LLM.Configs;
using CR.LLM.Constants;
using CR.LLM.Dtos.ChatGPT;
using CR.LLM.Dtos.Generate;
using CR.LLM.Exceptions;
using CR.Utils.DataUtils;
using CR.Utils.Net.MimeTypes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CR.LLM.Implements
{
    public class ChatGptService : ILlmService
    {
        private const string ApiEndpoint = "https://api.openai.com";
        private const string ApiCompletionsEndpoint = $"{ApiEndpoint}/v1/chat/completions";
        private const string Model = "gpt-4o-mini";

        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public ChatGptService(ILogger<ChatGptService> logger, IOptions<LlmConfig> config)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                config.Value.ChatGPTKey
            );
        }

        public async Task<Result<GenerateMessageDto>> GenerateTextAsync(
            string prompt,
            CancellationToken cancellationToken = default
        )
        {
            var requestBody = new
            {
                model = Model,
                messages = new[] { new { role = "user", content = prompt } }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                MimeTypeNames.ApplicationJson
            );

            HttpResponseMessage response;

            try
            {
                response = await _httpClient.PostAsync(
                    ApiCompletionsEndpoint,
                    content,
                    cancellationToken
                );
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError(
                        "Call api: StatusCode = {0}, ResponseBody = {1}",
                        response.StatusCode,
                        responseBody
                    );
                    return Result<GenerateMessageDto>.Failure(
                        LlmErrorCodes.LlmGenerateError,
                        this.GetCurrentMethodInfo()
                    );
                }

                var responseObject = JsonSerializer.Deserialize<ChatCompletion>(responseBody);
                if (responseObject == null || responseObject.Choices.Count == 0)
                {
                    _logger.LogError(
                        "Error deserialize response: ResponseBody = {0}",
                        responseBody
                    );
                    return Result<GenerateMessageDto>.Failure(
                        LlmErrorCodes.LlmGenerateError,
                        this.GetCurrentMethodInfo()
                    );
                }
                return Result<GenerateMessageDto>.Success(
                    new GenerateMessageDto { Message = responseObject.Choices[0].Message.Content }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating text");
                return Result<GenerateMessageDto>.Failure(
                    LlmErrorCodes.LlmGenerateError,
                    this.GetCurrentMethodInfo()
                );
            }
        }

        public async Task<Result<T>> GenerateContentAsync<T>(string prompt)
            where T : class
        {
            T? result = null;
            int attempts = 0;
            const int maxAttempts = 1;

            while (attempts < maxAttempts && result == null)
            {
                var resultGenText = await GenerateTextAsync(prompt);
                if (resultGenText.IsFailure)
                {
                    return Result<T>.Failure(resultGenText);
                }
                var response = resultGenText.Value;
                try
                {
                    string message = response.Message;
                    string prefix = "```json";
                    string suffix = "```";
                    if (message.StartsWith(prefix))
                    {
                        message = message.Substring(prefix.Length);
                    }

                    if (message.EndsWith(suffix))
                    {
                        message = message.Substring(0, message.Length - suffix.Length);
                    }

                    result = JsonSerializer.Deserialize<T>(message);
                }
                catch (JsonException ex)
                {
                    attempts++;
                    _logger.LogError(
                        ex,
                        "Error when deserialize response from LLM Response = {0}",
                        response.Message
                    );
                    if (attempts == maxAttempts)
                    {
                        return Result<T>.Failure(
                            LlmErrorCodes.LlmGenerateContentError,
                            this.GetCurrentMethodInfo()
                        );
                    }
                }
            }
            if (result != null)
            {
                return Result<T>.Success(result);
            }
            else
            {
                return Result<T>.Failure(
                    LlmErrorCodes.LlmGenerateContentError,
                    this.GetCurrentMethodInfo()
                );
            }
        }
    }
}
