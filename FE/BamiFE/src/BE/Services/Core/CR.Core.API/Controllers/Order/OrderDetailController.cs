using CR.Core.ApplicationServices.OrderModule.Abstracts;
using CR.Core.Dtos.OrderModule.OrderDetail;
using CR.DtoBase;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Order
{
    /// <summary>
    /// Quản lý Order Detail
    /// </summary>
    [Authorize]
    [Route("api/order-detail")]
    [ApiController]
    public class OrderDetailController : ApiControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(
            ILogger<OrderController> logger,
            IOrderDetailService orderDetailService
        )
            : base(logger)
        {
            _orderDetailService = orderDetailService;
        }

        /// <summary>
        /// Xem danh sách OrderDetailDone
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-order-detail-done")]
        [ProducesResponseType(
            typeof(ApiResponse<PagingResult<OrderDetailBaseDto>>),
            StatusCodes.Status200OK
        )]
        public async Task<ApiResponse> FindAllOrderDetailDone(PagingRequestBaseDto input)
        {
            try
            {
                return OkResult(await _orderDetailService.FindAllOrderDetailDone(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem danh sách OrderDetail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-order-detail")]
        [ProducesResponseType(
            typeof(ApiResponse<PagingResult<OrderDetailBaseDto>>),
            StatusCodes.Status200OK
        )]
        public async Task<ApiResponse> FindAllOrderDetail(FilterOrderDetailDto input)
        {
            try
            {
                return OkResult(await _orderDetailService.FindAllOrderDetail(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm order detail theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        [ProducesResponseType(
            typeof(ApiResponse<Result<OrderDetailBaseDto>>),
            StatusCodes.Status200OK
        )]
        public async Task<ApiResponse> FindById(int id)
        {
            try
            {
                return OkResult(await _orderDetailService.FindByIdDetailDone(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        ///Cập nhật order detail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(
            typeof(ApiResponse<UpdateOrderDetailResultDto>),
            StatusCodes.Status200OK
        )]
        public async Task<ApiResponse> UpdateOrderDetail(UpdateOrderDetailBaseDto input)
        {
            try
            {
                return OkResult(await _orderDetailService.UpdateOrderDetail(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
            //cập nhật thiết kế cho các item trong đơn, cập nhật các thông tin sku, kích thước, tag, note
        }

        /// <summary>
        ///Cập nhật thông tin cơ bản order detail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-basic-information")]
        [ProducesResponseType(
            typeof(ApiResponse<UpdateOrderDetailResultDto>),
            StatusCodes.Status200OK
        )]
        public async Task<ApiResponse> UpdateBasicInformationForOrderDetail(
            UpdateBasicInformationForOrderDetailDto input
        )
        {
            try
            {
                return OkResult(await _orderDetailService.UpdateDetailDone(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
            //cập nhật thiết kế cho các item trong đơn, cập nhật các thông tin sku, kích thước, tag, note
        }

        /// <summary>
        ///Duyệt order detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("approve/{id}")]
        [ProducesResponseType(
            typeof(ApiResponse<ApproveOrderDetailResultDto>),
            StatusCodes.Status200OK
        )]
        public async Task<ApiResponse> ApproveOrderDetail(int id)
        {
            try
            {
                return OkResult(await _orderDetailService.ApproveOrderDetail(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa Order Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                return OkResult(await _orderDetailService.Delete(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo Order Detail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(
            typeof(ApiResponse<CreateOrderDetailResultDto>),
            StatusCodes.Status200OK
        )]
        public async Task<ApiResponse> Create(AddOrderDetailDto input)
        {
            try
            {
                return OkResult(await _orderDetailService.Create(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
