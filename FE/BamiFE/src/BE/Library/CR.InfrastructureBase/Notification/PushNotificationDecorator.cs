using CR.DtoBase.Notification;
using Microsoft.Extensions.Logging;

namespace CR.InfrastructureBase.Notification
{
    public abstract class PushNotificationDecorator : IPushNotification
    {
        protected readonly ILogger _logger;
        protected readonly IPushNotification? _sendNotification;

        protected PushNotificationDecorator(ILogger logger, IPushNotification? sendNotification)
        {
            _logger = logger;
            _sendNotification = sendNotification;
        }

        public virtual async Task Send<TData>(NotiMessageInput<TData> message)
            where TData : class
        {
            if (_sendNotification == null)
            {
                return;
            }
            await _sendNotification.Send(message);
        }
    }
}
