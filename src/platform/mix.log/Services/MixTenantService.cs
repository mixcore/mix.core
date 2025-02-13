using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Interfaces;
using Mix.Service.Models;
using Mix.Service.Services;

namespace Mix.Log.Lib.Services
{
    public sealed class MixTenantService : IMixTenantService
    {
        private readonly IConfiguration _configuration;
        private readonly DatabaseService _databaseService;

        public List<MixTenantSystemModel> AllTenants { get; set; }

        public List<MixCulture> AllCultures { get; set; }
        public List<MixTheme> AllThemes { get; set; }

        public MixTenantService(DatabaseService databaseService, IConfiguration configuration)
        {
            _databaseService = databaseService;
            _configuration = configuration;
        }

        public async Task Reload(CancellationToken cancellationToken = default)
        {
            if (_configuration.GetValue<InitStep>("InitStatus") != InitStep.Blank)
            {
                using (var dbContext = _databaseService.GetDbContext())
                {
                    var mixTenants = await dbContext.MixTenant.ToListAsync(cancellationToken);

                    var tenantIds = mixTenants.Select(p => p.Id).ToList();

                    var domains = await dbContext.MixDomain.Where(p => tenantIds.Contains(p.TenantId)).ToListAsync(cancellationToken);
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
            return AllTenants?.FirstOrDefault() ?? new MixTenantSystemModel()
            {
                Id = 1
            };
        }
    }
}
