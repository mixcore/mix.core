using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Auth.Constants;
using Mix.Lib.Services;
using System.Security.Claims;

namespace Mix.Lib.Attributes
{
    public class MixAuthorizeOptionalAttribute : TypeFilterAttribute
    {
        public MixAuthorizeOptionalAttribute(string roles = null)
        : base(typeof(OptionalAuthorizeActionFilter))
        {
            Arguments = new object[] { roles ?? string.Empty };
        }
    }

    public class OptionalAuthorizeActionFilter : IAuthorizationFilter
    {
        public string[] AllowedRoles { get; set; }
        public string[] UserRoles { get; set; }
        protected readonly MixIdentityService _idService;
        protected readonly MixPermissionService _permissionService;
        private readonly TenantUserManager _userManager;
        private ClaimsPrincipal userPrinciple;
        public OptionalAuthorizeActionFilter(
            string roles,
            MixIdentityService idService,
            MixPermissionService permissionService,
            TenantUserManager userManager)
        {
            _idService = idService;
            _permissionService = permissionService;
            _userManager = userManager;
            AllowedRoles = roles.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()).ToArray();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            userPrinciple = context.HttpContext.User;

            return;
        }

        #region Privates

        private bool ValidEnpointPermission(AuthorizationFilterContext context)
        {
            return _permissionService.CheckEndpointPermission(UserRoles, context.HttpContext.Request.Path, context.HttpContext.Request.Method);
        }

        private bool ValidToken()
        {
            return userPrinciple.Identity.IsAuthenticated
                    && DateTime.TryParse(_idService.GetClaim(userPrinciple, MixClaims.ExpireAt), out var expireAt)
                    && DateTime.UtcNow < expireAt;
        }

        private bool IsInRoles()
        {
            if (AllowedRoles.Count() == 0)
            {
                return true;
            }

            UserRoles = _idService.GetClaim(userPrinciple, MixClaims.Role).Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(r => r.Trim()).ToArray();
            if (UserRoles.Any(r => r == MixRoles.SuperAdmin || r == $"{MixRoles.Owner}-{_idService.CurrentTenant.Id}"))
            {
                return true;
            }
            return AllowedRoles.Any(r => UserRoles.Any(ur => ur == $"{r}-{_idService.CurrentTenant.Id}"));
        }

        #endregion
    }
}
