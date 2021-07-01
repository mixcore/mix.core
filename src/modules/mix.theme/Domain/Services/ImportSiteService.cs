using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Theme.Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Theme.Domain.Services
{
    public class ImportSiteService
    {
        private readonly CommandRepository<MixCmsContext, MixConfiguration, int> _configRepo;
        private readonly CommandRepository<MixCmsContext, MixLanguage, int> _languageRepo;
        private readonly CommandRepository<MixCmsContext, MixPost, int> _postRepo;
        private readonly CommandRepository<MixCmsContext, MixPage, int> _pageRepo;
        private readonly CommandRepository<MixCmsContext, MixModule, int> _moduleRepo;
        private UnitOfWorkInfo _uowInfo;
        public ImportSiteService(
            MixCmsContext dbContext,
            CommandRepository<MixCmsContext, MixConfiguration, int> configRepo,
            CommandRepository<MixCmsContext, MixLanguage, int> languageRepo,
            CommandRepository<MixCmsContext, MixPost, int> postRepo,
            CommandRepository<MixCmsContext, MixPage, int> pageRepo,
            CommandRepository<MixCmsContext, MixModule, int> moduleRepo)
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

        public async Task<bool> ImportAsync(SiteDataViewModel data, string destCulture)
        {
            var result = true;
            return result;
        }

        #endregion Import
    }
}
