using CR.Constants.Authorization;
using CR.Constants.Core.Users;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CR.WebAPIBase.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var cookie = context
                .GetHttpContext()
                .AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme)
                .Result;
            if (
                cookie.Succeeded
                && cookie.Principal.Claims.Any(e =>
                    e.Type == UserClaimTypes.UserType && e.Value == UserTypes.SUPER_ADMIN.ToString()
                )
            )
            {
                return true;
            }
            context.GetHttpContext().Response.Redirect(AuthenticationPath.AuthenticateLogin);
            return true;
        }
    }
}
