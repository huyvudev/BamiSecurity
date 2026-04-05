using CR.DtoBase.Interfaces;

namespace CR.DtoBase.Notification;

/// <summary>
/// Khởi tạo job thông báo
/// </summary>
public class NotiJob<TData> : IRequestId where TData : class
{
    /// <summary>
    /// Id của tenant
    /// </summary>
    public int? TenantId { get; set; }

    /// <summary>
    /// Id của khóa học
    /// </summary>
    public int? CourseId { get; set; }

    /// <summary>
    /// Key của event
    /// </summary>
    public string EventKey { get; set; } = null!;
    /// <summary>
    /// UserId nhận thông báo
    /// </summary>
    public IEnumerable<int> RecipientIds { get; set; } = [];
    /// <summary>
    /// Dữ liệu gửi kèm
    /// </summary>
    public TData? Data { get; set; }

    public string? RequestId { get; set; }

    public NotiJob()
    {
    }

    public NotiJob(NotiJob<TData> noti)
    {
        TenantId = noti.TenantId;
        EventKey = noti.EventKey;
        RecipientIds = noti.RecipientIds;
        Data = noti.Data;
        RequestId = noti.RequestId;
        CourseId = noti.CourseId;
    }
}
