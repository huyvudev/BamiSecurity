using System.Text.Json.Serialization;

namespace CR.InfrastructureBase.Files.Dtos
{
    public class ResponseUploadDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("uri")]
        public string? Uri { get; set; }

        [JsonPropertyName("s3Key")]
        public string? S3Key { get; set; }
        [JsonPropertyName("olds3Key")]
        public string? OldS3Key { get; set; }

        [JsonPropertyName("mimeType")]
        public string? MimeType { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }
    }
}
