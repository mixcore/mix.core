using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Auth.Constants;
using Mix.Database.Entities.Cms;
using Mix.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Signalr.Hub.Attributes
{
    public sealed class MixDbHubAuthorizeAttribute : TypeFilterAttribute
    {
        public MixDbHubAuthorizeAttribute(string tableName)
        : base(typeof(DatabaseAuthorizeActionFilter))
        {
            Arguments = [tableName];
        }
        public class DatabaseAuthorizeActionFilter : IAuthorizationFilter
        {
            private string _tableName;
            public string[] UserRoles { get; set; }
            private readonly MixCmsContext _cmsContext;
            protected readonly MixPermissionService _permissionService;
            private ClaimsPrincipal userPrinciple;
            public DatabaseAuthorizeActionFilter(
                string tableName,
                MixCmsContext cmsContext,
                MixPermissionService permissionService)
            {
                _tableName = tableName;
                _cmsContext = cmsContext;
                _permissionService = permissionService;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                userPrinciple = context.HttpContext.User;
                if (string.IsNullOrEmpty(_tableName))
                {
                    _tableName = context.HttpContext.Request.RouteValues["name"]?.ToString();
                }
                var database = _cmsContext.MixDatabase.FirstOrDefault(m => m.SystemName == _tableName);
                if (database == null)
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                // Not allow bypass by default for security => if not set, only superadmin can access this database
                if (!CheckByPassAuthenticate(context.HttpContext.Request.Method, context.HttpContext.Request.Path, database))
                {
                    if (ValidToken())
                    {
                        if (!IsInRoles(context.HttpContext.Request.Method, database, context.HttpContext.Request.Path))
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
            }

            private bool CheckByPassAuthenticate(string method, string path, MixDatabase database)
            {
                return method switch
                {
                    "GET" => database.ReadPermissions == null
                        || database.ReadPermissions.Count == 0,
                    "POST" => (path.EndsWith("filter") && (database.ReadPermissions == null || database.ReadPermissions.Count == 0))
                        || (!path.EndsWith("filter") && (database.CreatePermissions == null || database.CreatePermissions.Count == 0)),
                    "PUT" => database.UpdatePermissions == null
                        || database.UpdatePermissions.Count == 0,
                    "PATCH" => database.UpdatePermissions == null
                        || database.UpdatePermissions.Count == 0,
                    "DELETE" => database.DeletePermissions == null
                        || database.DeletePermissions.Count == 0,
                    _ => false
                };
            }

            #region Privates

            private bool ValidEnpointPermission(AuthorizationFilterContext context)
            {
                return _permissionService.CheckEndpointPermissionAsync(
                        UserRoles, context.HttpContext.Request.Path, context.HttpContext.Request.Method).Result;
            }

            private bool ValidToken()
            {
                return userPrinciple.Identity.IsAuthenticated
                        && DateTime.TryParse(GetClaim(userPrinciple, MixClaims.ExpireAt), out var expireAt)
                        && DateTime.UtcNow < expireAt;
            }

            private bool IsInRoles(string method, MixDatabase database, string path)
            {

                UserRoles = GetClaim(userPrinciple, MixClaims.Role).Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => r.Trim()).ToArray();

                if (UserRoles.Any(r => r == MixRoles.SuperAdmin || r == $"{MixRoles.Owner}-1"))
                {
                    return true;
                }

                switch (method)
                {
                    case "GET": return CheckUserInRoles(database.ReadPermissions, UserRoles);
                    case "POST":
                        if (path.EndsWith("filter"))
                        {
                            return CheckUserInRoles(database.ReadPermissions, UserRoles);
                        }
                        else
                        {
                            return CheckUserInRoles(database.CreatePermissions, UserRoles);
                        }
                    case "PATCH":
                    case "PUT": return CheckUserInRoles(database.UpdatePermissions, UserRoles);
                    case "DELETE": return CheckUserInRoles(database.DeletePermissions, UserRoles);
                    default:
                        return false;
                }
            }

            private bool CheckUserInRoles(List<string> allowedRoles, string[] userRoles)
            {
                return allowedRoles == null || allowedRoles.Count == 0 || allowedRoles.Any(r => userRoles.Any(ur => ur == $"{r}-1"));
            }

            public string GetClaim(ClaimsPrincipal User, string claimType)
            {
                if (User == null)
                {
                    return null;
                }
                return string.Join(',', User.Claims.Where(c => c.Type == claimType).Select(m => m.Value));
            }
            #endregion
        }
    }
}
