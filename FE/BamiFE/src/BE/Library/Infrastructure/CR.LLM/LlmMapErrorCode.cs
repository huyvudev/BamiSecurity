using CR.ApplicationBase.Localization;
using CR.LLM.Constants;
using CR.LLM.Localization;
using Microsoft.AspNetCore.Http;

namespace CR.LLM
{
    public class LlmMapErrorCode
        : MapErrorCodeBase<LlmErrorCodes>,
            ILlmMapErrorCode
    {
        protected override string PrefixError => "error_llm_";

        public LlmMapErrorCode(
            ILlmLocalization localization,
            IHttpContextAccessor httpContext
        )
            : base(localization, httpContext) { }
    }
}
