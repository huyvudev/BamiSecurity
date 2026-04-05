using CR.InfrastructureBase.Exceptions;
using CR.S3Bucket.Constants;

namespace CR.S3Bucket.Exceptions
{
    public class S3BucketException : BaseException
    {
        public S3BucketException(int errorCode)
            : base(errorCode) { }

        public S3BucketException(string? message)
            : base(S3ManagerFileErrorCode.ErrorMessage)
        {
            ErrorMessage = message;
        }

        public S3BucketException(int errorCode, params string[] listParam)
            : base(errorCode, listParam) { }
    }
}
