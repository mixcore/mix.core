using Mix.Shared.Constants;
using Mix.Database.Entities.Cms;
using Mix.Shared.Services;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Lib.Services
{
    public class CultureService : JsonConfigurationServiceBase
    {
        public CultureService(MixCmsContext ctx) 
            : base(MixAppConfigFilePaths.Culture)
        {
            
            if (!GlobalConfigService.Instance.AppSettings.IsInit)
            {
                Cultures = AppSettings[MixAppSettingKeywords.Cultures]?.ToObject<List<MixCulture>>();
                if (Cultures == null)
                {
                    Cultures = ctx.MixCulture.ToList();
                    SetConfig(MixAppSettingKeywords.Cultures, Cultures);
                }
            }
        }

        public List<MixCulture> Cultures { get; set; }
        public MixCulture DefaultCulture { get => Cultures.FirstOrDefault(); }

        public bool CheckValidCulture(string specificulture)
        {
            return Cultures.Any(c => c.Specificulture == specificulture);
        }

        public MixCulture LoadCulture(string specificulture)
        {
            return Cultures.FirstOrDefault(m => m.Specificulture == specificulture) ?? DefaultCulture;
        }
    }
}
