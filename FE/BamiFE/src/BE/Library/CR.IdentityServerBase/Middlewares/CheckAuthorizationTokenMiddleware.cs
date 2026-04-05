using CR.ApplicationBase.Localization;
using CR.Constants.Core.Users;
using CR.Constants.ErrorCodes;
using CR.Utils.Net.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using System.Net;

namespace CR.IdentityServerBase.Middlewares
{
    public class CheckAuthorizationTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMapErrorCode _mapErrorCode;

        public CheckAuthorizationTokenMiddleware(
            RequestDelegate next,
            IMapErrorCode mapErrorCode
        )
        {
            _next = next;
            _mapErrorCode = mapErrorCode;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IOpenIddictTokenManager tokenManager,
            ILogger<CheckAuthorizationTokenMiddleware> logger
        )
        {
            bool hasAllowAnonymous =
                context.GetEndpoint() != null
                && context
                    .GetEndpoint()!
                    .Metadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
            if (hasAllowAnonymous)
            {
                await _next(context);
                return;
            }

            var authorizationId = context
                .User.Claims.FirstOrDefault(c => c.Type == UserClaimTypes.AuthorizationId)
                ?.Value;
            if (authorizationId is not null)
            {
                var tokenId = context
                    .User.Claims.FirstOrDefault(c => c.Type == UserClaimTypes.TokenId)
                    ?.Value;

                if (tokenId == null)
                {
                    await _next(context);
                    return;
                }

                var token = await tokenManager.FindByIdAsync(tokenId);
                bool isRevoke =
                    token != null
                    && await tokenManager.HasStatusAsync(
                        token,
                        OpenIddictConstants.Statuses.Revoked
                    );

                if (isRevoke)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsJsonAsync(
                        new ApiResponse(
                            StatusCode.Error,
                            string.Empty,
                            ErrorCode.Unauthorized,
                            _mapErrorCode.GetErrorMessage(ErrorCode.Unauthorized)
                        )
                    );
                    return;
                }
            }
            await _next(context);
        }
    }

    public static class CheckAuthorizationTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseCheckAuthorizationToken(
            this IApplicationBuilder builder
        )
        {
            return builder.UseMiddleware<CheckAuthorizationTokenMiddleware>();
        }
    }
}
