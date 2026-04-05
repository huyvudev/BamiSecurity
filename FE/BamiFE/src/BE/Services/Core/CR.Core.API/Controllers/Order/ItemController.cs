using CR.Core.ApplicationServices.OrderModule.Abstracts;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Order;

/// <summary>
/// Quản lý các item trong đơn hàng, bao gồm thông tin sản phẩm, artwork (design),
/// trạng thái sản xuất, kiểm soát chất lượng và đóng gói từng item
/// </summary>
[Authorize]
[Route("api/order/item")]
[ApiController]
public class ItemController : ApiControllerBase
{
    private readonly IOrderService _orderService;

    public ItemController(ILogger<OrderController> logger, IOrderService orderService)
        : base(logger)
    {
        _orderService = orderService;
    }
}
