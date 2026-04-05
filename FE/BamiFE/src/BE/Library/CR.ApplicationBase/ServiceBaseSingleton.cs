using CR.ApplicationBase.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CR.ApplicationBase
{
    public abstract class ServiceBaseSingleton : ServiceAbstract
    {
        protected ServiceBaseSingleton(ILogger logger, IHttpContextAccessor httpContext)
            : base(logger, httpContext) { }

        protected ServiceBaseSingleton(
            ILogger logger,
            IHttpContextAccessor httpContext,
            ILocalization localization
        )
            : base(logger, httpContext, localization) { }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected ServiceBaseSingleton(ILogger logger, IServiceProvider serviceProvider)
            : base(logger, serviceProvider) { }
    }
}
