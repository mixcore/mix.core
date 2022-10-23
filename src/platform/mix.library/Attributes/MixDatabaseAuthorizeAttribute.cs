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
            
            if (ValidToken())
            {
                if (!IsInRoles(context.HttpContext.Request.Method, database))
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

        private bool IsInRoles(string method, MixDatabase database)
        {
            var userRoles = _idService.GetClaim(userPrinciple, MixClaims.Role).Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(r => r.Trim()).ToArray();
            switch (method)
            {
                case "GET": return CheckUserInRoles(database.ReadPermissions, userRoles);
                case "POST": return CheckUserInRoles(database.CreatePermissions, userRoles);
                case "PATCH": 
                case "PUT": return CheckUserInRoles(database.ModifiedBy, userRoles);
                case "DELETE": return CheckUserInRoles(database.DeletePermissions, userRoles);
                default:
                    return false;
            }
        }

        private bool CheckUserInRoles(string roles, string[] userRoles)
        {
            var allowedRoles = JArray.Parse(roles).Values<string>().ToArray();
            return allowedRoles.Count() == 0 
                || allowedRoles.Any(r => userRoles.Any(ur => ur == $"{r}-{_idService.CurrentTenant.Id}"));
        }

        #endregion
    }
}
