using Microsoft.EntityFrameworkCore.Storage;
using Mix.Shared.Constants;
using Mix.Database.Entities.Cms;
using Mix.Shared.Services;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Mix.Lib.Services
{
    public class CultureService : JsonConfigurationServiceBase
    {
        private readonly GlobalConfigService _globalConfigService;
        public CultureService(GlobalConfigService globalConfigService) : base(MixAppConfigFilePaths.Culture)
        {
            _globalConfigService = globalConfigService;
            if (!_globalConfigService.IsInit)
            {
                Cultures = AppSettings[MixAppSettingKeywords.Cultures]?.ToObject<List<MixCulture>>();
                if (Cultures == null)
                {
                    using var ctx = new MixCmsContext();
                    Cultures = ctx.MixCulture.ToList();
                    SetConfig(MixAppSettingKeywords.Cultures, Cultures);
                }
            }
        }

        public List<MixCulture> Cultures { get; set; }
        public MixCulture DefaultCulture { get => Cultures.FirstOrDefault(); }

        public MixCulture LoadCulture(string specificulture)
        {
            return Cultures.FirstOrDefault(m => m.Specificulture == specificulture) ?? DefaultCulture;
        }
    }
}
