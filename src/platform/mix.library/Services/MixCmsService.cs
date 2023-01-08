using Microsoft.AspNetCore.Http;

namespace Mix.Lib.Services
{
    public sealed class MixCmsService : TenantServiceBase
    {
        private readonly MixConfigurationService _configService;
        public MixCmsService(IHttpContextAccessor httpContextAccessor, MixConfigurationService configService) : base(httpContextAccessor)
        {
            _configService = configService;
        }

        public string GetAssetFolder(string culture, string domain)
        {
            return $"{domain}/" +
                $"{MixFolders.SiteContentAssetsFolder}/" +
                $"{_configService.GetConfig(MixConfigurationNames.ThemeFolder, culture)}";
        }
    }
}
