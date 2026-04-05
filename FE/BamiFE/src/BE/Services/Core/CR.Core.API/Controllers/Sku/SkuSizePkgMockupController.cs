using CR.Core.ApplicationServices.SkuModule.SkuSizePkgMockup.Abstracts;
using CR.Core.Dtos.SkuModule.SkuSizePkgMockup;
using CR.DtoBase;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Sku
{
    /// <summary>
    /// Quản lý Sku Size Pkg Mockup
    /// </summary>
    [Authorize]
    [Route("api/sku/sku-size-pkg-mockup")]
    [ApiController]
    public class SkuSizePkgMockupController : ApiControllerBase
    {
        private readonly ISkuSizePkgMockupService _skuSizePkgMockupService;

        public SkuSizePkgMockupController(ILogger<SkuSizePkgMockupController> logger, ISkuSizePkgMockupService skuSizePkgMockupService)
            : base(logger)
        {
            _skuSizePkgMockupService = skuSizePkgMockupService;
        }

        /// <summary>
        /// Lấy danh sách Sku Size Pkg Mockup
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(ApiResponse<PagingResult<SkuSizePkgMockupDto>>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> FindAll(PagingRequestBaseDto input)
        {
            try
            {
                return OkResult(await _skuSizePkgMockupService.FindAll(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm Sku Size Pkg Mockup theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        [ProducesResponseType(typeof(ApiResponse<SkuSizePkgMockupDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Find(int id)
        {
            try
            {
                return OkResult(await _skuSizePkgMockupService.Find(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm mới Sku Size Pkg Mockup
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResponse<CreateSkuSizePkgMockupResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Create(CreateSkuSizePkgMockupDto input)
        {
            try
            {
                return OkResult(await _skuSizePkgMockupService.Create(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật Sku Size Pkg Mockup
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse<UpdateSkuSizePkgMockupResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Update(UpdateSkuSizePkgMockupDto input)
        {
            try
            {
                return OkResult(await _skuSizePkgMockupService.Update(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa Sku Size Pkg Mockup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                return OkResult(await _skuSizePkgMockupService.Delete(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
