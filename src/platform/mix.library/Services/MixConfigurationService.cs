using FirebaseAdmin.Auth.Multitenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Interfaces;

namespace Mix.Lib.Services
{
    public sealed class MixConfigurationService : IMixConfigurationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DatabaseService _databaseService;

        public List<MixConfigurationContentViewModel> Configs { get; set; }
        public IConfiguration Configuration { get; }

        public MixConfigurationService(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            DatabaseService databaseService,
            IServiceProvider serviceProvider,
            IMixTenantService mixTenantService)
        {
            _serviceProvider = serviceProvider;
            Configuration = configuration;
            _databaseService = databaseService;
        }

        public async Task Reload(int tenantId, UnitOfWorkInfo<MixCmsContext> uow = null)
        {
            if (Configuration.GetValue<InitStep>("InitStatus") != InitStep.Blank)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var cacheService = scope.ServiceProvider.GetService<MixCacheService>();
                    if (uow != null)
                    {
                        uow = scope.ServiceProvider.GetService<UnitOfWorkInfo<MixCmsContext>>();
                        Configs = await MixConfigurationContentViewModel.GetRepository(uow, cacheService).GetAllAsync(
                            m => m.TenantId == tenantId);
                    }
                    else
                    {
                        uow = new(new MixCmsContext(_databaseService));
                        try
                        {
                            Configs = await MixConfigurationContentViewModel
                                .GetRepository(uow, cacheService)
                                .GetAllAsync(p => true);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"MixConfigurationService getting config error {ex.Message}");
                        }
                        finally
                        {
                            uow.Dispose();
                        }
                    }
                }
            }
        }

        public async Task Set(string name, string content, string culture, int cultureId, UnitOfWorkInfo<MixCmsContext> uow, int tenantId)
        {
            var currentConfig = await uow.DbContext.MixConfigurationContent.FirstOrDefaultAsync(c => c.SystemName == name && c.MixCultureId == cultureId);
            if (currentConfig != null)
            {
                currentConfig.Content = content;
                await uow.DbContext.SaveChangesAsync();
            }
            else
            {
                MixConfigurationContentViewModel config = new MixConfigurationContentViewModel(uow)
                {
                    DisplayName = name,
                    SystemName = SeoHelper.GetSEOString(name),
                    Content = content,
                    Specificulture = culture,
                    MixCultureId = cultureId,
                    TenantId = tenantId
                };
                await config.SaveAsync();
            }
        }


        public async Task<string> GetConfig(string name, string culture, int tenantId, string defaultValue = default)
        {
            if (Configs == null)
            {
                await Reload(tenantId);
            }
            var config = Configs.FirstOrDefault(m => m.Specificulture == culture && m.SystemName == name);
            return config != null ? config.Content : defaultValue;
        }

        public async Task<T> GetConfig<T>(string name, string culture, int tenantId, T defaultValue = default)
        {
            if (Configs == null)
            {
                await Reload(tenantId);
            }
            var config = Configs.FirstOrDefault(m => m.Specificulture == culture && m.SystemName == name);
            return config != null ? config.GetValue<T>() : defaultValue;
        }
    }
}
