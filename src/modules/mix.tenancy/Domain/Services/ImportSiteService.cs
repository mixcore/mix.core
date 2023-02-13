using Mix.Tenancy.Domain.ViewModels;

namespace Mix.Tenancy.Domain.Services
{
    public class ImportSiteService : IImportSiteService
    {
        private readonly EntityRepository<MixCmsContext, MixConfiguration, int> _configRepo;
        private readonly EntityRepository<MixCmsContext, MixLanguage, int> _languageRepo;

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

            var uowInfo = new UnitOfWorkInfo(dbContext);
            _configRepo.SetUowInfo(uowInfo);
            _languageRepo.SetUowInfo(uowInfo);
            postRepo.SetUowInfo(uowInfo);
            pageRepo.SetUowInfo(uowInfo);
            moduleRepo.SetUowInfo(uowInfo);
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
