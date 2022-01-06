using Microsoft.EntityFrameworkCore;

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
            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
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

        public Guid? AdditionalDataId { get; set; }
        public AdditionalDataViewModel AdditionalData { get; set; }
        public PagingResponseModel<ModuleDataViewModel> SimpleDatas { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView()
        {
            await LoadAdditionalDataAsync();
            await base.ExpandView();
        }

        #region Private Methods

        private async Task LoadAdditionalDataAsync()
        {
            if (AdditionalDataId == default)
            {
                var nav = await Context.MixDataContentAssociation
                    .FirstOrDefaultAsync(
                        m => m.ParentType == MixDatabaseParentType.Module
                        && m.IntParentId == Id);
                AdditionalDataId = nav?.DataContentId;
            }
            if (AdditionalDataId.HasValue)
            {
                var repo = AdditionalDataViewModel.GetRepository(UowInfo);
                AdditionalData = await repo.GetSingleAsync(AdditionalDataId.Value);
            }
        }

        #endregion
        #endregion
        #region Public Methods

        public T Property<T>(string fieldName)
        {
            return AdditionalData != null
                ? AdditionalData.Property<T>(fieldName)
                : default;
        }

        public async Task LoadData(IPagingModel pagingModel, MixCacheService cacheService = null)
        {
            SimpleDatas = await ModuleDataViewModel.GetRepository(UowInfo).GetPagingAsync(
                m => m.ModuleContentId == Id,
                pagingModel,
                cacheService);
        }
        #endregion
    }
}
