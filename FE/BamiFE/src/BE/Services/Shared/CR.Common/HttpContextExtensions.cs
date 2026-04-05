using CR.ConstantBase.MultiTenancy;
using CR.Constants.Core.Users;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;

namespace CR.Common
{
    [Obsolete("Chuyển qua CR.InfrastructureBase.HttpContextExtensions")]
    public static class HttpContextExtensions
    {
        [Obsolete("Chuyển qua CR.InfrastructureBase.HttpContextExtensions")]
        public static int GetCurrentUserType(this IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var claim = claims?.FindFirst(UserClaimTypes.UserType);
            return claim == null
                ? throw new InvalidOperationException($"Claim {UserClaimTypes.UserType} not found.")
                : int.Parse(claim!.Value!);
        }

        [Obsolete("Chuyển qua CR.InfrastructureBase.HttpContextExtensions")]
        public static int GetCurrentUserId(this IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var claim =
                (claims?.FindFirst(UserClaimTypes.UserId))
                ?? throw new InvalidOperationException($"Claim {UserClaimTypes.UserId} not found.");
            int userId = int.Parse(claim.Value);
            return userId;
        }

        [Obsolete("Chuyển qua CR.InfrastructureBase.HttpContextExtensions")]
        public static int GetCurrentCustomerId(this IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var claim =
                (claims?.FindFirst(UserClaimTypes.CustomerId))
                ?? throw new InvalidOperationException(
                    $"Claim {UserClaimTypes.CustomerId} not found."
                );
            int customerId = int.Parse(claim.Value);
            return customerId;
        }

        [Obsolete("Chuyển qua CR.InfrastructureBase.HttpContextExtensions")]
        public static int? GetCurrentTenantId(this IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            if (claims?.IsAuthenticated == true) //có đăng nhập
            {
                //là tài khoản quản trị thì trả ra null
                var userType = httpContextAccessor.GetCurrentUserType();
                if (userType == UserTypes.ADMIN || userType == UserTypes.SUPER_ADMIN)
                {
                    return null;
                }

                //là tài khoản thường thì bắt buộc phải có tenantId
                var claimTenantId = claims?.FindFirst(UserClaimTypes.TenantId);
                if (claimTenantId == null)
                {
                    throw new InvalidOperationException($"Claim {UserClaimTypes.TenantId} not found.");
                }
                return int.Parse(claimTenantId.Value);
            }
            else //không đăng nhập thì lấy từ middleware
            {
                object? tenantIdInItems = null;
                var tenantFind = httpContextAccessor
                    .HttpContext?.Items
                    .TryGetValue(MultiTenancyQuery.TenantId, out tenantIdInItems);
                //trường hợp là vào trang quản trị thì tenantId sẽ là null
                return tenantIdInItems as int?;
            }
        }

        [Obsolete("Chuyển qua CR.InfrastructureBase.HttpContextExtensions")]
        public static string RandomNumber(int length = 6)
        {
            Random random = new();
            const string chars = "0123456789";

            return new string(
                Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray()
            );
        }

        [Obsolete("Chuyển qua CR.InfrastructureBase.HttpContextExtensions")]
        public static string? GetCurrentRemoteIpAddress(
            this IHttpContextAccessor httpContextAccessor
        )
        {
            string? senderIpv4 = httpContextAccessor
                ?.HttpContext
                ?.Connection
                ?.RemoteIpAddress
                ?.MapToIPv4()
                .ToString();
            if (
                httpContextAccessor
                    ?.HttpContext
                    ?.Request
                    .Headers.TryGetValue("x-forwarded-for", out var forwardedIps) == true
            )
            {
                senderIpv4 = forwardedIps.FirstOrDefault();
            }
            return senderIpv4;
        }

        [Obsolete("Chuyển qua CR.InfrastructureBase.HttpContextExtensions")]
        public static string? GetCurrentXForwardedFor(this IHttpContextAccessor httpContextAccessor)
        {
            string? forwardedIpsStr = null;
            if (
                httpContextAccessor
                    ?.HttpContext
                    ?.Request
                    .Headers.TryGetValue("x-forwarded-for", out var forwardedIps) == true
            )
            {
                forwardedIpsStr = JsonSerializer.Serialize(forwardedIps.ToList());
            }
            return forwardedIpsStr;
        }

        [Obsolete("Chuyển qua CR.InfrastructureBase.HttpContextExtensions")]
        public static TService GetService<TService>(this IHttpContextAccessor httpContextAccessor)
            where TService : class
        {
            var service =
                httpContextAccessor?.HttpContext?.RequestServices.GetService(typeof(TService))
                    as TService
                ?? throw new InvalidOperationException(
                    $"Can not resolve service type: {typeof(TService)}"
                );
            return service;
        }
    }
}
