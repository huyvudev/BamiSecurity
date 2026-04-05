using CR.Common.Localization;
using CR.Core.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;

namespace CR.Authentication.ApplicationServices.Common.Localization
{
    public class CoreLocalization : CRLocalization, ICoreLocalization
    {
        public CoreLocalization(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            LoadDictionary("CR.Core.ApplicationServices.Common.Localization.SourceFiles");
        }
    }
}
