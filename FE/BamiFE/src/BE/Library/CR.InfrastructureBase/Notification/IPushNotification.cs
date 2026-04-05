using CR.DtoBase.Notification;

namespace CR.InfrastructureBase.Notification
{
    /// <summary>
    /// Tìm mẫu thông báo (có thể phải tìm theo tenantId) và đẩy message thông báo vào các queue của mỗi loại thông báo
    /// </summary>
    public interface IPushNotification
    {
        Task Send<TData>(NotiMessageInput<TData> message)
            where TData : class;
    }
}
