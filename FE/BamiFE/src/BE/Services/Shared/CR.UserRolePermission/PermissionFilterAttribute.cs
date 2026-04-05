using CR.ApplicationBase.Localization;
using CR.Constants.Core.Users;
using CR.Core.ApplicationServices.AuthenticationModule.Abstracts;
using CR.Utils.Net.Request;
using MB.ApplicationBase.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CR.Common.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionFilterAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _permissions;
        private IPermissionService? _permissionServices;
        private LocalizationBase? _localization;
        private IHttpContextAccessor? _httpContext;

        public PermissionFilterAttribute(params string[] permissions)
        {
            _permissions = permissions;
        }

        private void GetServices(AuthorizationFilterContext context)
        {
            _permissionServices =
                context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();
            _localization =
                context.HttpContext.RequestServices.GetRequiredService<LocalizationBase>();
            _httpContext =
                context.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata.Any(em =>
                em.GetType() == typeof(AllowAnonymousAttribute)
            );

            if (hasAllowAnonymous)
                return;

            GetServices(context);
            var userType = _httpContext!.GetCurrentUserType();
            if (userType == UserTypes.SUPER_ADMIN)
            {
                return;
            }

            bool isGrant = _permissionServices!.CheckPermission(_permissions);

            var permissionQueryParam = context
                .HttpContext.Request.Query[QueryParamKeys.Permission]
                .ToString()
                .Trim();
            if (
                !string.IsNullOrEmpty(permissionQueryParam)
                && isGrant
                && !_permissionServices!.CheckPermission(permissionQueryParam)
                && _permissions.Contains(permissionQueryParam)
            )
            {
                isGrant = false;
            }

            if (!isGrant)
            {
                context.Result = new ForbidObjectResult(
                    new { message = _localization!.Localize("error_UserNotHavePermission") }
                );
            }
        }
    }
}
