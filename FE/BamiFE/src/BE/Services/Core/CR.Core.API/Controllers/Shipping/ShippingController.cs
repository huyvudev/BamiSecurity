using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Shipping;

/// <summary>
/// Quản lý vận chuyển, theo dõi đơn hàng, quản lý trạng thái vận chuyển
/// </summary>
[Authorize]
[Route("api/shipping")]
[ApiController]
public class ShippingController : ApiControllerBase
{
    public ShippingController(ILogger<ShippingController> logger) : base(logger)
    {
    }
}
