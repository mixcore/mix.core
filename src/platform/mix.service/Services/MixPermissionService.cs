using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.Cms;
using Mix.Database.Entities.MixDb;
using Mix.Database.Services;
using Mix.Heart.Exceptions;
using Mix.Heart.UnitOfWork;
using Mix.Shared.Extensions;
using Mix.Shared.Services;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;

namespace Mix.Service.Services
{
    public sealed class MixPermissionService
    {
        private readonly DatabaseService _databaseService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Dictionary<string, string[]> RoleEndpoints { get; private set; }
        private MixCmsAccountContext accountDbContext;
        private UnitOfWorkInfo<MixDbDbContext> uow;
        private int? _tenantId;
        public MixPermissionService(
                IHttpContextAccessor httpContextAccessor)
        { 
            _httpContextAccessor = httpContextAccessor;
            _databaseService = new(httpContextAccessor);
            _tenantId = _httpContextAccessor.HttpContext?.Session.GetInt32(MixRequestQueryKeywords.TenantId) ?? 1;
        }

        public async Task Reload()
        {
            if (!GlobalConfigService.Instance.IsInit)
            {
                try
                {
                    uow = new(new MixDbDbContext(_databaseService));
                    RoleEndpoints = new Dictionary<string, string[]>();
                    accountDbContext = new MixCmsAccountContext(_databaseService);
                    using var cmsDbContext = new MixCmsContext(_databaseService);

                    var roles = await accountDbContext.MixRoles.ToListAsync();

                    foreach (var role in roles)
                    {
                        var permissionIds = cmsDbContext.MixDatabaseAssociation
                                            .Where(m => m.GuidParentId == role.Id)
                                            .Select(m => m.ChildId);
                        var endpointIds = cmsDbContext.MixDatabaseAssociation
                                            .Where(m => m.ParentDatabaseName == MixDatabaseNames.SYSTEM_PERMISSION
                                                        && m.ChildDatabaseName == MixDatabaseNames.SYSTEM_PERMISSION_ENDPOINT
                                                        && permissionIds.Contains(m.ParentId))
                                            .Select(m => m.ChildId)
                                            .ToList();

                        // TODO: PermissionEndpoint cannot initial at first time
                        var endpoints = await uow.DbContext.PermissionEndpoint.Where(
                                                m => endpointIds.Contains(m.Id)
                                                    && !string.IsNullOrEmpty(m.Path)
                                                )
                                        .GroupBy(m => m.MixTenantId)
                                        .Select(m => new TenantRoleEndpoints
                                        {
                                            MixTenantId = m.Key,
                                            Endpoints = m.ToList()
                                        })
                                        .ToListAsync();

                        if (endpoints.Any())
                        {
                            foreach (var endpoint in endpoints)
                            {
                                RoleEndpoints.Add(
                                    $"{role.Name}-{endpoint.MixTenantId}",
                                    endpoint.Endpoints.Select(p => $"{p.Method.ToLower()}-{p.Path.ToLower()}")
                                        .Distinct().ToArray());
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw new MixException(Heart.Enums.MixErrorStatus.Badrequest, ex);
                }
                finally
                {
                    uow.Dispose();
                    accountDbContext.Dispose();
                }
            }
        }

        public bool CheckEndpointPermission(string[] userRoles, PathString path, string method)
        {
            if (userRoles == null)
            {
                return false;
            }

            foreach (var role in userRoles)
            {
                string currentEndpoint = $"{method.ToLower()}-{path.ToString().ToLower()}";
                if (RoleEndpoints.ContainsKey(role))
                {
                    return RoleEndpoints[role].Any(
                   e => new Regex(e.ToLower()).Match(currentEndpoint).Success);
                }
            }
            return false;
        }

    }
    public class TenantRoleEndpoints
    {
        public int MixTenantId { get; set; }
        public List<MixPermissionEndpoint> Endpoints { get; set; }
    }
}