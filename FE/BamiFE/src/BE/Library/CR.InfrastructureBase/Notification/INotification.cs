using CR.InfrastructureBase.Notification.Dtos;

namespace CR.InfrastructureBase.Notification
{
    [Obsolete($"bỏ chuyển qua {nameof(IPushNotification)}")]
    public interface INotification
    {
        /// <summary>
        /// Send EMAIL, SMS, PUSH APP
        /// </summary>
        /// <typeparam name="T">DTO</typeparam>
        /// <param name="dto">Data</param>
        /// <param name="key">Key</param>
        /// <param name="receiver">Receiver thông tin người nhận</param>
        /// <param name="attachments">Danh sách file đính kèm dạng list url</param>
        /// <returns></returns>
        Task SendNotificationAsync<T>(
            T dto,
            string key,
            Receiver receiver,
            List<string>? attachments = null
        );
    }
}
