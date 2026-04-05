using CR.Core.ApplicationServices.ItemModule.Abstracts;
using CR.Core.Dtos.Note;
using CR.Core.Dtos.OrderModule.Item;
using CR.Core.Dtos.OrderModule.Order;
using CR.DtoBase;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Item
{
    /// <summary>
    /// Quản lý item
    /// </summary>
    [Authorize]
    [Route("api/item")]
    [ApiController]
    public class OrderItemController : ApiControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(ILogger<OrderItemController> logger, IOrderItemService itemService)
            : base(logger)
        {
            _orderItemService = itemService;
        }
        /// <summary>
        /// Danh sách các order item ( chung đơn hàng xếp liền nhau, index giảm dần, thời gian tạo giảm dần)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-order-item")]
        [ProducesResponseType(typeof(ApiResponse<PagingResult<OrderItemDto>>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> FindAllOrderItem(FilterOrderItemDto input)
        {
            try
            {
                return OkResult(await _orderItemService.FindAllOrderItem(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm OrderItem theo Id
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <returns></returns>
        [HttpGet("find-order-item-by-id/{orderItemId}")]
        [ProducesResponseType(typeof(ApiResponse<OrderItemDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> FindOrderItemById(int orderItemId)
        {
            try
            {
                return OkResult(await _orderItemService.FindOrderItemById(orderItemId));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm OrderItem bằng mã Template
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-order-item-by-template")]
        [ProducesResponseType(typeof(ApiResponse<OrderItemDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> FindOrderItemByTemplate(FindOrderItemDto input)
        {
            try
            {
                return OkResult(await _orderItemService.FindOrderItemByTemplate(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật ghi chú cho item
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-note")]
        public async Task<ApiResponse> UpdateNote(UpdateNoteDto input)
        {
            try
            {
                return OkResult(await _orderItemService.UpdateNote(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}