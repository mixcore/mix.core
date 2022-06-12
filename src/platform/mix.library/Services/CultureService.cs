using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mix.Lib.Services
{
    public class CultureService : JsonConfigurationServiceBase
    {
        private MixCmsContext _ctx;
        private readonly IServiceProvider servicesProvider;
        public CultureService(IServiceProvider servicesProvider)
            : base(MixAppConfigFilePaths.Culture)
        {
            this.servicesProvider = servicesProvider;
            if (!GlobalConfigService.Instance.AppSettings.IsInit)
            {
                LoadCultures();
            }

        }

        public List<MixCulture> Cultures { get; set; }
        public MixCulture DefaultCulture { get => Cultures.FirstOrDefault(); }

        public bool CheckValidCulture(string specificulture)
        {
            if (Cultures == null)
            {
                LoadCultures();
            }
            return Cultures != null && Cultures.Any(c => c.Specificulture == specificulture);
        }

        public MixCulture LoadCulture(string specificulture)
        {
            return Cultures.FirstOrDefault(m => m.Specificulture == specificulture) ?? DefaultCulture;
        }

        public void LoadCultures()
        {
            using var scope = servicesProvider.CreateScope();
            _ctx = scope.ServiceProvider.GetService<MixCmsContext>();
            Cultures = _ctx.MixCulture.AsNoTracking().ToList();
            SetConfig(MixAppSettingKeywords.Cultures, Cultures);
        }
    }
}
