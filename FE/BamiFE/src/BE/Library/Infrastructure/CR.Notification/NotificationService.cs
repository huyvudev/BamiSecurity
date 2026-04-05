using System.Net;
using System.Text;
using System.Text.Json;
using CR.InfrastructureBase.Notification.Dtos;
using CR.Notification.Configs;
using CR.Utils.Net.MimeTypes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CR.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger _logger;
        private readonly NotificationConfig _config;

        public NotificationService(
            ILogger<NotificationService> logger,
            IOptions<NotificationConfig> config
        )
        {
            _logger = logger;
            _config = config.Value;
        }

        public async Task SendNotificationAsync<T>(
            T dto,
            string key,
            Receiver receiver,
            List<string>? attachments = null
        )
        {
            var body = new SendNotificationDto<T>()
            {
                Key = key,
                Data = dto,
                Receiver = receiver,
                Attachments = attachments
            };
            string json = JsonSerializer.Serialize(body);
            await SendNotificationAsync(json);
        }

        private async Task SendNotificationAsync(string json)
        {
            HttpClient httpClient = new() { BaseAddress = new Uri(_config.BaseUrl) };
            var content = new StringContent(json, Encoding.UTF8, MimeTypeNames.ApplicationJson);
            HttpResponseMessage? response = null;
            try
            {
                response = await httpClient.PostAsync(
                    NotificationConfig.SendNotificationPath,
                    content
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"Không gọi được api gửi thông báo: Path = {NotificationConfig.SendNotificationPath}, RequestBody = {json}, exception: {ex.Message}"
                );
            }
            if (response != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _logger.LogInformation(
                        $"gọi api gửi thông báo thành công: Path = {NotificationConfig.SendNotificationPath}, RequestBody = {json}, StatusCode = {response.StatusCode}, ResponeBody = {responseContent}"
                    );
                }
                else
                {
                    _logger.LogError(
                        $"Không gọi được api gửi thông báo: Path = {NotificationConfig.SendNotificationPath}, RequestBody = {json}, StatusCode = {response.StatusCode}, ResponeBody = {responseContent}"
                    );
                }
            }
        }
    }
}
