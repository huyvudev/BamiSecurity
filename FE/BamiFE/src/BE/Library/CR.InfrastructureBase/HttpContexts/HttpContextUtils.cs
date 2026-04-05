using CR.ConstantBase.MultiTenancy;
using CR.Constants.Core.Users;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CR.InfrastructureBase.HttpContexts
{
    public static class HttpContextUtils
    {
        /// <summary>
        /// Trả về user id nếu có trong token, ngược lại trả về null
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        public static int? GetCurrentUserIdInContext(this IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var claim = claims?.FindFirst("user_id");
            if (claim != null && int.TryParse(claim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }

        /// <summary>
        /// Trả về tenant id nếu có trong token hoặc lấy từ middleware nếu không có trả về null
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static int? GetCurrentTenantIdInContext(this IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            if (claims?.IsAuthenticated == true) //có đăng nhập
            {
                var claim = claims.FindFirst(UserClaimTypes.UserType);
                if (claim == null)
                {
                    throw new InvalidOperationException($"Claim {UserClaimTypes.UserType} not found.");
                }
                var userType = int.Parse(claim.Value);
                //là tài khoản quản trị thì trả ra null
                if (userType == UserTypes.ADMIN || userType == UserTypes.SUPER_ADMIN)
                {
                    return null;
                }
                //là tài khoản thường thì bắt buộc phải có tenantId
                var claimTenantId = claims.FindFirst(UserClaimTypes.TenantId);
                if (claimTenantId == null)
                {
                    throw new InvalidOperationException($"Claim {UserClaimTypes.TenantId} not found.");
                }
                return int.Parse(claimTenantId.Value);
            }
            else //không đăng nhập thì lấy từ middleware
            {
                object? tenantIdInItems = null;
                httpContextAccessor
                    .HttpContext?.Items
                    .TryGetValue(MultiTenancyQuery.TenantId, out tenantIdInItems);
                //trường hợp là vào trang quản trị thì tenantId sẽ là null
                return tenantIdInItems as int?;
            }
        }
    }
}
