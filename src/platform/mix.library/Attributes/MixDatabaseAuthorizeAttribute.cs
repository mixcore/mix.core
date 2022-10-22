using Google.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Identity.Constants;
using Mix.Lib.Services;
using System.Security.Claims;

namespace Mix.Lib.Attributes
{
    public class MixDatabaseAuthorizeAttribute : TypeFilterAttribute
    {
        public MixDatabaseAuthorizeAttribute()
        : base(typeof(DatabaseAuthorizeActionFilter))
        {
        }
    }

    public class DatabaseAuthorizeActionFilter : IAuthorizationFilter
    {
        private readonly MixCmsContext _cmsContext;
        public string[] AllowedReadRoles { get; set; }
        public string[] AllowedWriteRoles { get; set; }
        protected readonly MixIdentityService _idService;
        private ClaimsPrincipal userPrinciple;
        public DatabaseAuthorizeActionFilter(
            MixIdentityService idService,
            MixCmsContext cmsContext)
        {
            _cmsContext = cmsContext;
            _idService = idService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            userPrinciple = context.HttpContext.User;
            var _tableName = context.HttpContext.Request.RouteValues["name"].ToString();
            var database = _cmsContext.MixDatabase.FirstOrDefault(m => m.SystemName == _tableName);
            if (database == null)
            {
                context.Result = new BadRequestResult();
                return;
            }
            AllowedReadRoles = JArray.Parse(database.ReadPermissions).Values<string>("text").ToArray();
            AllowedWriteRoles = JArray.Parse(database.WritePermissions).Values<string>("text").ToArray();
            if (ValidToken())
            {
                if (!IsInRoles(context.HttpContext.Request.Method))
                {
                    if (!ValidEnpointPermission(context))
                    {
                        context.Result = new ForbidResult();
                        return;
                    }
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
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

        private bool IsInRoles(string method)
        {
            if (method == "GET" && AllowedReadRoles.Count() == 0)
            {
                return true;
            }
            if (method != "GET" && AllowedWriteRoles.Count() == 0)
            {
                return true;
            }

            var userRoles = _idService.GetClaim(userPrinciple, MixClaims.Role).Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(r => r.Trim()).ToArray();
            if (userRoles.Any(r => r == MixRoles.SuperAdmin))
            {
                return true;
            }
            if (method == "GET")
            {
                return AllowedReadRoles.Any(r => userRoles.Any(ur => ur == $"{r}-{_idService.CurrentTenant.Id}"));
            }
            return AllowedWriteRoles.Any(r => userRoles.Any(ur => ur == $"{r}-{_idService.CurrentTenant.Id}"));
        }

        #endregion
    }
}
