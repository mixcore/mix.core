using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Identity.Constants;
using Mix.Lib.Services;
using System.Security.Claims;

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
        private ClaimsPrincipal userPrinciple;
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
            userPrinciple = context.HttpContext.User;

            if (!ValidToken())
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!IsInRoles())
            {
                context.Result = new ForbidResult();
                return;
            }

            if (!ValidEnpointPermission(context))
            {
                context.Result = new ForbidResult();
                return;
            }
        }

        #region Privates

        private bool ValidEnpointPermission(AuthorizationFilterContext context)
        {
            return _idService.CheckEndpointPermission(
                    context.HttpContext.User, context.HttpContext.Request.Path, context.HttpContext.Request.Method);
        }

        private bool ValidToken()
        {
            return userPrinciple.Identity.IsAuthenticated 
                    && DateTime.TryParse(_idService.GetClaim(userPrinciple, MixClaims.ExpireAt), out var expireAt) 
                    && DateTime.UtcNow < expireAt;
        }

        private bool IsInRoles()
        {
            if (Roles.Count() == 0)
            {
                return true;
            }

            var userRoles = _idService.GetClaim(userPrinciple, MixClaims.Role).Split(',');
            foreach (var role in userRoles)
            {
                if (Roles.Contains(role))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
