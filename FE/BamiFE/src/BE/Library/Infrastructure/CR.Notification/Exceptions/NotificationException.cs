using CR.InfrastructureBase.Exceptions;

namespace CR.Notification.Exceptions
{
    public class NotificationException : BaseException
    {
        public NotificationException(int errorCode)
            : base(errorCode) { }
    }
}
