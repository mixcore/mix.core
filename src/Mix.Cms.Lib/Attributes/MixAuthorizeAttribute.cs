using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Identity.Constants;
using Mix.Identity.Helpers;
using System;

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
        protected AuditLogRepository _auditlogRepo;
        protected MixIdentityHelper _mixIdentityHelper;
        
        public AuthorizeActionFilter(
            MixIdentityService idService,
            AuditLogRepository auditlogRepo,
            MixIdentityHelper mixIdentityHelper)
        {
            _idService = idService;
            _auditlogRepo = auditlogRepo;
            _mixIdentityHelper = mixIdentityHelper;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAuthorized = _idService.CheckEndpointPermission(
                context.HttpContext.User, context.HttpContext.Request.Path, context.HttpContext.Request.Method);

            if (!isAuthorized)
            {
                context.Result = new UnauthorizedResult();
            }
            
            context.HttpContext.Request.EnableBuffering();
            context.HttpContext.Request.Headers.Add("RequestId", Guid.NewGuid().ToString());
            _auditlogRepo.Log(
                _mixIdentityHelper.GetClaim(context.HttpContext.User, MixClaims.Username),
                context.HttpContext.Request, false, null);
            context.HttpContext.Request.Body.Seek(0, System.IO.SeekOrigin.Begin);
        }
    }
}
