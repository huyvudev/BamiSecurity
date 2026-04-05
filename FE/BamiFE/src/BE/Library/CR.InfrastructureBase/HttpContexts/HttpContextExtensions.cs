using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using CR.ApplicationBase.Localization;
using CR.ConstantBase.Common;
using CR.ConstantBase.MultiTenancy;
using CR.Constants.Core.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CR.InfrastructureBase
{
    public static class HttpContextExtensions
    {
        private static Claim FindClaim(
            this IHttpContextAccessor httpContextAccessor,
            string claimType
        )
        {
            var claims = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var claim =
                claims?.FindFirst(claimType)
                ?? throw new InvalidOperationException($"Claim \"{claimType}\" not found.");
            return claim;
        }

        public static int GetCurrentUserType(this IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var claim = claims?.FindFirst(UserClaimTypes.UserType);
            return claim == null
                ? throw new InvalidOperationException($"Claim {UserClaimTypes.UserType} not found.")
                : int.Parse(claim!.Value!);
        }

        public static int GetCurrentUserId(this IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var claim =
                (claims?.FindFirst(UserClaimTypes.UserId))
                ?? throw new InvalidOperationException($"Claim {UserClaimTypes.UserId} not found.");
            int userId = int.Parse(claim.Value);
            return userId;
        }

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

        public static int? GetCurrentTenantId(this IHttpContextAccessor httpContextAccessor)
        {
            var logger = httpContextAccessor.GetService<ILogger<HttpContextAccessor>>();
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
                var claimTenantId = claims.FindFirst(UserClaimTypes.TenantId);
                if (claimTenantId == null)
                {
                    throw new InvalidOperationException(
                        $"Claim {UserClaimTypes.TenantId} not found."
                    );
                }

                logger.LogInformation(
                    "{ClassName}->{MethodName}: {Message}, tenantId = {TenantId}",
                    typeof(HttpContextExtensions).FullName,
                    nameof(GetCurrentTenantId),
                    "claims.IsAuthenticated == true",
                    claimTenantId.Value
                );
                return int.Parse(claimTenantId.Value);
            }
            else //không đăng nhập thì lấy từ middleware xử lý theo trường hợp tìm tenant bằng domain
            {
                object? tenantIdInItems = null;
                httpContextAccessor
                    .HttpContext?.Items
                    .TryGetValue(MultiTenancyQuery.TenantId, out tenantIdInItems);
                //trường hợp là vào trang quản trị thì tenantId sẽ là null
                logger.LogInformation(
                    "{ClassName}->{MethodName}: {Message}, tenantId = {TenantId}",
                    typeof(HttpContextExtensions).FullName,
                    nameof(GetCurrentTenantId),
                    "claims.IsAuthenticated == false => Get tenant id from middleware",
                    tenantIdInItems
                );
                return tenantIdInItems as int?;
            }
        }

        public static string GetCurrentAuthorizationId(this IHttpContextAccessor httpContextAccessor)
        {
            var claim = httpContextAccessor.FindClaim(UserClaimTypes.AuthorizationId);
            return claim.Value;
        }

        public static string? GetCurrentName(this IHttpContextAccessor httpContextAccessor)
        {
            var claim = httpContextAccessor.FindClaim(UserClaimTypes.Name);
            return claim.Value;
        }

        public static string GetCurrentSubject(this IHttpContextAccessor httpContextAccessor)
        {
            var claim = httpContextAccessor.FindClaim(ClaimTypes.NameIdentifier);
            return claim.Value;
        }

        public static string GetCurrentLanguage(this IHttpContextAccessor httpContextAccessor)
        {
            object? language = null;
            httpContextAccessor
                .HttpContext?.Items
                .TryGetValue(MultiTenancyQuery.Language, out language);
            return language?.ToString() ?? LocalizationNames.English;
        }

        public static string RandomNumber(int length = 6)
        {
            Random random = new();
            const string chars = "0123456789";

            return new string(
                Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray()
            );
        }

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

        public static string? GetCurrentRequestId(this IHttpContextAccessor httpContextAccessor)
        {
            string? requestId = null;
            if (
                httpContextAccessor
                    ?.HttpContext
                    ?.Request
                    .Headers.TryGetValue(HeaderNames.XRequestId, out var requestIds) == true
            )
            {
                requestId = requestIds.FirstOrDefault();
            }
            return requestId;
        }

        public static string GetRequestHostDomainWithSchema(this IHttpContextAccessor httpContextAccessor)
        {
            var request = httpContextAccessor.HttpContext?.Request;
            string? host = request?.Host.Host;
            int? port = request?.Host.Port;

            //chạy theo domain phân biệt theo cả port
            if (port.HasValue
                && !new int?[] { 80, 443, 8081, 8080 }.Contains(port)
            )
            {
                host += $":{port}";
            }
            return $"{request?.Scheme ?? "http"}://" + host;
        }

        public static string GetRequestHostDomain(this IHttpContextAccessor httpContextAccessor)
        {
            var request = httpContextAccessor.HttpContext?.Request;
            string? host = request?.Host.Host;
            int? port = request?.Host.Port;

            //chạy theo domain phân biệt theo cả port
            if (port.HasValue
                && !new int?[] { 80, 443, 8081, 8080 }.Contains(port)
            )
            {
                host += $":{port}";
            }
            return host ?? string.Empty;
        }

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
