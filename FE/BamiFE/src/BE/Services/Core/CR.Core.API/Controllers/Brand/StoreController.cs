using CR.Core.ApplicationServices.BrandModule.Abstracts;
using CR.Core.Domain.Users;
using CR.Core.Dtos.BrandModule.Store;
using CR.DtoBase;
using CR.UserRolePermission;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.BrandControllers
{
    /// <summary>
    /// Quản lý cửa hàng
    /// </summary>
    [Authorize]
    [Route("api/brand/store")]
    [AuthorizeAdminUserTypeFilter]
    [ApiController]
    public class StoreController : ApiControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(ILogger<StoreController> logger, IStoreService storeService)
            : base(logger)
        {
            _storeService = storeService;
        }

        /// <summary>
        /// Xem danh sách Store
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(ApiResponse<PagingResult<StoreDto>>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> FindAll(PagingRequestBaseDto input)
        {
            try
            {
                return OkResult(await _storeService.FindAll(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm store theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        [ProducesResponseType(typeof(ApiResponse<User>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> FindById(int id)
        {
            try
            {
                return OkResult(await _storeService.GetById(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm Store
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResponse<CreateStoreResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> CreateBrand(CreateStoreDto input)
        {
            try
            {
                return OkResult(await _storeService.CreateStore(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật Store
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse<UpdateStoreResultDto>), StatusCodes.Status200OK)]
        public async Task<ApiResponse> Update(UpdateStoreDto input)
        {
            try
            {
                return OkResult(await _storeService.Update(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa Store
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                return OkResult(await _storeService.Delete(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
