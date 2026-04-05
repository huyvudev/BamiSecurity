using CR.ApplicationBase.Common;
using CR.Constants.Common.SysVar;
using CR.Constants.Core.Users;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.AuthenticationModule.Abstracts;
using CR.Core.ApplicationServices.AuthenticationModule.Dtos.RoleDto;
using CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserActionDtos;
using CR.Core.ApplicationServices.AuthenticationModule.Dtos.UserDto;
using CR.Core.ApplicationServices.Common;
using CR.Core.Domain.SysVar;
using CR.Core.Domain.Users;
using CR.Core.Dtos.AuthenticationModule.UserDto;
using CR.DtoBase;
using CR.InfrastructureBase;
using CR.InfrastructureBase.Exceptions;
using CR.Utils.DataUtils;
using CR.Utils.Security;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.AuthenticationModule.Implements
{
    public class UserService : CoreServiceBase, IUserService
    {
        public UserService(
            ILogger<UserService> logger,
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContext
        )
            : base(logger, httpContext) { }

        public async Task<Result> CreateUser(CreateUserDto input)
        {
            _logger.LogInformation("{MethodName}: input = {@Input}", nameof(CreateUser), input);
            input.Password = PasswordHasher.HashPassword(input.Password);
            //Kiểm tra User đã tồn tại chưa
            if (await _dbContext.Users.AnyAsync(u => u.Username == input.Username && !u.Deleted))
            {
                return Result.Failure(ErrorCode.UsernameHasBeenUsed, this.GetCurrentMethodInfo());
            }
            var userRoleIds = (input.RoleIds ?? []).Distinct();
            //Kiểm tra quyền có tồn tại trong DB chưa, nếu 1 Id quyền trong input không tồn tại thì throw Excpt
            if (
                input.RoleIds is not null
                && await _dbContext
                    .Roles.Where(x => userRoleIds.Contains(x.Id) && !x.Deleted)
                    .CountAsync() != input.RoleIds.Count()
            )
            {
                return Result.Failure(ErrorCode.RoleNotFound, this.GetCurrentMethodInfo());
            }
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            //Thêm mới User cùng các quyền tương ứng
            var user = _dbContext.Users.Add(
                new User
                {
                    Username = input.Username,
                    Password = input.Password,
                    UserCode = input.UserCode,
                    S3Key = input.AvatarS3key,
                    IsPasswordTemp = input.IsPasswordTemp,
                    FullName = input.FullName,
                    Email = input.Email,
                    Gender = input.Gender,
                    PhoneNumber = input.PhoneNumber,
                    DateOfBirth = input.DateOfBirth,
                }
            );
            await _dbContext.SaveChangesAsync();
            var userRoles = new List<UserRole>();
            foreach (var item in userRoleIds)
            {
                userRoles.Add(new UserRole { RoleId = item, UserId = user.Entity.Id, });
            }
            _dbContext.UserRoles.AddRange(userRoles);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return Result.Success();
        }

        public async Task<Result<UserDto>> RegisterUser(UserRegisterDto input)
        {
            _logger.LogInformation("{MethodName}: input = {@Input}", nameof(RegisterUser), input);
            if (await _dbContext.Users.AnyAsync(u => (u.Username == input.Email) && !u.Deleted))
            {
                Result.Failure(
                    ErrorCode.UserRegisterExistPersonalEmail,
                    this.GetCurrentMethodInfo()
                );
            }
            var user = _dbContext
                .Users.Add(
                    new User
                    {
                        Username = input.Username,
                        Email = input.Email,
                        Password = PasswordHasher.HashPassword(string.Empty),
                        UserType = UserTypeEnum.CUSTOMER,
                        Status = UserStatus.TEMP,
                        UserCode = input.UserCode,
                    }
                )
                .Entity;
            await _dbContext.SaveChangesAsync();
            return Result<UserDto>.Success(
                new UserDto
                {
                    Id = user.Id,
                    Username = input.Username,
                    Email = input.Email,
                    UserType = UserTypeEnum.CUSTOMER,
                    Status = UserStatus.TEMP
                }
            );
        }

        public async Task<Result> VerifyRegisterOtp(string email, string otpCode)
        {
            _logger.LogInformation(
                "{MethodName}: email = {Email}, otpCode = {OtpCode}",
                nameof(VerifyRegisterOtp),
                email,
                otpCode
            );

            var user = await _dbContext.Users.FirstOrDefaultAsync(u =>
                u.Username == email && u.UserType == UserTypeEnum.CUSTOMER && !u.Deleted
            );
            if (user == null)
            {
                return Result.Failure(ErrorCode.UserNotFound, this.GetCurrentMethodInfo());
            }

            if (user.Status != UserStatus.TEMP)
            {
                return Result.Failure(ErrorCode.UserIsRegistered, this.GetCurrentMethodInfo());
            }

            try
            {
                await Task.CompletedTask;
                //await _otpService.VerifyOtp(user.Username, user.OtpRequestId, otpCode);
            }
            catch
            {
                var otpDefault = FindEntities<SysVar>(e =>
                    e.GrName == GrNames.OTP && e.VarName == VarNames.DEFAULT_OTP
                );
                if (otpDefault != null && otpCode != otpDefault.VarValue)
                {
                    throw;
                }
            }
            user.Status = UserStatus.TEMP;
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<UserDto>> GetById(int id)
        {
            _logger.LogInformation("{MethodName}: id = {Id}", nameof(GetById), id);
            var user = await _dbContext
                .Users.Where(u => u.Id == id && !u.Deleted)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    AvatarImageUri = u.AvatarImageUri,
                    Username = u.Username,
                    UserType = u.UserType,
                    Email = u.Email,
                    Gender = u.Gender,
                    PhoneNumber = u.PhoneNumber,
                    Status = u.Status,
                    IsPasswordTemp = u.IsPasswordTemp,
                })
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return Result<UserDto>.Failure(ErrorCode.UserNotFound, this.GetCurrentMethodInfo());
            }
            return Result<UserDto>.Success(user);
        }

        public async Task<Result<UserDto>> FindByCurrentId()
        {
            var userId = _httpContext.GetCurrentUserId();
            _logger.LogInformation("{MethodName}: ", nameof(GetById));

            var result = await _dbContext
                .Users.AsNoTracking()
                .Include(x => x.UserRoles.Where(ur => !ur.Deleted))
                .ThenInclude(ur => ur.Role)
                .Where(u => u.Id == userId && !u.Deleted && u.Status == UserStatus.ACTIVE)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    AvatarImageUri = u.AvatarImageUri,
                    Username = u.Username,
                    UserType = u.UserType,
                    IsPasswordTemp = u.IsPasswordTemp,
                    Roles = u.UserRoles.Select(x => new UserRoleDto
                    {
                        Id = x.RoleId,
                        Description = x.Role.Description,
                        Name = x.Role.Name,
                        PermissionInWeb = x.Role.PermissionInWeb,
                        Status = x.Role.Status
                    })
                })
                .FirstOrDefaultAsync();
            if (result == null)
            {
                return Result<UserDto>.Failure(ErrorCode.UserNotFound, this.GetCurrentMethodInfo());
            }
            ;
            return Result<UserDto>.Success(result);
        }

        public async Task<Result<PagingResult<UserDto>>> FindAll(FilterUserPagingDto input)
        {
            _logger.LogInformation("{MethodName}: input = {@Input}", nameof(FindAll), input);
            var userQueries = _dbContext
                .Users.AsNoTracking()
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .Where(x =>
                    !x.Deleted
                    && x.UserType == UserTypeEnum.ADMIN
                    && (string.IsNullOrEmpty(input.Username) || x.Username.Contains(input.Username))
                    && (
                        string.IsNullOrEmpty(input.FullName)
                        || (
                            !string.IsNullOrEmpty(x.Username)
                            && x.FullName!.Contains(input.FullName)
                        )
                    )
                    && (!input.Status.HasValue || x.Status == input.Status)
                )
                .Select(x => new UserDto
                {
                    Id = x.Id,
                    AvatarImageUri = x.AvatarImageUri,
                    Username = x.Username,
                    UserType = x.UserType,
                    Status = x.Status,
                    PhoneNumber = x.PhoneNumber,
                    FullName = x.FullName,
                    Email = x.Email,
                    IsPasswordTemp = x.IsPasswordTemp,
                    Gender = x.Gender,
                    Roles = x.UserRoles.Select(s => new UserRoleDto
                    {
                        Id = s.RoleId,
                        Description = s.Role.Description,
                        Name = s.Role.Name,
                        PermissionInWeb = s.Role.PermissionInWeb,
                        Status = s.Role.Status
                    })
                });
            // đếm tổng trước khi phân trang
            PagingResult<UserDto> result =
                new()
                {
                    TotalItems = await userQueries.CountAsync(),
                    Items = await userQueries.Paging(input).ToListAsync()
                };
            return Result<PagingResult<UserDto>>.Success(result);
        }

        public async Task<Result> Update(UpdateUserDto input)
        {
            _logger.LogInformation("{MethodName}: input = {@Input}", nameof(Update), input);
            var user = await _dbContext
                .Users.Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == input.Id && !u.Deleted);
            if (user == null)
            {
                return Result.Failure(ErrorCode.UserNotFound, this.GetCurrentMethodInfo());
            }
            user.FullName = input.FullName;

            //Thêm role
            //if (
            //    input.RoleIds is not null
            //    && _dbContext.Roles.Where(e => input.RoleIds.Contains(e.Id)).Count()
            //        != input.RoleIds.Count
            //)
            //{
            //    throw new UserFriendlyException(ErrorCode.RoleNotFound);
            //}

            ////Xóa những role gán với user
            //var removeUserRole = user.UserRoles.ExceptBy(input.RoleIds ?? [], e => e.Id);
            //foreach (var item in removeUserRole)
            //{
            //    item.Deleted = true;
            //}

            //Thêm những role trong input chưa  có trong db
            //var insertUserRole = input
            //    .RoleIds?.ExceptBy(user.UserRoles.Select(e => e.Id), x => x)
            //    .Select(x => new UserRole { UserId = input.Id, RoleId = x });
            //if (insertUserRole is not null)
            //{
            //    _dbContext.UserRoles.AddRange(insertUserRole);
            //}

            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> Delete(int id)
        {
            var userId = _httpContext.GetCurrentUserId();
            _logger.LogInformation(
                "{MethodName}: id = {Id}, userId = {UserId}",
                nameof(Delete),
                id,
                userId
            );

            var user = await _dbContext
                .Users.Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == id && !u.Deleted);
            if (user == null)
            {
                return Result.Failure(ErrorCode.UserNotFound, this.GetCurrentMethodInfo());
            }
            user.Deleted = true;
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> ChangeStatus(int id)
        {
            var userId = _httpContext.GetCurrentUserId();
            _logger.LogInformation("{MethodName}: id = {Id}", nameof(ChangeStatus), id);
            var user = await _dbContext
                .Users.Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == id && !u.Deleted);
            if (user == null)
            {
                return Result.Failure(ErrorCode.UserNotFound, this.GetCurrentMethodInfo());
            }
            if (user.Status == UserStatus.ACTIVE)
            {
                user.Status = UserStatus.DEACTIVE;
            }
            else
            {
                user.Status = UserStatus.ACTIVE;
            }
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> SetPassword(SetPasswordUserDto input)
        {
            _logger.LogInformation("{MethodName}: input = {@Input}", nameof(SetPassword), input);

            var user = await _dbContext
                .Users.Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == input.Id && !u.Deleted);
            if (user == null)
            {
                return Result.Failure(ErrorCode.UserNotFound, this.GetCurrentMethodInfo());
            }
            user.Password = PasswordHasher.HashPassword(input.Password);
            user.IsPasswordTemp = input.IsPasswordTemp;
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> ChangePassword(ChangePasswordDto input)
        {
            var userId = _httpContext.GetCurrentUserId();
            _logger.LogInformation("{MethodName}: input = {@Input}", nameof(ChangePassword), input);
            var user = await _dbContext.Users.FirstOrDefaultAsync(e =>
                e.Id == userId && !e.Deleted
            );
            if (user == null)
            {
                return Result.Failure(ErrorCode.UserNotFound, this.GetCurrentMethodInfo());
            }
            if (
                !user.IsPasswordTemp
                && !PasswordHasher.VerifyPassword(input.OldPassword!, user.Password)
            )
            {
                throw new UserFriendlyException(ErrorCode.UserOldPasswordIncorrect);
            }

            user.Password = PasswordHasher.HashPassword(input.NewPassword!);
            user.IsPasswordTemp = false;
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> UpdateUserFullName(UpdateFullNameUserDto input)
        {
            var userId = _httpContext.GetCurrentUserId();
            _logger.LogInformation(
                "{MethodName}: input = {@Input}",
                nameof(UpdateUserFullName),
                @input
            );

            var user = await _dbContext.Users.FirstOrDefaultAsync(e =>
                e.Id == userId && !e.Deleted
            );
            if (user == null)
            {
                return Result.Failure(ErrorCode.UserNotFound, this.GetCurrentMethodInfo());
            }
            user.FullName = input.FullName;
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> LoginInfor(int userId)
        {
            _logger.LogInformation("{MethodName}: userId = {UserId}", nameof(LoginInfor), userId);

            var user = await _dbContext.Users.FirstOrDefaultAsync(e =>
                e.Id == userId && !e.Deleted
            );
            if (user == null)
            {
                return Result.Failure(ErrorCode.UserNotFound, this.GetCurrentMethodInfo());
            }
            user.LastLogin = DateTimeUtils.GetDate();
            var contextInfo = _httpContext.HttpContext;

            // Lấy thông tin trình duyệt
            if (contextInfo != null)
            {
                var header = contextInfo.Request.Headers;
                string? operatingSystem = "";
                string? browser = "";
                var platFormCheck = header.ContainsKey("Sec-Ch-Ua-Platform");
                if (platFormCheck)
                {
                    var operatingSystemInfo = contextInfo
                        .Request.Headers["Sec-Ch-Ua-Platform"]
                        .ToString();
                    operatingSystem =
                        operatingSystemInfo.Length > 0
                            ? operatingSystemInfo.Replace("\"", "")
                            : null;
                }
                var browserInfoCheck = header.ContainsKey("Sec-Ch-Ua");
                var browserInfo = contextInfo.Request.Headers["Sec-Ch-Ua"].ToString();
                var browserInfoString =
                    browserInfo.Length > 0 ? browserInfo.Replace("\"", "").Split(',') : null;
                if (browserInfoCheck && browserInfoString != null)
                {
                    browser = browserInfoString.Length > 0 ? browserInfoString[0].Trim() : null;
                }
                user.OperatingSystem = operatingSystem;
                user.Browser = browser;
            }
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> UpdateAvatar(string s3Key)
        {
            var userId = _httpContext.GetCurrentUserId();
            _logger.LogInformation("{MethodName}: s3Key = {S3Key}", nameof(UpdateAvatar), s3Key);
            var user = await _dbContext.Users.FirstOrDefaultAsync(e =>
                e.Id == userId && !e.Deleted
            );
            if (user == null)
            {
                return Result.Failure(ErrorCode.UserNotFound, this.GetCurrentMethodInfo());
            }
            //var returnValueS3Key = await _s3ManagerFile.MoveAsync((s3Key, MediaTypes.Image));
            //var image = returnValueS3Key?.Images?.Find(c => c.S3KeyOld == s3Key);
            //user.AvatarImageUri = image?.Uri;
            //user.S3Key = image?.S3Key;
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<int>> GetLimitedInputTurn(string varName)
        {
            var sysVar = await _dbContext.SysVars.FirstOrDefaultAsync(o =>
                o.GrName == GrNames.AUTHMAXTURN && o.VarName == varName
            );
            if (sysVar == null)
            {
                return Result<int>.Failure(
                    ErrorCode.SysVarsIsNotConfig,
                    this.GetCurrentMethodInfo()
                );
            }

            return Result<int>.Success(int.Parse(sysVar.VarValue));
        }

        public async Task<Result> UpdateUserStatus(string userName, int userStatus)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(e =>
                e.Username == userName && !e.Deleted
            );
            if (user == null)
            {
                return Result.Failure(ErrorCode.UserNotFound, this.GetCurrentMethodInfo());
            }
            user.Status = userStatus;
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> UpdateUserStatus(int userId, int userStatus)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(e =>
                e.Id == userId && !e.Deleted
            );
            if (user == null)
            {
                return Result.Failure(ErrorCode.UserNotFound, this.GetCurrentMethodInfo());
            }
            user.Status = userStatus;
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> UpdateRole(UpdateRoleDto input)
        {
            _logger.LogInformation("{MethodName}: input = {@Input}", nameof(UpdateRole), input);

            //Danh sách role hiện tại được gán cho user
            var currentUserRole = await _dbContext
                .UserRoles.Where(e => e.UserId == input.UserId && !e.Deleted)
                .ToListAsync();

            //Lặp qua những phần tử không có trong input
            foreach (var item in currentUserRole.Where(x => !input.RoleIds.Contains(x.Id)))
            {
                //Xoá phần tử
                item.Deleted = true;
            }

            //Thêm mới những role gán với user
            var addNewUserRole = input
                .RoleIds.ExceptBy(currentUserRole.Select(x => x.Id), x => x)
                .Select(x => new UserRole { UserId = input.UserId, RoleId = x });
            _dbContext.UserRoles.AddRange(addNewUserRole);
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }
    }
}
