using CR.ApplicationBase.Localization;
using CR.ConvertFile.Constants;
using CR.ConvertFile.Localization;
using Microsoft.AspNetCore.Http;

namespace CR.ConvertFile
{
    public class ConvertFileMapErrorCode
        : MapErrorCodeBase<ConvertFileErrorCodes>,
            IConvertFileMapErrorCode
    {
        protected override string PrefixError => "error_convert_file_";

        public ConvertFileMapErrorCode(
            IConvertFileLocalization localization,
            IHttpContextAccessor httpContext
        )
            : base(localization, httpContext) { }
    }
}
