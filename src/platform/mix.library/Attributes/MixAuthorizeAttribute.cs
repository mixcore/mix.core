using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Identity.Constants;
using Mix.Lib.Services;

namespace Mix.Lib.Attributes
{
    public class MixAuthorizeAttribute : TypeFilterAttribute
    {
        public MixAuthorizeAttribute(string roles)
        : base(typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { roles };
        }
    }

    public class AuthorizeActionFilter : IAuthorizationFilter
    {
        public string[] Roles { get; set; }
        protected readonly MixIdentityService _idService;
        private readonly TenantUserManager _userManager;

        public AuthorizeActionFilter(
            string roles,
            MixIdentityService idService,
            TenantUserManager userManager)
        {
            Roles = roles.Replace(" ", string.Empty).Split(',');
            _idService = idService;
            _userManager = userManager;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userRoles = _idService.GetClaim(context.HttpContext.User, MixClaims.Role).Split(',');
            bool isInRole = false;
            foreach (var role in userRoles)
            {
                if (Roles.Contains(role))
                {
                    isInRole = true;
                    break;
                }
            }
            bool isAuthorized = context.HttpContext.User.Identity.IsAuthenticated
                && isInRole
                && _idService.CheckEndpointPermission(
                    context.HttpContext.User, context.HttpContext.Request.Path, context.HttpContext.Request.Method);

            if (!isAuthorized)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
