using System.Text.Json.Serialization;

namespace CR.InfrastructureBase.Notification.Dtos
{
    [Obsolete("Bỏ")]
    public class SendNotificationDto<T>
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("receiver")]
        public Receiver? Receiver { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("attachments")]
        public List<string>? Attachments { get; set; }
    }
}
