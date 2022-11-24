using Microsoft.EntityFrameworkCore;
using Mix.Database.Services;
using Mix.Lib.Models;

namespace Mix.Lib.Services
{
    public sealed class MixTenantService
    {
        public List<MixTenantSystemModel> AllTenants { get; set; }
        private readonly DatabaseService _databaseService;

        public MixTenantService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task Reload(CancellationToken cancellationToken = default)
        {
            if (GlobalConfigService.Instance.InitStatus != InitStep.Blank)
            {
                using (var dbContext = _databaseService.GetDbContext())
                {
                    var mixTenants = await dbContext.MixTenant.ToListAsync(cancellationToken);

                    var tenantIds = mixTenants.Select(p => p.Id).ToList();

                    var domains = await dbContext.MixDomain.Where(p => tenantIds.Contains(p.MixTenantId)).ToListAsync(cancellationToken);
                    var cultures = await dbContext.MixCulture.Where(p => tenantIds.Contains(p.MixTenantId)).ToListAsync(cancellationToken);

                    var tenants = mixTenants
                        .Select(p => new MixTenantSystemModel
                        {
                            Id = p.Id,
                            SystemName = p.SystemName,
                            Description = p.Description,
                            PrimaryDomain = p.PrimaryDomain,
                            DisplayName = p.DisplayName,
                            Configurations = new TenantConfigService(p.SystemName).AppSettings,
                            Domains = domains.Where(d => d.MixTenantId == p.Id).ToList(),
                            Cultures = cultures.Where(c => c.MixTenantId == p.Id).ToList(),
                        })
                        .ToList();

                    AllTenants = tenants;
                }
            }
        }

        public MixTenantSystemModel GetTenant(string host)
        {
            return AllTenants.FirstOrDefault(m => m.Domains.Any(d => d.Host == host)) ?? AllTenants.First();
        }
    }
}
