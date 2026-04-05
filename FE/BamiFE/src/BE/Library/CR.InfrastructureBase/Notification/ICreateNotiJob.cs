using CR.DtoBase.Notification;

namespace CR.InfrastructureBase.Notification;

/// <summary>
/// Tạo job thông báo
/// </summary>
public interface ICreateNotiJob
{
    Task CreateJob<TNotiJob, TData>(TNotiJob job)
        where TNotiJob : NotiJob<TData>
        where TData : class;
}
