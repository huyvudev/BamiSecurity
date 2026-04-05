using System.Text.Json;
using CR.Constants.Authorization.Role;
using CR.Constants.Core.Users;
using CR.Constants.ErrorCodes;
using CR.Constants.RolePermission;
using CR.Core.ApplicationServices.AuthenticationModule.Abstracts;
using CR.Core.ApplicationServices.AuthenticationModule.Dtos.PermissionDto;
using CR.Core.ApplicationServices.Common;
using CR.InfrastructureBase;
using CR.InfrastructureBase.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.AuthenticationModule.Implements
{
    public class PermissionService : CoreServiceBase, IPermissionService
    {
        public PermissionService(
            ILogger<PermissionService> logger,
            IHttpContextAccessor httpContext
        )
            : base(logger, httpContext) { }

        public bool CheckPermission(params string[] permissionKeys)
        {
            var userId = _httpContext.GetCurrentUserId();
            var userType = _httpContext.GetCurrentUserType();
            _logger.LogInformation(
                $"{nameof(CheckPermission)}: "
                    + $"permissionKey = {JsonSerializer.Serialize(permissionKeys)},"
                    + $" userId = {userId}, userType = {userType}"
            );

            return userType == UserTypes.SUPER_ADMIN
                || (
                    from userRole in _dbContext.UserRoles
                    join role in _dbContext.Roles on userRole.RoleId equals role.Id
                    join rolePermission in _dbContext.RolePermissions
                        on role.Id equals rolePermission.RoleId
                    where
                        userRole.UserId == userId
                        && !role.Deleted
                        && !userRole.Deleted
                        && permissionKeys.Contains(rolePermission.PermissionKey)
                    select rolePermission.PermissionKey
                ).Any();
        }

        public IEnumerable<string> GetPermissionInternalService(int? permissionInWeb, int userId)
        {
            _logger.LogInformation(
                $"{nameof(GetPermissionInternalService)}: permissionInWeb = {permissionInWeb}, userId = {userId}"
            );
            var user =
                _dbContext
                    .Users.Select(x => new { x.Id, x.UserType })
                    .FirstOrDefault(x => x.Id == userId)
                ?? throw new UserFriendlyException(ErrorCode.UserNotFound);
            return GetPermissionByUser(userId, user.UserType, permissionInWeb);
        }

        /// <summary>
        /// Lấy danh sách các quyền theo user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="permissionInWeb"></param>
        /// <returns></returns>
        private List<string> GetPermissionByUser(int userId, UserTypeEnum userType, int? permissionInWeb)
        {
            if (userType == UserTypeEnum.SUPER_ADMIN)
            {
                if (!permissionInWeb.HasValue || permissionInWeb == PermissionInWebs.Core)
                {
                    return PermissionConfig.CoreConfigs.Select(o => o.Key).ToList();
                }
            }
            return
            [
                .. _dbContext.UserRoles
                    .Include(x => x.Role)
                    .ThenInclude(x => x.RolePermissions)
                    .Where(x =>
                            (!permissionInWeb.HasValue || x.Role.PermissionInWeb == permissionInWeb)
                            && x.UserId == userId
                            && !x.Role.Deleted
                            && !x.Deleted)
                    .SelectMany(x => x.Role.RolePermissions.Select(x => x.PermissionKey))
            ];
        }

        public IEnumerable<string> GetPermissionInWeb(int? permissionInWeb)
        {
            var userId = _httpContext.GetCurrentUserId();
            var userType = _httpContext.GetCurrentUserType();
            _logger.LogInformation(
                $"{nameof(GetPermissionInWeb)}: userId = {userId}, userType = {userType}, permissionInWeb = {permissionInWeb}"
            );
            return GetPermissionByUser(userId, (UserTypeEnum)userType, permissionInWeb);
        }

        public IEnumerable<PermissionDetailDto> FindAllPermission(int permissionConfig)
        {
            _logger.LogInformation($"{nameof(FindAllPermission)}");
            return permissionConfig switch
            {
                PermissionInWebs.Core
                    => PermissionConfig.CoreConfigs.Select(x => new PermissionDetailDto
                    {
                        Key = x.Key,
                        ParentKey = x.Value.ParentKey,
                        Label = L(x.Value.LName),
                        Icon = x.Value.Icon
                    }),
                _ => [],
            };
        }

        //public IEnumerable<PermissionInWebDto> FindByPermissionInWeb()
        //{
        //    var query = _dbContext
        //        .Roles.Where(r => !r.Deleted)
        //        .GroupBy(r => r.PermissionInWeb)
        //        .Select(roles => new PermissionInWebDto
        //        {
        //            PermissionInWeb = roles.Key,
        //            TotalRole = roles.Select(o => o.Id).Count(),
        //            TotalUser = (
        //                from role in roles
        //                join userRole in _dbContext.UserRoles on role.Id equals userRole.RoleId
        //                join user in _dbContext.Users on userRole.UserId equals user.Id
        //                where !userRole.Deleted && !user.Deleted
        //                select userRole.UserId
        //            ).Count(),
        //        });
        //    return query;
        //}
    }
}
