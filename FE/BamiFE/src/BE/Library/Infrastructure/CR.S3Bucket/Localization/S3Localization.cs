using CR.ApplicationBase.Localization;
using Microsoft.AspNetCore.Http;

namespace CR.S3Bucket.Localization
{
    public class S3Localization : LocalizationBase, IS3Localization
    {
        public S3Localization(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            LoadDictionary("CR.S3Bucket.Localization.SourceFiles");
        }
    }
}
