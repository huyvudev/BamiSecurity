using CR.EntitiesBase.Base;

namespace CR.LLM.Constants
{
    public class LlmErrorCodes : IErrorCode
    {
        public const int LlmGenerateError = 14000;
        public const int LlmGenerateContentError = 14001;
    }
}
