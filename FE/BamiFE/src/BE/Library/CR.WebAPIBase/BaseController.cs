using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CR.WebAPIBase
{
    /// <summary>
    /// Base controller, xử lý ngoại lệ
    /// </summary>
    public class BaseController : ControllerBase
    {
        protected ILogger? _logger;
    }
}
