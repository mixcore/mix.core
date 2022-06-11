namespace Mix.Lib.Services
{
    public class CultureService : JsonConfigurationServiceBase
    {
        private readonly MixCmsContext _ctx;
        public CultureService(MixCmsContext ctx)
            : base(MixAppConfigFilePaths.Culture)
        {
            _ctx = ctx;
            if (!GlobalConfigService.Instance.AppSettings.IsInit)
            {
                LoadCultures();
            }
        }

        public List<MixCulture> Cultures { get; set; }
        public MixCulture DefaultCulture { get => Cultures.FirstOrDefault(); }

        public bool CheckValidCulture(string specificulture)
        {
            return Cultures != null && Cultures.Any(c => c.Specificulture == specificulture);
        }

        public MixCulture LoadCulture(string specificulture)
        {
            return Cultures.FirstOrDefault(m => m.Specificulture == specificulture) ?? DefaultCulture;
        }

        public void LoadCultures()
        {
            Cultures = AppSettings[MixAppSettingKeywords.Cultures]?.ToObject<List<MixCulture>>();
            if (Cultures == null)
            {
                Cultures = _ctx.MixCulture.ToList();
                SetConfig(MixAppSettingKeywords.Cultures, Cultures);
            }
        }
    }
}
