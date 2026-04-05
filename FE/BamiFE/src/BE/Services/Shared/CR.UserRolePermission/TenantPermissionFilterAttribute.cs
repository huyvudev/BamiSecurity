using CR.ApplicationBase.Localization;
using CR.Constants.Core.Users;
using CR.InfrastructureBase;
using CR.Utils.Net.Request;
using MB.ApplicationBase.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CR.UserRolePermission;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TenantPermissionFilterAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string[] _permissions;
    private LocalizationBase? _localization;
    private IHttpContextAccessor? _httpContext;

    public TenantPermissionFilterAttribute(params string[] permissions)
    {
        _permissions = permissions;
    }

    private void GetServices(AuthorizationFilterContext context)
    {
        _localization = context.HttpContext.RequestServices.GetRequiredService<LocalizationBase>();
        _httpContext =
            context.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata.Any(em =>
            em.GetType() == typeof(AllowAnonymousAttribute)
        );

        if (hasAllowAnonymous)
            return;

        GetServices(context);
        var userType = _httpContext!.GetCurrentUserType();
        if (userType == UserTypes.SUPER_ADMIN || userType == UserTypes.TENANT_ADMIN)
        {
            return;
        }

        var permissionQueryParam = context
            .HttpContext.Request.Query[QueryParamKeys.Permission]
            .ToString()
            .Trim();
        bool isGrant = true;
        if (!isGrant)
        {
            context.Result = new ForbidObjectResult(
                new { message = _localization!.Localize("error_UserNotHavePermission") }
            );
        }
    }
}
