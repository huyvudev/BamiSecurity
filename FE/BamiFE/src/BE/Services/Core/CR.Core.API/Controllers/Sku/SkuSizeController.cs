using CR.Core.ApplicationServices.SkuModule.SkuSize.Abstracts;
using CR.Core.Dtos.SkuModule.SkuSize;
using CR.DtoBase;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Sku
{
    /// <summary>
    /// Quản lý Sku Size
    /// </summary>
    [Authorize]
    [Route("api/sku/sku-size")]
    [ApiController]
    public class SkuSizeController : ApiControllerBase
    {
        private readonly ISkuSizeService _skuSizeService;

        public SkuSizeController(ILogger<SkuSizeController> logger, ISkuSizeService skuSizeService) : base(logger)
        {
            _skuSizeService = skuSizeService;
        }

        /// <summary>
        /// Lấy danh sách Sku Size
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(ApiResponse<PagingResult<SkuSizeDto>>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> FindAll(PagingRequestBaseDto input)
        {
            try
            {
                return OkResult(await _skuSizeService.FindAll(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm Sku Size theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        [ProducesResponseType(typeof(ApiResponse<SkuSizeDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Find(int id)
        {
            try
            {
                return OkResult(await _skuSizeService.Find(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm mới Sku Size
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResponse<CreateSkuSizeResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Create(CreateSkuSizeDto input)
        {
            try
            {
                return OkResult(await _skuSizeService.Create(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật Sku Size
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse<UpdateSkuSizeResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Update(UpdateSkuSizeDto input)
        {
            try
            {
                return OkResult(await _skuSizeService.Update(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa Sku Size
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                return OkResult(await _skuSizeService.Delete(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
