using CR.ConstantBase.Notification;
using CR.DtoBase.Interfaces;

namespace CR.DtoBase.Notification
{
    /// <summary>
    /// Khởi tạo message
    /// </summary>
    public class NotiMessageInput<TData> : IRequestId where TData : class
    {
        public required string? RequestId { get; set; }
        public required IEnumerable<NotificationTypes> NotiType { get; set; } = [];
        public required string EventKey { get; set; }
        public required int? TenantId { get; set; }
        public List<Recipient> CC { get; set; } = new();
        public required Recipient Recipient { get; set; }
        public required TData? Data { get; set; }
    }
}
