using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Order;

/// <summary>
/// Quản lý giỏ (gộp đơn = tạo giỏ, theo dõi trạng thái giỏ trạng thái item trong giỏ, quản lý giỏ, quản lý item trong giỏ)
/// </summary>
[Authorize]
[Route("api/order/basket")]
[ApiController]
public class BasketController : ApiControllerBase
{
    public BasketController(ILogger<BasketController> logger) : base(logger)
    {
    }

    //gộp đơn = tạo giỏ: thêm item vào giỏ. item chung giỏ là các item cùng đơn, cùng SKU, cùng cách đóng gói,...


    //theo dõi trạng thái giỏ là theo dói trạng thái item trong giỏ
}
