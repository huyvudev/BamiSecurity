using System.Text.Json.Serialization;

namespace CR.InfrastructureBase.Notification.Dtos
{
    [Obsolete("Bỏ")]
    public class Receiver
    {
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("email")]
        public EmailNotification? Email { get; set; }

        [JsonPropertyName("userId")]
        public int? UserId { get; set; }

        [JsonPropertyName("fcm_tokens")]
        public List<string>? FcmTokens { get; set; }
    }

    [Obsolete("Bỏ")]
    public class EmailNotification
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }
    }
}
