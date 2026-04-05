using CR.DtoBase.Interfaces;

namespace CR.DtoBase.Notification
{
    /// <summary>
    /// Các thông tin chuẩn bị cho gửi thông báo
    /// </summary>
    /// <typeparam name="TTemplateId">Id mẫu gửi thông báo</typeparam>
    /// <typeparam name="TData"></typeparam>
    public abstract class NotificationMessage<TTemplateId, TData> : IRequestId where TData : class
    {
        public string? RequestId { get; set; }
        public int? TenantId { get; set; }
        public TTemplateId TemplateId { get; set; } = default!;
        public TData? Data { get; set; }
        public Recipient Recipient { get; set; } = null!;
        public List<Recipient> CC { get; set; } = new();

        protected NotificationMessage() { }

        protected NotificationMessage(TTemplateId templateId, NotiMessageInput<TData> message)
        {
            RequestId = message.RequestId;
            TenantId = message.TenantId;
            TemplateId = templateId;
            Recipient = message.Recipient;
            CC = message.CC;
            Data = message.Data;
        }
    }
}
