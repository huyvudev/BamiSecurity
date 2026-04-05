using CR.Core.ApplicationServices.PartnerModule.Abstracts;
using CR.Core.Dtos.PartnerModule.Partner;
using CR.Core.Dtos.PartnerModule.PartnerType;
using CR.DtoBase;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Partner
{
    /// <summary>
    /// Quản lý đối tác
    /// </summary>
    [Authorize]
    [Route("api/partner")]
    [ApiController]
    public class PartnerController : ApiControllerBase
    {
        private readonly IPartnerService _partnerService;

        public PartnerController(ILogger<PartnerController> logger, IPartnerService partnerService)
            : base(logger)
        {
            _partnerService = partnerService;
        }

        /// <summary>
        /// Xem danh sách Partner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(
            typeof(ApiResponse<PagingResult<PartnerDto>>),
            StatusCodes.Status200OK
        )]
        public async Task<ApiResponse> FindAll(FilterPartnerDto input)
        {
            try
            {
                return OkResult(await _partnerService.FindAll(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm partner theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        [ProducesResponseType(typeof(ApiResponse<Result<PartnerDto>>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> FindById(int id)
        {
            try
            {
                return OkResult(await _partnerService.GetById(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm Partner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResponse<CreatePartnerResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> CreatePartner(CreatePartnerDto input)
        {
            try
            {
                return OkResult(await _partnerService.CreatePartner(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật Partner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse<UpdatePartnerResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Update(UpdatePartnerDto input)
        {
            try
            {
                return OkResult(await _partnerService.Update(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa Partner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                return OkResult(await _partnerService.Delete(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
