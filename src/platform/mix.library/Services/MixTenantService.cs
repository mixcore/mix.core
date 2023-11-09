using Microsoft.EntityFrameworkCore;
using Mix.Database.Services;
using Mix.Lib.Interfaces;

namespace Mix.Lib.Services
{
    public sealed class MixTenantService : IMixTenantService
    {
        private readonly DatabaseService _databaseService;

        public List<MixTenantSystemModel> AllTenants { get; set; }

        public List<MixCulture> AllCultures { get; set; }
        public List<MixTheme> AllThemes { get; set; }

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
                    AllCultures = await dbContext.MixCulture.ToListAsync(cancellationToken);
                    AllThemes = await dbContext.MixTheme.ToListAsync(cancellationToken);

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
                            Cultures = AllCultures.Where(c => c.MixTenantId == p.Id).ToList(),
                            Themes = AllThemes.Where(c => c.MixTenantId == p.Id).ToList(),
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

        public async Task<MixTenantSystemModel> GetDefaultTenant()
        {
            if (AllTenants == null)
            {
                await Reload();
            }
            return AllTenants.FirstOrDefault();
        }
    }
}
