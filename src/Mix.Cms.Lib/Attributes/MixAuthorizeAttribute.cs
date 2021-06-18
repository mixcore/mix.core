using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Cms.Lib.Constants;
using Mix.Identity.Constants;
using Mix.Identity.Helpers;
using Mix.Identity.Models;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Security.Claims;

namespace Mix.Cms.Lib.Attributes
{
    public class MixAuthorizeAttribute : TypeFilterAttribute
    {
        public MixAuthorizeAttribute()
        : base(typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { };
        }
    }

    public class AuthorizeActionFilter : IAuthorizationFilter
    {
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly MixIdentityHelper _idHelper;
        public AuthorizeActionFilter(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, MixIdentityHelper idHelper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _idHelper = idHelper;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAuthorized = VerifyUserAsync(context.HttpContext.User, context.HttpContext.Request.Path, context.HttpContext.Request.Method);

            if (!isAuthorized)
            {
                context.Result = new ForbidResult();
            }
        }


        private bool VerifyUserAsync(ClaimsPrincipal user, PathString path, string method)
        {
            var roles = _idHelper.GetClaims(user, MixClaims.Role);
            if (roles.Any(r => r == MixDefaultRoles.SuperAdmin))
            {
                return true;
            }
            var getPermissions = ViewModels.Account.MixRoles.ReadViewModel.Repository.GetSingleModel(r => r.Name == roles.First());
            return getPermissions.IsSucceed
                ? getPermissions.Data.MixPermissions.Any(
                    p => p.Property<JArray>("endpoints")
                            .Any(e=> e["endpoint"].Value<string>() == path
                                    && e["method"].Value<string>() == method.ToUpper())
                    )
                : false;
        }
    }
}
