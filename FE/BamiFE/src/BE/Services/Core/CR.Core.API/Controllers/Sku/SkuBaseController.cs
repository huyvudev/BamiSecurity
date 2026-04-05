using CR.Core.ApplicationServices.SkuModule.SkuBase.Abstracts;
using CR.Core.Dtos.SkuModule.SkuBase;
using CR.DtoBase;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.SkuBase
{
    /// <summary>
    /// Quản lý SKU Base
    /// </summary>
    [Authorize]
    [Route("api/sku/sku-base")]
    [ApiController]
    public class SkuBaseController : ApiControllerBase
    {
        private readonly ISkuBaseService _skuBaseService;

        public SkuBaseController(ILogger<SkuBaseController> logger, ISkuBaseService skuBaseService)
            : base(logger)
        {
            _skuBaseService = skuBaseService;
        }

        /// <summary>
        /// Lấy danh sách SKU Base
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(
            typeof(ApiResponse<PagingResult<SkuBaseDto>>),
            StatusCodes.Status200OK
        )]
        public async Task<ApiResponse> FindAll(PagingRequestBaseDto input)
        {
            try
            {
                return OkResult(await _skuBaseService.FindAll(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm SKU Base theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        [ProducesResponseType(typeof(ApiResponse<SkuBaseDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Find(int id)
        {
            try
            {
                return OkResult(await _skuBaseService.Find(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm mới SKU Base
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResponse<CreateSkuBaseResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Create(CreateSkuBaseDto input)
        {
            try
            {
                return OkResult(await _skuBaseService.Create(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật SKU Base
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse<UpdateSkuBaseResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Update(UpdateSkuBaseDto input)
        {
            try
            {
                return OkResult(await _skuBaseService.Update(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa SKU Base
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                return OkResult(await _skuBaseService.Delete(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
