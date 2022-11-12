using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                $"{_configService.GetConfig<string>(MixConfigurationNames.ThemeFolder, culture)}";
        }
    }
}
