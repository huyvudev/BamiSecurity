using System.Net;
using CR.Common.Filters;
using CR.Constants.RolePermission.Constant;
using CR.Core.ApplicationServices.AuthenticationModule.Abstracts;
using CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserDto;
using CR.Core.Domain.Users;
using CR.Core.Dtos.AuthenticationModule.UserDto;
using CR.UserRolePermission;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Core;

[Authorize]
[Route("api/core/user")]
[AuthorizeAdminUserTypeFilter]
[ApiController]
public class UserController : ApiControllerBase
{
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
        : base(logger)
    {
        _userService = userService;
    }

    /// <summary>
    /// Xem danh sách User CMS (ADMIN)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("find-all")]
    [ProducesResponseType(typeof(ApiResponse<User>), (int)HttpStatusCode.OK)]
    public async Task<ApiResponse> FindAll([FromQuery] FilterUserPagingDto input)
    {
        try
        {
            return OkResult(await _userService.FindAll(input));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Tìm kiếm người dùng theo Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<User>), (int)HttpStatusCode.OK)]
    public async Task<ApiResponse> FindById(int id)
    {
        try
        {
            return OkResult(await _userService.GetById(id));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Thông tin tài khoản từ userId token
    /// </summary>
    /// <returns></returns>
    [HttpGet("find-by-user")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), (int)HttpStatusCode.OK)]
    public async Task<ApiResponse> FindByUser()
    {
        try
        {
            return OkResult(await _userService.FindByCurrentId());
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Thêm User
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("add")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), (int)HttpStatusCode.OK)]
    [PermissionFilter(PermissionKeys.CoreButtonAddAccount)]
    public async Task<ApiResponse> CreateUser([FromBody] CreateUserDto input)
    {
        try
        {
            return OkResult(await _userService.CreateUser(input));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Cập nhật User
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("update")]
    //[PermissionFilter(PermissionKeys.UserUpdate)]
    public async Task<ApiResponse> Update([FromBody] UpdateUserDto input)
    {
        try
        {
            return OkResult(await _userService.Update(input));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Xóa User
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("delete/{id}")]
    //[PermissionFilter(PermissionKeys.CoreButtonAccountManagerDelete)]
    public async Task<ApiResponse> Delete(int id)
    {
        try
        {
            return OkResult(await _userService.Delete(id));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Thay đổi trạng thái tai khoản
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("change-status/{id}")]
    [PermissionFilter(PermissionKeys.CoreButtonActiveDeactiveAccount)]
    public async Task<ApiResponse> ChangeStatus(int id)
    {
        try
        {
            return OkResult(await _userService.ChangeStatus(id));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Set password cho user
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("set-password")]
    [PermissionFilter(PermissionKeys.CoreButtonSetPasswordAccount)]
    public async Task<ApiResponse> SetPassword(SetPasswordUserDto input)
    {
        try
        {
            return OkResult(await _userService.SetPassword(input));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }
}
