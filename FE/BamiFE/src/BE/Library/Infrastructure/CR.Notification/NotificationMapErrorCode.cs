using CR.ApplicationBase.Localization;
using CR.Notification.Constants;
using CR.Notification.Localization;
using Microsoft.AspNetCore.Http;

namespace CR.Notification
{
    public class NotificationMapErrorCode
        : MapErrorCodeBase<NotificationErrorCode>,
            INotificationMapErrorCode
    {
        protected override string PrefixError => "error_notification_";

        public NotificationMapErrorCode(
            INotificationLocalization localization,
            IHttpContextAccessor httpContext
        )
            : base(localization, httpContext) { }
    }
}
