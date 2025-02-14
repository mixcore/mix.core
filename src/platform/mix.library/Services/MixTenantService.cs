using Microsoft.EntityFrameworkCore;
using Mix.Database.Services.MixGlobalSettings;
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
            using (var dbContext = _databaseService.GetDbContext())
            {
                var mixTenants = await dbContext.MixTenant.ToListAsync(cancellationToken);

                var tenantIds = mixTenants.Select(p => p.Id).ToList();

                var domains = await dbContext.MixDomain.ToListAsync(cancellationToken);
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
                        Domains = domains.Where(d => d.TenantId == p.Id).ToList(),
                        Cultures = AllCultures.Where(c => c.TenantId == p.Id).ToList(),
                        Configurations = new TenantConfigurationModel()
                        {
                            Domain = domains?.FirstOrDefault()?.Host,
                            DefaultCulture = AllCultures.First().Specificulture
                        },
                        Themes = AllThemes.Where(c => c.TenantId == p.Id).ToList(),
                    })
                    .ToList();

                AllTenants = tenants;
                dbContext.Dispose();
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
