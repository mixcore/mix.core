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
            
            _logger.LogInformation("Authorization check for user: {User}", userPrinciple?.Identity?.Name ?? "Anonymous");

            if (ValidToken())
            {
                _logger.LogInformation("Token is valid");
                
                UserRoles = _idService.GetClaim(userPrinciple, MixClaims.Role)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => r.Trim()).ToArray();
                    
                _logger.LogInformation("User roles: {Roles}", string.Join(", ", UserRoles));
                _logger.LogInformation("Required roles: {RequiredRoles}", string.Join(", ", AllowedRoles));

                if (!IsInRoles())
                {
                    _logger.LogWarning("User not in required roles");
                    
                    if (!ValidEndpointPermission(context))
                    {
                        _logger.LogWarning("Endpoint permission check failed");
                        context.Result = new ForbidResult();
                        return;
                    }
                    _logger.LogInformation("Endpoint permission check passed");
                }
                else
                {
                    _logger.LogInformation("User is in required roles");
                }
            }
            else
            {
                _logger.LogWarning("Invalid token");
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        #region Privates

        private bool ValidEndpointPermission(AuthorizationFilterContext context)
        {
            return _permissionService.CheckEndpointPermissionAsync(UserRoles, context.HttpContext.Request.Path, context.HttpContext.Request.Method).Result;
        }

        private bool ValidToken()
        {
            if (!userPrinciple.Identity.IsAuthenticated)
            {
                _logger.LogWarning("User is not authenticated");
                return false;
            }

            var expireAtClaim = _idService.GetClaim(userPrinciple, MixClaims.ExpireAt);
            if (string.IsNullOrEmpty(expireAtClaim))
            {
                _logger.LogWarning("ExpireAt claim is missing");
                return false;
            }

            // Use DateTimeOffset for better timezone handling
            if (!DateTimeOffset.TryParse(expireAtClaim, out var expireAt))
            {
                _logger.LogWarning("ExpireAt claim is not a valid date: {ExpireAt}", expireAtClaim);
                return false;
            }

            var isValid = DateTimeOffset.UtcNow < expireAt;
            _logger.LogInformation("Token expiration check: Current={Current}, Expires={Expires}, IsValid={IsValid}", 
                DateTimeOffset.UtcNow, expireAt, isValid);
            
            return isValid;
        }

        private bool IsInRoles()
        {
            if (AllowedRoles.Count() == 0)
            {
                return true;
            }

            // UserRoles is already set in OnAuthorization
            if (UserRoles.Any(r => r == MixRoles.SuperAdmin || r == $"{MixRoles.Owner}-{_idService.CurrentTenant.Id}"))
            {
                return true;
            }
            return AllowedRoles.Any(r => UserRoles.Any(ur => ur == $"{r}-{_idService.CurrentTenant.Id}"));
        }

        #endregion
    }
}
