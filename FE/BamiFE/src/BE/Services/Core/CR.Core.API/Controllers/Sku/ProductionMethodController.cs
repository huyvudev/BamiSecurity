using CR.Core.ApplicationServices.SkuModule.ProductionMethod.Abstacts;
using CR.Core.Dtos.SkuModule.ProductionMethod;
using CR.DtoBase;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.ProductionMethod
{
    /// <summary>
    /// Quản lý phương thức sản xuất
    /// </summary>
    [Authorize]
    [Route("api/sku/production-method")]
    [ApiController]
    public class ProductionMethodController : ApiControllerBase
    {
        private readonly IProductMethodService _productionMethodService;

        public ProductionMethodController(ILogger<ProductionMethodController> logger, IProductMethodService productionMethodService) : base(logger)
        {
            _productionMethodService = productionMethodService;
        }

        /// <summary>
        /// Lấy danh sách phương thức sản xuất
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(ApiResponse<PagingResult<ProductionMethodDto>>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> FindAll(PagingRequestBaseDto input)
        {
            try
            {
                return OkResult(await _productionMethodService.FindAll(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm phương thức sản xuất theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProductionMethodDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Find(int id)
        {
            try
            {
                return OkResult(await _productionMethodService.Find(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm mới phương thức sản xuất
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResponse<CreateProductionMethodResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Create(CreateProductionMethodDto input)
        {
            try
            {
                return OkResult(await _productionMethodService.Create(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật phương thức sản xuất
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse<UpdateProductionMethodResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Update(UpdateProductionMethodDto input)
        {
            try
            {
                return OkResult(await _productionMethodService.Update(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa phương thức sản xuất
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                return OkResult(await _productionMethodService.Delete(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
