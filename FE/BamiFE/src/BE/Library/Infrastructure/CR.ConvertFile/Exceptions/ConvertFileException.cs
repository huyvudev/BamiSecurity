using CR.InfrastructureBase.Exceptions;

namespace CR.ConvertFile.Exceptions
{
    [Obsolete("Chuyển qua dùng result pattern")]
    public class ConvertFileException : BaseException
    {
        public ConvertFileException(int errorCode)
            : base(errorCode) { }
    }
}
