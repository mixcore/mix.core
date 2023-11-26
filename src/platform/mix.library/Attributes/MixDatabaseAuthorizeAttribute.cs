using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Auth.Constants;
using Mix.Lib.Services;
using System.Security.Claims;

namespace Mix.Lib.Attributes
{
    public class MixDatabaseAuthorizeAttribute : TypeFilterAttribute
    {
        public MixDatabaseAuthorizeAttribute(string tableName)
        : base(typeof(DatabaseAuthorizeActionFilter))
        {
            Arguments = new object[] { tableName };
        }
    }

    public class DatabaseAuthorizeActionFilter : IAuthorizationFilter
    {
        private string _tableName;
        public string[] UserRoles { get; set; }
        private readonly MixCmsContext _cmsContext;
        protected readonly MixIdentityService _idService;
        protected readonly MixPermissionService _permissionService;
        private ClaimsPrincipal userPrinciple;
        public DatabaseAuthorizeActionFilter(
            string tableName,
            MixIdentityService idService,
            MixCmsContext cmsContext,
            MixPermissionService permissionService)
        {
            _tableName = tableName;
            _cmsContext = cmsContext;
            _idService = idService;
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

            if (!CheckByPassAuthenticate(context.HttpContext.Request.Method, context.HttpContext.Request.Path, database))
            {
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
        }

        private bool CheckByPassAuthenticate(string method, string path, MixDatabase database)
        {
            return
                (method == "GET" && (string.IsNullOrEmpty(database.ReadPermissions) || JArray.Parse(database.ReadPermissions).Count == 0))
                || (method == "POST" && path.EndsWith("filter") && (string.IsNullOrEmpty(database.ReadPermissions) || JArray.Parse(database.ReadPermissions).Count == 0))
                || (method == "POST" && (string.IsNullOrEmpty(database.CreatePermissions) || JArray.Parse(database.CreatePermissions).Count == 0))
                || ((method == "PUT" || method == "PATCH") && (string.IsNullOrEmpty(database.UpdatePermissions) || JArray.Parse(database.UpdatePermissions).Count == 0))
                || (method == "DELETE" && (string.IsNullOrEmpty(database.DeletePermissions) || JArray.Parse(database.DeletePermissions).Count == 0));
        }

        #region Privates

        private bool ValidEnpointPermission(AuthorizationFilterContext context)
        {
            return _permissionService.CheckEndpointPermission(
                    UserRoles, context.HttpContext.Request.Path, context.HttpContext.Request.Method);
        }

        private bool ValidToken()
        {
            return userPrinciple.Identity.IsAuthenticated
                    && DateTime.TryParse(_idService.GetClaim(userPrinciple, MixClaims.ExpireAt), out var expireAt)
                    && DateTime.UtcNow < expireAt;
        }

        private bool IsInRoles(string method, MixDatabase database)
        {

            UserRoles = _idService.GetClaim(userPrinciple, MixClaims.Role).Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(r => r.Trim()).ToArray();

            if (UserRoles.Any(r => r == MixRoles.SuperAdmin || r == $"{MixRoles.Owner}-{_idService.CurrentTenant.Id}"))
            {
                return true;
            }

            switch (method)
            {
                case "GET": return CheckUserInRoles(database.ReadPermissions, UserRoles);
                case "POST": return CheckUserInRoles(database.CreatePermissions, UserRoles);
                case "PATCH":
                case "PUT": return CheckUserInRoles(database.UpdatePermissions, UserRoles);
                case "DELETE": return CheckUserInRoles(database.DeletePermissions, UserRoles);
                default:
                    return false;
            }
        }

        private bool CheckUserInRoles(string roles, string[] userRoles)
        {
            var allowedRoles = JArray.Parse(roles).Values<string>().ToArray();
            return allowedRoles.Length == 0 || allowedRoles.Any(r => userRoles.Any(ur => ur == $"{r}-{_idService.CurrentTenant.Id}"));
        }

        #endregion
    }
}
