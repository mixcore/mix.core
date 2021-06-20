using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Cms.Lib.Services;

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
        protected readonly MixIdentityService _idService;
        public AuthorizeActionFilter(MixIdentityService idService)
        {
            _idService = idService;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAuthorized = _idService.CheckEndpointPermission(
                context.HttpContext.User, context.HttpContext.Request.Path, context.HttpContext.Request.Method);

            if (!isAuthorized)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
