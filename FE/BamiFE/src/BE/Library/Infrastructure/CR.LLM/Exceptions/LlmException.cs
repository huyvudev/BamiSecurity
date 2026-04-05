using CR.InfrastructureBase.Exceptions;

namespace CR.LLM.Exceptions
{
    [Obsolete("Chuyển qua dùng result pattern")]
    public class LlmException : BaseException
    {
        public LlmException(int errorCode)
            : base(errorCode) { }
    }
}
