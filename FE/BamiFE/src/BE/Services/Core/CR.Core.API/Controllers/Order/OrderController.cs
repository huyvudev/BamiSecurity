using CR.Core.ApplicationServices.OrderModule.Abstracts;
using CR.Core.Dtos.OrderModule.Order;
using CR.Core.Dtos.OrderModule.OrderDto;
using CR.DtoBase;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Order;

/// <summary>
/// Quản trị viên quản lý các thông tin liên quan đến đơn hàng, bao gồm tạo, cập nhật thông tin đơn hàng
/// </summary>
[Authorize]
[Route("api/order")]
[ApiController]
public class OrderController : ApiControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        : base(logger)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Thêm Order
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("add")]
    [ProducesResponseType(typeof(ApiResponse<CreateOrderResultDto>), StatusCodes.Status200OK)]
    public async Task<ApiResponse> CreateOrder(CreateOrderDto input)
    {
        try
        {
            return OkResult(await _orderService.CreateOrder(input));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Xem danh sách Order
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("find-all")]
    [ProducesResponseType(typeof(ApiResponse<PagingResult<OrderBaseDto>>), StatusCodes.Status200OK)]
    public async Task<ApiResponse> FindAllOrder(FilterOrderDto input)
    {
        try
        {
            return OkResult(await _orderService.FindAllOrder(input));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Xem danh sách Order Pending
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("find-all-order-pending")]
    [ProducesResponseType(typeof(ApiResponse<PagingResult<OrderBaseDto>>), StatusCodes.Status200OK)]
    public async Task<ApiResponse> FindAllOrderPending(PagingRequestBaseDto input)
    {
        try
        {
            return OkResult(await _orderService.FindAllOrderPending(input));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Tìm kiếm order theo Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("find/{id}")]
    [ProducesResponseType(typeof(ApiResponse<Result<OrderBaseDto>>), StatusCodes.Status200OK)]
    public async Task<ApiResponse> FindOrderById(int id)
    {
        try
        {
            return OkResult(await _orderService.FindById(id));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Cập nhật order
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("update")]
    [ProducesResponseType(typeof(ApiResponse<UpdateOrderResultDto>), StatusCodes.Status200OK)]
    public async Task<ApiResponse> UpdateOrder(UpdateOrderDto input)
    {
        try
        {
            return OkResult(await _orderService.UpdateOrder(input));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
        //cập nhật thiết kế cho các item trong đơn, cập nhật các thông tin sku, kích thước, tag, note
    }

    /// <summary>
    /// Đẩy đơn hàng sang cho sản xuất
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("approve-order")]
    public async Task<ApiResponse> ApproveOrder(int id)
    {
        try
        {
            return OkResult(await _orderService.ApproveOrder(id));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Xóa Order đồng thời xóa cả những orderDetail trong nó trạng thái pending
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("delete/{id}")]
    public async Task<ApiResponse> DeleteOrder(int id)
    {
        try
        {
            return OkResult(await _orderService.Delete(id));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }
}
