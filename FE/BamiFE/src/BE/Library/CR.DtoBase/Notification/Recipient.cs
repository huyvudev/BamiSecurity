namespace CR.DtoBase.Notification
{
    public class Recipient
    {
        public required string? Email { get; set; }
        public IEnumerable<string> FcmToken { get; set; } = [];
    }
}
