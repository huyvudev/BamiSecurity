using CR.Common;
using CR.EntitiesBase.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CR.WebAPIBase.Middlewares
{
    public class MultiTenancyMiddleware<T> where T : IMultiTenancyOption
    {
        private readonly RequestDelegate _next;

        public MultiTenancyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IServiceProvider serviceProvider,
            IHttpContextAccessor httpContext
        )
        {
            int? tenantId = httpContext.GetCurrentTenantId();
            if (tenantId != null)
            {
                // Lấy tất cả các DbContext triển khai IMultiTenancyOption
                var dbContext = context.RequestServices.GetRequiredService<T>();
                dbContext.TenantId = tenantId;
            }
            await _next(context);
        }
    }

    public static class TenantMiddlewareExtensions
    {
        public static IApplicationBuilder UseMultiTenancy<T>(this IApplicationBuilder builder) where T : IMultiTenancyOption
        {
            return builder.UseMiddleware<MultiTenancyMiddleware<T>>();
        }
    }
}
