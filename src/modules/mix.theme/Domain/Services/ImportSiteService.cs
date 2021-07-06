using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Theme.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Theme.Domain.Services
{
    public class ImportSiteService
    {
        private readonly Repository<MixCmsContext, MixConfiguration, int> _configRepo;
        private readonly Repository<MixCmsContext, MixLanguage, int> _languageRepo;
        private readonly Repository<MixCmsContext, MixPost, int> _postRepo;
        private readonly Repository<MixCmsContext, MixPage, int> _pageRepo;
        private readonly Repository<MixCmsContext, MixModule, int> _moduleRepo;
        private UnitOfWorkInfo _uowInfo;
        public ImportSiteService(
            MixCmsContext dbContext,
            Repository<MixCmsContext, MixConfiguration, int> configRepo,
            Repository<MixCmsContext, MixLanguage, int> languageRepo,
            Repository<MixCmsContext, MixPost, int> postRepo,
            Repository<MixCmsContext, MixPage, int> pageRepo,
            Repository<MixCmsContext, MixModule, int> moduleRepo)
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

        private Dictionary<int, int> dicConfigurationIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicLanguageIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicModuleIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicPostIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicPageIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicFieldIds = new Dictionary<int, int>();
        private Dictionary<int, int> dicMixDatabaseIds = new Dictionary<int, int>();

        public async Task ImportAsync(SiteDataViewModel data, string destCulture)
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
