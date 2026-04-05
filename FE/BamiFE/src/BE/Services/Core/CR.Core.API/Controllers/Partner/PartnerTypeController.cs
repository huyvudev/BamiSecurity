using CR.Core.ApplicationServices.PartnerModule.Abstracts;
using CR.Core.Dtos.BrandModule.Brand;
using CR.Core.Dtos.PartnerModule.PartnerType;
using CR.DtoBase;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Partner
{
    /// <summary>
    /// Quản lý kiểu đối tác
    /// </summary>
    [Authorize]
    [Route("api/partner/partner-type")]
    [ApiController]
    public class PartnerTypeController : ApiControllerBase
    {
        private readonly IPartnerTypeService _partnerTypeService;

        public PartnerTypeController(
            ILogger<PartnerTypeController> logger,
            IPartnerTypeService partnerTypeService
        )
            : base(logger)
        {
            _partnerTypeService = partnerTypeService;
        }

        /// <summary>
        /// Xem danh sách partnerType
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(
            typeof(ApiResponse<PagingResult<PartnerTypeDto>>),
            StatusCodes.Status200OK
        )]
        public async Task<ApiResponse> FindAll(PagingRequestBaseDto input)
        {
            try
            {
                return OkResult(await _partnerTypeService.FindAll(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm partnerType theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        [ProducesResponseType(typeof(ApiResponse<Result<BrandDto>>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> FindById(int id)
        {
            try
            {
                return OkResult(await _partnerTypeService.GetById(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm PartnerType
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResponse<CreateBrandResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> CreatePartnerType(CreatePartnerTypeDto input)
        {
            try
            {
                return OkResult(await _partnerTypeService.CreatePartnerType(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật PartnerType
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(
            typeof(ApiResponse<UpdatePartnerTypeResultDto>),
            StatusCodes.Status200OK
        )]
        public async Task<ApiResponse> Update(UpdatePartnerTypeDto input)
        {
            try
            {
                return OkResult(await _partnerTypeService.Update(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa PartnerType
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                return OkResult(await _partnerTypeService.Delete(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
