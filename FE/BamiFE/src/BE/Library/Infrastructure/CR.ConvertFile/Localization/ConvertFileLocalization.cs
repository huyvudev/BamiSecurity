using CR.ApplicationBase.Localization;
using Microsoft.AspNetCore.Http;

namespace CR.ConvertFile.Localization
{
    public class ConvertFileLocalization : LocalizationBase, IConvertFileLocalization
    {
        public ConvertFileLocalization(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            LoadDictionary("CR.ConvertFile.Localization.SourceFiles");
        }
    }
}
