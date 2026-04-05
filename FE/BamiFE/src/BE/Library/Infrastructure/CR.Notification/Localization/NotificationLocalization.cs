using CR.ApplicationBase.Localization;
using Microsoft.AspNetCore.Http;

namespace CR.Notification.Localization
{
    public class NotificationLocalization : LocalizationBase, INotificationLocalization
    {
        public NotificationLocalization(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            LoadDictionary("CR.Notification.Localization.SourceFiles");
        }
    }
}
