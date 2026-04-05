using CR.Constants.Common.SysVar;
using CR.Constants.Core.Users;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.AuthenticationModule.Abstracts;
using CR.Core.ApplicationServices.Common;
using CR.Core.Domain.Users;
using CR.InfrastructureBase;
using CR.InfrastructureBase.Exceptions;
using CR.Utils.DataUtils;
using CR.Utils.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.AuthenticationModule.Implements
{
    public class UserAuthorizationService : CoreServiceBase, IUserAuthenticationService
    {
        public UserAuthorizationService(
            ILogger<UserAuthorizationService> logger,
            IHttpContextAccessor httpContext
        )
            : base(logger, httpContext) { }

        public async Task<User> ValidateAdminUser(string username, string password)
        {
            _logger.LogInformation(
                "{MethodName}: username = {Username}",
                nameof(ValidateAdminUser),
                username
            );
            var user =
                await _dbContext.Users.FirstOrDefaultAsync(u =>
                    u.Username == username && !u.Deleted && u.TenantId == null
                ) ?? throw new UserFriendlyException(ErrorCode.UsernameOrPasswordIncorrect);
            if (UserStatus.USER_NOT_VALID.Contains(user.Status))
            {
                throw new UserFriendlyException(ErrorCode.UserNotFound);
            }
            else if (
                !new UserTypeEnum[] { UserTypeEnum.ADMIN, UserTypeEnum.SUPER_ADMIN }.Contains(
                    user.UserType
                )
            )
            {
                throw new UserFriendlyException(ErrorCode.UserLoginUserTypeInvalid);
            }
            else if (!PasswordHasher.VerifyPassword(password, user.Password))
            {
                HandleCountIncorrectPassword(user);
            }
            else if (user.Status == UserStatus.DEACTIVE)
            {
                throw new UserFriendlyException(ErrorCode.UserIsDeactive);
            }
            return user;
        }

        public async Task<User> ValidateAppUser(string username, string password)
        {
            var tenantId = _httpContext.GetCurrentTenantId();
            _logger.LogInformation(
                "{MethodName}: username = {Username}",
                nameof(ValidateAppUser),
                username
            );
            var now = DateTimeUtils.GetDate();
            var user =
                await _dbContext.Users.FirstOrDefaultAsync(u =>
                    u.Username == username
                    && !u.Deleted
                    && u.TenantId == tenantId
                    && u.Status == UserStatus.ACTIVE
                ) ?? throw new UserFriendlyException(ErrorCode.UserNotFound);
            if (
                !new UserTypeEnum[] { UserTypeEnum.CUSTOMER, UserTypeEnum.SUPER_ADMIN, UserTypeEnum.ADMIN }.Contains(
                    user.UserType
                )
            )
            {
                throw new UserFriendlyException(ErrorCode.UserLoginUserTypeInvalid);
            }
            else if (user.Status == UserStatus.TEMP)
            {
                throw new UserFriendlyException(ErrorCode.UserNotFound);
            }
            if (user.TimeLockUser >= now)
            {
                throw new UserFriendlyException(ErrorCode.UserIsInactiveBecauseMultiLoginTime);
            }
            else if (!PasswordHasher.VerifyPassword(password, user.Password))
            {
                HandleCountIncorrectPassword(user);
            }
            else if (user.Status == UserStatus.DEACTIVE)
            {
                throw new UserFriendlyException(ErrorCode.UserIsDeactive);
            }
            else if (user.Status == UserStatus.LOCK)
            {
                throw new UserFriendlyException(ErrorCode.UserIsLock);
            }
            user.LoginFailCount = 0;
            user.DateTimeLoginFailCount = new DateTime(0, DateTimeKind.Local);
            // Nếu là lần đầu đăng nhập đối với Tài khoản được tạo trên CMS
            if (user.IsFirstTime)
            {
                user.IsFirstTime = false;
                await _dbContext.SaveChangesAsync();
                // Trả về param Token
                user.IsFirstTime = true;
            }
            return user;
        }

        /// <summary>
        /// Tính số lần đăng nhập thất bại
        /// </summary>
        /// <param name="user"></param>
        /// <exception cref="UserFriendlyException"></exception>
        private void HandleCountIncorrectPassword(User user)
        {
            var now = DateTimeUtils.GetDate();
            if (user.DateTimeLoginFailCount < DateTimeUtils.GetDate() && user.LoginFailCount != 0)
            {
                user.LoginFailCount = 0;
                user.DateTimeLoginFailCount = new DateTime(0, DateTimeKind.Unspecified);
            }
            user.LoginFailCount++;
            var loginMaxTurn = GetLimitedInputTurn(VarNames.LOGINMAXTURN);
            user.DateTimeLoginFailCount = DateTimeUtils.GetDate().AddMinutes(15);
            if (user.LoginFailCount >= loginMaxTurn)
            {
                user.TimeLockUser = now.AddHours(1);
                user.LoginFailCount = 0;
                user.DateTimeLoginFailCount = new DateTime(0, DateTimeKind.Unspecified);
            }
            _dbContext.SaveChanges();
            if (user.TimeLockUser >= now)
            {
                throw new UserFriendlyException(ErrorCode.UserIsInactiveBecauseMultiLoginTime);
            }

            if (user.Status == UserStatus.DEACTIVE)
            {
                throw new UserFriendlyException(ErrorCode.UserIsDeactive);
            }

            if (user.LoginFailCount == 1)
            {
                throw new UserFriendlyException(ErrorCode.UsernameOrPasswordIncorrect);
            }
            throw new UserFriendlyException(
                ErrorCode.AppPasswordIncorrect,
                loginMaxTurn.ToString(),
                (loginMaxTurn - user.LoginFailCount).ToString()
            );
        }

        public int GetLimitedInputTurn(string varName)
        {
            var sysVar =
                _dbContext.SysVars.FirstOrDefault(o =>
                    o.GrName == GrNames.AUTHMAXTURN && o.VarName == varName
                )
                ?? throw new UserFriendlyException(
                    ErrorCode.SysVarsIsNotConfig,
                    GrNames.AUTHMAXTURN,
                    varName
                );
            return int.Parse(sysVar.VarValue);
        }

        public async Task<User> FindUserAuthorizationById(int id)
        {
            _logger.LogInformation($"{nameof(FindUserAuthorizationById)}: id = {id}");
            var user =
                await _dbContext.Users.FirstOrDefaultAsync(u =>
                    u.Id == id && u.Status == UserStatus.ACTIVE && !u.Deleted
                ) ?? throw new UserFriendlyException(ErrorCode.UserNotFound);
            if (new int[] { UserStatus.TEMP, UserStatus.LOCK }.Contains(user.Status))
            {
                throw new UserFriendlyException(ErrorCode.UserNotFound);
            }
            else if (user.Status == UserStatus.DEACTIVE)
            {
                throw new UserFriendlyException(ErrorCode.UserIsDeactive);
            }
            return user;
        }
    }
}
