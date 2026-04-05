using CR.Core.ApplicationServices.SkuModule.Sku.Abstracts;
using CR.Core.Dtos.SkuModule.Sku;
using CR.DtoBase;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Sku
{
    /// <summary>
    /// Quản lý SKU
    /// </summary>
    [Authorize]
    [Route("api/sku")]
    [ApiController]
    public class SkuController : ApiControllerBase
    {
        private readonly ISkuService _skuService;
        public SkuController(ILogger<SkuController> logger, ISkuService skuService) : base(logger)
        {
            _skuService = skuService;
        }

        /// <summary>
        /// Lấy danh sách SKU
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(ApiResponse<PagingResult<SkuDto>>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> FindAll(PagingRequestBaseDto input)
        {
            try
            {
                return OkResult(await _skuService.FindAll(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm SKU theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        [ProducesResponseType(typeof(ApiResponse<SkuDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Find(int id)
        {
            try
            {
                return OkResult(await _skuService.Find(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm mới SKU
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResponse<CreateSkuResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Create(CreateSkuDto input)
        {
            try
            {
                return OkResult(await _skuService.Create(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật SKU
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse<UpdateSkuResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Update(UpdateSkuDto input)
        {
            try
            {
                return OkResult(await _skuService.Update(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa SKU
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                return OkResult(await _skuService.Delete(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
