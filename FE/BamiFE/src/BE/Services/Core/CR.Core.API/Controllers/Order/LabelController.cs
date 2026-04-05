using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Order;

/// <summary>
/// Quản lý in nhãn (label) vận chuyển
/// </summary>
[Authorize]
[Route("api/order/label")]
[ApiController]
public class LabelController : ApiControllerBase
{
    public LabelController(ILogger<LabelController> logger) : base(logger)
    {
    }
}
