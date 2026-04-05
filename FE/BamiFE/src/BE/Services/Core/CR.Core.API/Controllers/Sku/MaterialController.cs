using CR.Core.ApplicationServices.SkuModule.Material.Abstracts;
using CR.Core.Dtos.SkuModule.Material;
using CR.DtoBase;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Material
{
    /// <summary>
    /// Quản lý vật liệu
    /// </summary>
    [Authorize]
    [Route("api/sku/material")]
    [ApiController]
    public class MaterialController : ApiControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialController(ILogger<MaterialController> logger, IMaterialService materialService) : base(logger)
        {
            _materialService = materialService;
        }

        /// <summary>
        /// Lấy danh sách vật liệu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(ApiResponse<PagingResult<MaterialDto>>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> FindAll(PagingRequestBaseDto input)
        {
            try
            {
                return OkResult(await _materialService.FindAll(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm vật liệu theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MaterialDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Find(int id)
        {
            try
            {
                return OkResult(await _materialService.Find(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm mới vật liệu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResponse<CreateMaterialResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Create(CreateMaterialDto input)
        {
            try
            {
                return OkResult(await _materialService.Create(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật vật liệu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse<UpdateMaterialResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Update(UpdateMaterialDto input)
        {
            try
            {
                return OkResult(await _materialService.Update(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa vật liệu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                return OkResult(await _materialService.Delete(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
