using Mix.Tenancy.Domain.ViewModels;

namespace Mix.Tenancy.Domain.Services
{
    public class ImportSiteService
    {
        private readonly EntityRepository<MixCmsContext, MixConfiguration, int> _configRepo;
        private readonly EntityRepository<MixCmsContext, MixLanguage, int> _languageRepo;
        private readonly EntityRepository<MixCmsContext, MixPost, int> _postRepo;
        private readonly EntityRepository<MixCmsContext, MixPage, int> _pageRepo;
        private readonly EntityRepository<MixCmsContext, MixModule, int> _moduleRepo;
        private readonly UnitOfWorkInfo _uowInfo;

        private readonly Dictionary<int, int> dicConfigurationIds = new();
        private readonly Dictionary<int, int> dicLanguageIds = new();
        private readonly Dictionary<int, int> dicModuleIds = new();
        private readonly Dictionary<int, int> dicPostIds = new();
        private readonly Dictionary<int, int> dicPageIds = new();
        private readonly Dictionary<int, int> dicFieldIds = new();
        private readonly Dictionary<int, int> dicMixDatabaseIds = new();

        public ImportSiteService(
            MixCmsContext dbContext,
            EntityRepository<MixCmsContext, MixConfiguration, int> configRepo,
            EntityRepository<MixCmsContext, MixLanguage, int> languageRepo,
            EntityRepository<MixCmsContext, MixPost, int> postRepo,
            EntityRepository<MixCmsContext, MixPage, int> pageRepo,
            EntityRepository<MixCmsContext, MixModule, int> moduleRepo)
        {
            _configRepo = configRepo;
            _languageRepo = languageRepo;
            _postRepo = postRepo;
            _pageRepo = pageRepo;
            _moduleRepo = moduleRepo;

            _uowInfo = new UnitOfWorkInfo(dbContext);
            _configRepo.SetUowInfo(_uowInfo);
            _languageRepo.SetUowInfo(_uowInfo);
            _postRepo.SetUowInfo(_uowInfo);
            _pageRepo.SetUowInfo(_uowInfo);
            _moduleRepo.SetUowInfo(_uowInfo);
        }
        #region Import

        public async Task ImportAsync(TenantDataViewModel data, string destCulture)
        {
            if (data.Configurations != null && data.Configurations.Count > 0)
            {
                await ImportConfigurationsAsync(data.Configurations, destCulture);
            }

            if (data.Languages != null && data.Languages.Count > 0)
            {
                await ImportLanguagesAsync(data.Languages, destCulture);
            }
        }

        private async Task ImportLanguagesAsync(List<MixLanguage> languages, string destCulture)
        {
            foreach (var item in languages)
            {
                await _languageRepo.SaveAsync(item);
            }
        }

        private async Task ImportConfigurationsAsync(List<MixConfiguration> configurations, string destCulture)
        {
            foreach (var item in configurations)
            {
                await _configRepo.SaveAsync(item);
            }
        }

        #endregion Import
    }
}
