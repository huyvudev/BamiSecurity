using CR.ApplicationBase.Localization;
using Microsoft.AspNetCore.Http;

namespace CR.Common.Localization
{
    public class CRLocalization : LocalizationBase
    {
        public CRLocalization(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            LoadDictionary("CR.Common.Localization.SourceFiles");
        }
    }
}
