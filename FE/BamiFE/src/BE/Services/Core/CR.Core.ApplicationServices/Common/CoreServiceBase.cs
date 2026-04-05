using CR.ApplicationBase;
using CR.ApplicationBase.Localization;
using CR.Core.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.Common
{
    public abstract class CoreServiceBase : ServiceBase<CoreDbContext>
    {
        protected CoreServiceBase(ILogger logger, IHttpContextAccessor httpContext)
            : base(logger, httpContext) { }

        protected CoreServiceBase(
            ILogger logger,
            IHttpContextAccessor httpContext,
            ILocalization localization
        )
            : base(logger, httpContext, localization) { }

        protected static string GetItemTemplate(int orderId, int itemIndex)
        {
            return $"#{orderId:D6}_{itemIndex}";
        }
    }
}
