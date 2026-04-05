using CR.Core.ApplicationServices.OrderModule.Abstracts;
using CR.Core.Dtos.OrderModule.SaleOrder;
using CR.Core.Dtos.PartnerModule.Partner;
using CR.DtoBase;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Order;

/// <summary>
/// Sale quản lý order (tạo order)
/// </summary>
[Authorize]
[Route("api/order/sale")]
[ApiController]
public class SaleOrderController : ApiControllerBase
{
    private readonly ISaleOrderService _saleOrderService;

    public SaleOrderController(ILogger<OrderController> logger, ISaleOrderService saleOrderService)
        : base(logger)
    {
        _saleOrderService = saleOrderService;
    }

    /// <summary>
    /// Sale thêm đơn hàng trực tiếp từng đơn hàng
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("add")]
    [ProducesResponseType(typeof(ApiResponse<CreateSaleOrderResultDto>), StatusCodes.Status200OK)]
    public async Task<ApiResponse> CreateOrder(CreateSaleOrderDto input)
    {
        try
        {
            return OkResult(await _saleOrderService.CreateOrder(input));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }
}
