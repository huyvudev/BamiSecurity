using CR.Core.ApplicationServices.BrandModule.Abstracts;
using CR.Core.Dtos.BrandModule.Brand;
using CR.DtoBase;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.BrandControllers
{
    /// <summary>
    /// Quản lý nhãn hàng
    /// </summary>
    [Authorize]
    [Route("api/brand")]
    [ApiController]
    public class BrandController : ApiControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(ILogger<BrandController> logger, IBrandService brandService)
            : base(logger)
        {
            _brandService = brandService;
        }

        /// <summary>
        /// Xem danh sách Brand
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(ApiResponse<PagingResult<BrandDto>>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> FindAll(PagingRequestBaseDto input)
        {
            try
            {
                return OkResult(await _brandService.FindAll(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm brand theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        [ProducesResponseType(typeof(ApiResponse<Result<BrandDto>>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> FindById(int id)
        {
            try
            {
                return OkResult(await _brandService.GetById(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm Brand
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResponse<CreateBrandResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> CreateBrand(CreateBrandDto input)
        {
            try
            {
                return OkResult(await _brandService.CreateBrand(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật Brand
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse<UpdateBrandResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Update(UpdateBrandDto input)
        {
            try
            {
                return OkResult(await _brandService.Update(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa Brand
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                return OkResult(await _brandService.Delete(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
