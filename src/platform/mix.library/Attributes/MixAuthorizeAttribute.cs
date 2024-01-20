using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Mix.Auth.Constants;
using Mix.Lib.Services;
using System.Security.Claims;

namespace Mix.Lib.Attributes
{
    public class MixAuthorizeAttribute : TypeFilterAttribute
    {
        public MixAuthorizeAttribute(string roles = null)
        : base(typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { roles ?? string.Empty };
        }
    }

    public class AuthorizeActionFilter : IAuthorizationFilter
    {
        public string[] AllowedRoles { get; set; }
        public string[] UserRoles { get; set; }
        protected readonly ILogger<MixAuthorizeAttribute> _logger;
        protected readonly MixIdentityService _idService;
        protected readonly MixPermissionService _permissionService;
        private readonly TenantUserManager _userManager;
        private ClaimsPrincipal userPrinciple;
        public AuthorizeActionFilter(
            string roles,
            MixIdentityService idService,
            MixPermissionService permissionService,
            TenantUserManager userManager,
            ILogger<MixAuthorizeAttribute> logger)
        {
            _idService = idService;
            _permissionService = permissionService;
            _userManager = userManager;
            _logger = logger;
            AllowedRoles = roles.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()).ToArray();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            userPrinciple = context.HttpContext.User;

            if (ValidToken())
            {
                if (!IsInRoles())
                {
                    _logger.LogError("Not in role");
                    if (!ValidEnpointPermission(context))
                    {
                        _logger.LogError("forbidden");
                        context.Result = new ForbidResult();
                        return;
                    }
                }
            }
            else
            {
                _logger.LogError("Invalid Token");
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        #region Privates

        private bool ValidEnpointPermission(AuthorizationFilterContext context)
        {
            return _permissionService.CheckEndpointPermissionAsync(UserRoles, context.HttpContext.Request.Path, context.HttpContext.Request.Method).Result;
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
