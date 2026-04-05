using System.Net;
using CR.Common.Filters;
using CR.Constants.Authorization.Role;
using CR.Constants.RolePermission.Constant;
using CR.Core.ApplicationServices.AuthenticationModule.Abstracts;
using CR.Core.ApplicationServices.AuthenticationModule.Dtos.PermissionDto;
using CR.DtoBase.Validations;
using CR.UserRolePermission;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Core
{
    [Authorize]
    [Route("api/auth/permission")]
    [ApiController]
    [AuthorizeAdminUserTypeFilter]
    public class PermissionController : ApiControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(
            ILogger<PermissionController> logger,
            IPermissionService permissionService
        )
            : base(logger)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// Xem danh quyền
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(
            typeof(ApiResponse<IEnumerable<PermissionDetailDto>>),
            (int)HttpStatusCode.OK
        )]
        [PermissionFilter(PermissionKeys.CoreMenuRole)]
        public ApiResponse FindAll()
        {
            try
            {
                return new ApiResponse(_permissionService.FindAllPermission(PermissionInWebs.Core));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy permissionKey của người dùng hiện tại
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-permissions")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<string>>), (int)HttpStatusCode.OK)]
        public ApiResponse GetAllPermission()
        {
            try
            {
                return new(_permissionService.GetPermissionInWeb(PermissionInWebs.Core));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy permissionKey của người dùng bất kì
        /// </summary>
        /// <param name="permissionInWeb">Website cần lấy permission</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("get-permissions-internal")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<string>>), (int)HttpStatusCode.OK)]
        public ApiResponse GetPermissionInternalService(int? permissionInWeb, int userId)
        {
            try
            {
                return new(
                    _permissionService.GetPermissionInternalService(permissionInWeb, userId)
                );
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        ///// <summary>
        ///// Lấy số nhóm quyền, số người dùng theo website
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("find-by-permission-in-web")]
        //[ProducesResponseType(
        //    typeof(ApiResponse<IEnumerable<PermissionInWebDto>>),
        //    (int)HttpStatusCode.OK
        //)]
        //[PermissionFilter(PermissionKeys.CoreTablePermission)]
        //public ApiResponse FindByPermissionInWeb()
        //{
        //    try
        //    {
        //        return new(_permissionService.FindByPermissionInWeb());
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}
    }
}
