using CR.DtoBase.Notification;

namespace CR.InfrastructureBase.Notification;

/// <summary>
/// Xử lý job thông báo, gồm các công việc như lấy thông tin người nhận.
/// Dùng <see cref="IPushNotification"/> để tạo thông báo theo từng loại (email, noti app,...) và đẩy vào queue tương ứng
/// </summary>
public interface INotiJobHandler
{
    Task Consume<TData>(NotiJob<TData> message)
        where TData : class;
}