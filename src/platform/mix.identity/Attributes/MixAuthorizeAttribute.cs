using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Identity.Services;

namespace Mix.Identity.Attributes
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
        
        public AuthorizeActionFilter(
            MixIdentityService idService)
        {
            _idService = idService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAuthorized = context.HttpContext.User.Identity.IsAuthenticated && _idService.CheckEndpointPermission(
                context.HttpContext.User, context.HttpContext.Request.Path, context.HttpContext.Request.Method);

            if (!isAuthorized)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
