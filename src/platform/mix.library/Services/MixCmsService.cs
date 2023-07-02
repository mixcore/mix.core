using Microsoft.AspNetCore.Http;
using Mix.Lib.Interfaces;

namespace Mix.Lib.Services
{
    public sealed class MixCmsService : TenantServiceBase, IMixCmsService
    {
        private readonly MixConfigurationService _configService;
        public MixCmsService(
            IHttpContextAccessor httpContextAccessor, 
            MixConfigurationService configService,
            MixCacheService cacheService)
            : base(httpContextAccessor, cacheService)
        {
            _configService = configService;
        }

        public string GetAssetFolder(string culture, string domain)
        {
            return $"{domain}/" +
                $"{MixFolders.SiteContentAssetsFolder}/" +
                $"{_configService.GetConfig<string>(MixConfigurationNames.ThemeFolder, culture)}";
        }

        public MixTenantSystemModel GetCurrentTenant()
        {
            return CurrentTenant;
        }
    }
}
