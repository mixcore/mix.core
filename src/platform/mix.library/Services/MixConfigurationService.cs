using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Services;
using Mix.Lib.Interfaces;

namespace Mix.Lib.Services
{
    public sealed class MixConfigurationService : TenantServiceBase, IMixConfigurationService
    {
        private readonly DatabaseService _databaseService;

        public List<MixConfigurationContentViewModel> Configs { get; set; }

        public MixConfigurationService(IHttpContextAccessor httpContextAccessor, DatabaseService databaseService, MixCacheService cacheService) 
            : base(httpContextAccessor, cacheService)
        {
            _databaseService = databaseService;
        }

        public async Task Reload(UnitOfWorkInfo<MixCmsContext> uow = null)
        {
            if (GlobalConfigService.Instance.InitStatus != InitStep.Blank)
            {
                if (uow != null)
                {
                    Configs = await MixConfigurationContentViewModel.GetRepository(uow, CacheService).GetAllAsync(
                        m => m.MixTenantId == CurrentTenant.Id);
                }
                else
                {
                    uow = new(new MixCmsContext(_databaseService));
                    Configs = await MixConfigurationContentViewModel.GetRepository(uow, CacheService).GetAllAsync(
                        m => m.MixTenantId == CurrentTenant.Id);
                    uow.Dispose();
                }
            }
        }

        public async Task Set(string name, string content, string culture, int cultureId, UnitOfWorkInfo<MixCmsContext> uow)
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
                    MixTenantId = CurrentTenant.Id
                };
                await config.SaveAsync();
            }
        }


        public async Task<string> GetConfig(string name, string culture, string defaultValue = default)
        {
            if (Configs == null)
            {
                await Reload();
            }
            var config = Configs.FirstOrDefault(m => m.Specificulture == culture && m.SystemName == name);
            return config != null ? config.Content : defaultValue;
        }

        public async Task<T> GetConfig<T>(string name, string culture, T defaultValue = default)
        {
            if (Configs == null)
            {
                await Reload();
            }
            var config = Configs.FirstOrDefault(m => m.Specificulture == culture && m.SystemName == name);
            return config != null ? config.GetValue<T>(): defaultValue;
        }
    }
}
