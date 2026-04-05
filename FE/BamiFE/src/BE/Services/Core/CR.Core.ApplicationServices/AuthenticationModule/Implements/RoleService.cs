using CR.ApplicationBase.Common;
using CR.Constants.Authorization.Role;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.AuthenticationModule.Abstracts;
using CR.Core.ApplicationServices.AuthenticationModule.Dtos.RoleDto;
using CR.Core.ApplicationServices.Common;
using CR.Core.Domain.Users;
using CR.DtoBase;
using CR.InfrastructureBase;
using CR.InfrastructureBase.Exceptions;
using CR.Utils.DataUtils;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.AuthenticationModule.Implements
{
    public class RoleService : CoreServiceBase, IRoleService
    {
        public RoleService(ILogger<RoleService> logger, IHttpContextAccessor httpContext)
            : base(logger, httpContext) { }

        public async Task<Result> Add(CreateRolePermissionDto input)
        {
            _logger.LogInformation("{MethodName}: input = {@Input}", nameof(Add), input);
            if (await _dbContext.Roles.AnyAsync(s => s.Name == input.Name && !s.Deleted))
            {
                return Result.Failure(ErrorCode.RoleNameExist, this.GetCurrentMethodInfo());
            }
            var roleInsert = new Role { Name = input.Name, Description = input.Description, };
            _dbContext.Add(roleInsert);
            await _dbContext.SaveChangesAsync();

            foreach (var item in input.PermissionKeys.Where(i => i != null))
            {
                _dbContext.RolePermissions.Add(
                    new RolePermission { RoleId = roleInsert.Id, PermissionKey = item! }
                );
            }
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> Delete(int id)
        {
            _logger.LogInformation("{MethodName}: id = {Id}", nameof(Delete), id);
            var role = await _dbContext.Roles.FirstOrDefaultAsync(e => e.Id == id && !e.Deleted);
            if (role == null)
            {
                return Result.Failure(ErrorCode.RoleNotFound, this.GetCurrentMethodInfo());
            }
            role.Deleted = true;
            var rolePermission = _dbContext.RolePermissions.Where(e => e.RoleId == id);
            foreach (var item in rolePermission)
            {
                _dbContext.Remove(item);
            }
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<PagingResult<RoleDto>>> FindAll(FilterRoleDto input)
        {
            _logger.LogInformation("{MethodName}: input = {@Input}", nameof(FindAll), input);
            var userRoleFind =
                from user in _dbContext.Users
                join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                where !user.Deleted && !userRole.Deleted
                select userRole;

            var rolePermissions =
                from role in _dbContext.Roles
                join user in _dbContext.Users on role.CreatedBy equals user.Id
                where
                    !role.Deleted
                    && !user.Deleted
                    && (input.UserType == null || role.UserType == input.UserType)
                    && (role.PermissionInWeb == PermissionInWebs.Core)
                    && (input.Status == null || role.Status == input.Status)
                    && (input.Keyword == null || role.Name.Contains(input.Keyword.ToLower()))
                select new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    UserType = role.UserType,
                    Description = role.Description,
                    PermissionInWeb = role.PermissionInWeb,
                    Status = role.Status,
                    TotalUse = userRoleFind
                        .Where(u => u.RoleId == role.Id)
                        .Select(u => u.UserId)
                        .Distinct()
                        .Count(),
                    CreatedDate = role.CreatedDate,
                    CreatedByUserName = user.Username
                };
            var result = new PagingResult<RoleDto>();
            result.TotalItems = await rolePermissions.CountAsync();
            result.Items = await rolePermissions.PagingAndSorting(input).ToArrayAsync();
            return Result.Success(result);
        }

        public async Task<Result<RoleDto>> FindById(int id)
        {
            _logger.LogInformation("{MethodName}: id = {Id}", nameof(FindById), id);
            var role = await _dbContext
                .Roles.Where(e => e.Id == id && !e.Deleted)
                .Select(e => new RoleDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    UserType = e.UserType,
                    PermissionInWeb = e.PermissionInWeb,
                    Status = e.Status,
                    TotalUse = _dbContext.UserRoles.Count(u => u.RoleId == e.Id),
                    CreatedDate = e.CreatedDate,
                    PermissionKeys = e.RolePermissions.Select(rp => rp.PermissionKey).ToList()
                })
                .FirstOrDefaultAsync();
            if (role == null)
            {
                return Result<RoleDto>.Failure(ErrorCode.RoleNotFound, this.GetCurrentMethodInfo());
            }
            return Result.Success(role);
        }

        public async Task<Result<RoleDto>> Update(UpdateRolePermissionDto input)
        {
            _logger.LogInformation("{MethodName}: input = {@Input}", nameof(Update), input);
            var role = await _dbContext.Roles.FirstOrDefaultAsync(e =>
                e.Id == input.Id && !e.Deleted
            );
            if (role == null)
            {
                return Result<RoleDto>.Failure(ErrorCode.RoleNotFound, this.GetCurrentMethodInfo());
            }
            if (
                await _dbContext.Roles.AnyAsync(s =>
                    s.Id != role.Id && s.Name == input.Name && !s.Deleted
                )
            )
            {
                return Result<RoleDto>.Failure(
                    ErrorCode.RoleNameExist,
                    this.GetCurrentMethodInfo()
                );
            }
            role.Name = input.Name;
            role.Description = input.Description;

            //List permission có trong db
            var currentListPermission = _dbContext.RolePermissions.Where(e => e.RoleId == input.Id);

            //List Rolepermission bị xóa
            var removeListPermission = currentListPermission.Where(e =>
                input.PermissionKeysRemove.Contains(e.PermissionKey)
            );
            foreach (var item in removeListPermission)
            {
                _dbContext.RolePermissions.Remove(item);
            }

            foreach (var item in input.PermissionKeys)
            {
                //Thêm các rolepermission với các permission key từ input vào List permission input
                var rolePermission = await _dbContext.RolePermissions.FirstOrDefaultAsync(e =>
                    e.RoleId == input.Id && e.PermissionKey == item
                );
                if (rolePermission == null && item != null)
                {
                    _dbContext.RolePermissions.Add(
                        new RolePermission { RoleId = input.Id, PermissionKey = item, }
                    );
                }
            }
            await _dbContext.SaveChangesAsync();
            return Result<RoleDto>.Success(
                new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    UserType = role.UserType,
                    PermissionInWeb = role.PermissionInWeb,
                    Status = role.Status,
                    TotalUse = await _dbContext.UserRoles.CountAsync(u => u.RoleId == role.Id),
                    CreatedDate = role.CreatedDate,
                }
            );
        }

        public async Task<Result> ChangeStatus(int id)
        {
            _logger.LogInformation("{MethodName}: id = {Id}", nameof(ChangeStatus), id);
            var role = await _dbContext.Roles.FirstOrDefaultAsync(e => e.Id == id && !e.Deleted);
            if (role == null)
            {
                return Result<RoleDto>.Failure(ErrorCode.RoleNotFound, this.GetCurrentMethodInfo());
            }

            if (role.Status == RoleStatus.ACTIVE)
            {
                role.Status = RoleStatus.DEACTIVE;
            }
            else
            {
                role.Status = RoleStatus.ACTIVE;
            }
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<IEnumerable<int>>> FindAllWebByUser()
        {
            var userId = _httpContext.GetCurrentUserId();
            var result = await (
                from userRole in _dbContext.UserRoles
                join role in _dbContext.Roles on userRole.RoleId equals role.Id
                where userRole.UserId == userId && !userRole.Deleted && !role.Deleted
                select role.PermissionInWeb
            ).ToListAsync();
            return Result<IEnumerable<int>>.Success(result.Distinct());
        }

        public async Task<Result<IEnumerable<RoleDto>>> FindRoleByUser(int userId)
        {
            _logger.LogInformation(
                "{MethodName}: userId = {UserId}",
                nameof(FindRoleByUser),
                userId
            );

            var result =
                from user in _dbContext.Users
                join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                join role in _dbContext.Roles on userRole.RoleId equals role.Id
                where
                    !user.Deleted
                    && !userRole.Deleted
                    && user.Id == userId
                    && !role.Deleted
                    && role.Status == RoleStatus.ACTIVE
                select new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    UserType = role.UserType,
                    Description = role.Description,
                    PermissionInWeb = role.PermissionInWeb,
                    Status = role.Status
                };
            return Result<IEnumerable<RoleDto>>.Success(await result.ToListAsync());
        }

        public async Task<Result<IEnumerable<RoleDto>>> GetAll()
        {
            var userRoleFind =
                from user in _dbContext.Users
                join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                where !user.Deleted && !userRole.Deleted
                select userRole;

            var result =
                from role in _dbContext.Roles
                where !role.Deleted && role.Status == RoleStatus.ACTIVE
                select new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    UserType = role.UserType,
                    Description = role.Description,
                    PermissionInWeb = role.PermissionInWeb,
                    Status = role.Status,
                    TotalUse = userRoleFind
                        .Where(u => u.RoleId == role.Id)
                        .Select(u => u.UserId)
                        .Distinct()
                        .Count()
                };
            return Result<IEnumerable<RoleDto>>.Success(await result.ToListAsync());
        }
    }
}
