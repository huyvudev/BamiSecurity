using CR.EntitiesBase.Base;

namespace CR.ConvertFile.Constants
{
    public class ConvertFileErrorCodes : IErrorCode
    {
        public const int InvalidFormatFileExtension = 12000;
        public const int ConvertFileServiceOutOfDuration = 12001;
        public const int InternalServerError = 12002;
    }
}
