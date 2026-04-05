using CR.ApplicationBase.Localization;
using Microsoft.AspNetCore.Http;

namespace CR.LLM.Localization
{
    public class LlmLocalization : LocalizationBase, ILlmLocalization
    {
        public LlmLocalization(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            LoadDictionary("CR.LLM.Localization.SourceFiles");
        }
    }
}
