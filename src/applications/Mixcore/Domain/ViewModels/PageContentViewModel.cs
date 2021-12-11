using Microsoft.EntityFrameworkCore;

namespace Mixcore.Domain.ViewModels
{
    public class PageContentViewModel
        : ExtraColumnMultilanguageSEOContentViewModelBase
            <MixCmsContext, MixPageContent, int, PageContentViewModel>
    {
        #region Contructors

        public PageContentViewModel()
        {
        }

        public PageContentViewModel(MixPageContent entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
        {
        }

        public PageContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public string ClassName { get; set; }

        public string DetailUrl => $"/Page/{Id}/{SeoName}";

        public List<ModuleContentViewModel> Modules { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView(MixCacheService cacheService = null)
        {
            Modules ??= await LoadModulesAsync(cacheService);
            await base.ExpandView(cacheService);
        }

        #endregion

        #region Private Methods

        private async Task<List<ModuleContentViewModel>> LoadModulesAsync(MixCacheService cacheService)
        {
            var tasks = new List<Task>();
            var moduleIds = Context.MixPageModuleAssociation
                    .AsNoTracking()
                    .Where(p => p.LeftId == Id)
                    .OrderBy(m => m.Priority)
                    .Select(m => m.RightId);
            var moduleRepo = ModuleContentViewModel.GetRepository(UowInfo);
            var modules = await moduleRepo.GetListAsync(m => moduleIds.Contains(m.Id), cacheService);
            var paging = new PagingModel();
            foreach (var item in modules)
            {
                tasks.Add(item.LoadData(paging, cacheService));
            }
            await Task.WhenAll(tasks);
            return modules;
        }
        #endregion
    }
}
