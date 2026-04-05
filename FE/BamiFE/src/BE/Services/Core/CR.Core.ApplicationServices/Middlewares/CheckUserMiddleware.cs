using CR.Constants.Core.Users;
using CR.Constants.ErrorCodes;
using CR.Core.Infrastructure.Exceptions;
using CR.Core.Infrastructure.Persistence;
using CR.Utils.Net.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace CR.Core.ApplicationServices.Middlewares
{
    public class CheckUserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICoreMapErrorCode _mapErrorCode;

        public CheckUserMiddleware(RequestDelegate next, ICoreMapErrorCode mapErrorCode)
        {
            _next = next;
            _mapErrorCode = mapErrorCode;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endPoint = context.GetEndpoint();
            bool hasAllowAnonymous =
                endPoint != null
                && (
                    endPoint!.Metadata.Any(e => e.GetType() == typeof(AllowAnonymousAttribute))
                    || !endPoint!.Metadata.Any(e => e.GetType() == typeof(AuthorizeAttribute))
                );

            if (hasAllowAnonymous)
            {
                await _next(context);
                return;
            }

            var claim = context.User.FindFirst(UserClaimTypes.UserId);
            var dbContext = context.RequestServices.GetRequiredService<CoreDbContext>();

            if (claim != null)
            {
                int userId = int.Parse(claim.Value);
                var user = await dbContext
                    .Users.Select(u => new { u.Id, u.Status })
                    .FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null && user.Status != UserStatus.ACTIVE)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsJsonAsync(
                        new ApiResponse(
                            StatusCode.Error,
                            string.Empty,
                            ErrorCode.UserIsDeactive,
                            _mapErrorCode.GetErrorMessage(ErrorCode.UserIsDeactive)
                        )
                    );
                    return;
                }
            }
            await _next(context);
        }
    }

    /// <summary>
    /// Extension check user middleware
    /// </summary>
    public static class CheckUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseCheckUser(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckUserMiddleware>();
        }
    }
}
