using Authorization.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.Attributes
{
    public class PermissionsAuthorize : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _permissions;

        public PermissionsAuthorize(string permission)
        {
            _permissions = new string[] { permission };
        }

        public PermissionsAuthorize(params string[] permissions)
        {
            _permissions = permissions;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context != null)
            {

            }

            var serviceProvider = context.HttpContext.RequestServices;
            var _authorizationService = serviceProvider.GetService<IAuthorizationService>();

            var userRole = context.HttpContext.User.FindFirst("Role")?.Value;

            if (userRole == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            // Check if the user has the required permsissions
            if ((await _authorizationService.HasPermission(userRole, _permissions)) == false)
            {
                context.Result = new ForbidResult();
                return;
            }
        }

    }

}
