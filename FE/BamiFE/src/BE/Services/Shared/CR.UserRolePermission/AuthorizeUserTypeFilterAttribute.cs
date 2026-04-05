using CR.ApplicationBase.Localization;
using CR.Constants.Core.Users;
using CR.InfrastructureBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CR.UserRolePermission
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeUserTypeFilterAttribute : Attribute, IAuthorizationFilter
    {
        private readonly int[] _userTypes;
        private LocalizationBase _localization = null!;
        private IHttpContextAccessor _httpContext = null!;

        public AuthorizeUserTypeFilterAttribute(params int[] userTypes)
        {
            _userTypes = userTypes ?? [];
        }

        private void GetServices(AuthorizationFilterContext context)
        {
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
            var userType = _httpContext.GetCurrentUserType();
            if (userType == UserTypes.SUPER_ADMIN)
            {
                return;
            }

            if (!_userTypes.Contains(userType))
            {
                context.Result = new UnauthorizedObjectResult(
                    new { message = _localization.Localize("error_UserNotHavePermission"), userNotHavePermission = true }
                );
            }
        }
    }

    /// <summary>
    /// UserAdmin: Filter những user là quản trị
    /// </summary>
    public class AuthorizeAdminUserTypeFilterAttribute : AuthorizeUserTypeFilterAttribute
    {
        public AuthorizeAdminUserTypeFilterAttribute()
            : base(UserTypes.SUPER_ADMIN, UserTypes.ADMIN) { }
    }

    /// <summary>
    /// UserCustomer: Filter những user là quản trị của Tenant (TenantAdmin)
    /// </summary>
    public class AuthorizeTenantAdminUserTypeFilterAttribute : AuthorizeUserTypeFilterAttribute
    {
        public AuthorizeTenantAdminUserTypeFilterAttribute()
            : base(UserTypes.TENANT_ADMIN) { }
    }

    /// <summary>
    /// UserCustomer: Filter những user là customer của Tenant (customer)
    /// </summary>
    public class AuthorizeCustomerUserTypeFilterAttribute : AuthorizeUserTypeFilterAttribute
    {
        public AuthorizeCustomerUserTypeFilterAttribute()
            : base(UserTypes.CUSTOMER) { }
    }
}
