using System.Net;
using CR.Common.Filters;
using CR.Constants.RolePermission.Constant;
using CR.Core.ApplicationServices.AuthenticationModule.Abstracts;
using CR.Core.ApplicationServices.AuthenticationModule.Dtos.RoleDto;
using CR.Core.Domain.Users;
using CR.UserRolePermission;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Core
{
    [Authorize]
    [Route("api/auth/role")]
    [AuthorizeAdminUserTypeFilter]
    [ApiController]
    public class RoleController : ApiControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(ILogger<RoleController> logger, IRoleService roleService)
            : base(logger)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Thêm Role
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), (int)HttpStatusCode.OK)]
        [PermissionFilter(PermissionKeys.CoreButtonAddRole)]
        public async Task<ApiResponse> Add(CreateRolePermissionDto input)
        {
            try
            {
                return OkResult(await _roleService.Add(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// update Role
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), (int)HttpStatusCode.OK)]
        [PermissionFilter(PermissionKeys.CoreButtonUpdateRole)]
        public async Task<ApiResponse> Update(UpdateRolePermissionDto input)
        {
            try
            {
                return OkResult(await _roleService.Update(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa Role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), (int)HttpStatusCode.OK)]
        [PermissionFilter(PermissionKeys.CoreButtonDeleteRole)]
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                return OkResult(await _roleService.Delete(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Find Role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), (int)HttpStatusCode.OK)]
        [PermissionFilter(PermissionKeys.CoreButtonUpdateRole)]
        public async Task<ApiResponse> FindById(int id)
        {
            try
            {
                return OkResult(await _roleService.FindById(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem danh sách Role
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), (int)HttpStatusCode.OK)]
        [PermissionFilter(
            PermissionKeys.CoreMenuRole
        )]
        public async Task<ApiResponse> FindAll([FromQuery] FilterRoleDto input)
        {
            try
            {
                return OkResult(await _roleService.FindAll(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Khoá role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-status/{id}")]
        [ProducesResponseType(typeof(ApiResponse<Role>), (int)HttpStatusCode.OK)]
        [PermissionFilter(PermissionKeys.CoreButtonActiveDeactiveRole)]
        public async Task<ApiResponse> ChangeStatus(int id)
        {
            try
            {
                return OkResult(await _roleService.ChangeStatus(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách web user có quyền
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all-web-user")]
        [ProducesResponseType(typeof(ApiResponse<User>), (int)HttpStatusCode.OK)]
        public async Task<ApiResponse> FindAllWebByUser()
        {
            try
            {
                return OkResult(await _roleService.FindAllWebByUser());
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm role theo user
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-by-user/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), (int)HttpStatusCode.OK)]
        [PermissionFilter(PermissionKeys.CoreButtonUpdateAccount)]
        public async Task<ApiResponse> FindRoleByUser(int userId)
        {
            try
            {
                return OkResult(await _roleService.FindRoleByUser(userId));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách tất cả role không chia quyền theo web
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), (int)HttpStatusCode.OK)]
        [PermissionFilter(PermissionKeys.CoreButtonAddAccount, PermissionKeys.CoreButtonUpdateAccount)]
        public async Task<ApiResponse> GetAll()
        {
            try
            {
                return OkResult(await _roleService.GetAll());
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
