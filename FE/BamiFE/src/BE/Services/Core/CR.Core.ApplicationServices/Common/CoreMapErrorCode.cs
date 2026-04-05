using CR.ApplicationBase.Localization;
using CR.Constants.ErrorCodes;
using CR.Core.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;

namespace CR.Core.ApplicationServices.Common
{
    public class CoreMapErrorCode : MapErrorCodeBase<CoreErrorCode>, ICoreMapErrorCode
    {
        public CoreMapErrorCode(ICoreLocalization localization, IHttpContextAccessor httpContext)
            : base(localization, httpContext) { }
    }
}
