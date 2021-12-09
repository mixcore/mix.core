namespace Mixcore.Domain.ViewModels
{
    public class ModuleContentViewModel
        : ExtraColumnMultilanguageSEOContentViewModelBase
            <MixCmsContext, MixModuleContent, int, ModuleContentViewModel>
    {
        #region Contructors

        public ModuleContentViewModel()
        {
        }

        public ModuleContentViewModel(MixModuleContent entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
        {
        }

        public ModuleContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public string SystemName { get; set; }
        public string ClassName { get; set; }
        public int? PageSize { get; set; }
        public MixModuleType Type { get; set; }

        public string DetailUrl => $"/Module/{Id}/{SeoName}";

        public PagingResponseModel<ModuleDataViewModel> SimpleDatas { get; set; }
        #endregion

        #region Overrides

        #endregion

        public async Task LoadData(IPagingModel pagingModel, MixCacheService cacheService = null)
        {
            SimpleDatas = await ModuleDataViewModel.GetRepository(UowInfo).GetPagingAsync(
                m => m.ModuleContentId == Id, 
                pagingModel,
                cacheService);
        }
    }
}
