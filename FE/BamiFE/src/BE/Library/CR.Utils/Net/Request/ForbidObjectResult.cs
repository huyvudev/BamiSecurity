using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CR.Utils.Net.Request
{
    public class ForbidObjectResult : ObjectResult
    {
        public ForbidObjectResult(object? value) : base(value)
        {
            StatusCode = StatusCodes.Status403Forbidden;
        }
    }
}
