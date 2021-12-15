using Microsoft.EntityFrameworkCore;

namespace Mixcore.Domain.ViewModels
{
    [GenerateRestApiController(QueryOnly = true)]
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

        public Guid? AdditionalDataId { get; set; }

        public List<ModuleContentViewModel> Modules { get; set; }
        public AdditionalDataViewModel AdditionalData { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView(MixCacheService cacheService = null)
        {
            await LoadModulesAsync(cacheService);
            await LoadAdditionalDataAsync(cacheService);
            await base.ExpandView(cacheService);
        }



        #endregion

        #region Public Method
        public ModuleContentViewModel GetModule(string moduleName)
        {
            return Modules.FirstOrDefault(m => m.SystemName == moduleName);
        }
        public T Property<T>(string fieldName)
        {
            return AdditionalData != null 
                ? AdditionalData.Property<T>(fieldName)
                : default;
        }

        #endregion
        #region Private Methods
        private async Task LoadAdditionalDataAsync(MixCacheService cacheService)
        {
            if (AdditionalDataId == default)
            {
                var nav = await Context.MixDataContentAssociation
                    .FirstOrDefaultAsync(m => m.ParentType == MixDatabaseParentType.Page
                        && m.IntParentId == Id);
                AdditionalDataId = nav?.DataContentId;
            }
            if (AdditionalDataId.HasValue)
            {
                var repo = AdditionalDataViewModel.GetRepository(UowInfo);
                AdditionalData = await repo.GetSingleAsync(AdditionalDataId.Value);
            }
        }

        private async Task LoadModulesAsync(MixCacheService cacheService)
        {
            var tasks = new List<Task>();
            var moduleIds = await Context.MixPageModuleAssociation
                    .AsNoTracking()
                    .Where(p => p.LeftId == Id)
                    .OrderBy(m => m.Priority)
                    .Select(m => m.RightId)
                    .ToListAsync();
            var moduleRepo = ModuleContentViewModel.GetRepository(UowInfo);
            Modules = await moduleRepo.GetListAsync(m => moduleIds.Contains(m.Id), cacheService);
            var paging = new PagingModel();
            foreach (var item in Modules)
            {
                tasks.Add(item.LoadData(paging, cacheService));
            }
            await Task.WhenAll(tasks);
        }
        #endregion
    }
}
