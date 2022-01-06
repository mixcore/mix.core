using Microsoft.EntityFrameworkCore;

namespace Mixcore.Domain.ViewModels
{
    [GenerateRestApiController(QueryOnly = true)]
    public class PostContentViewModel
        : ExtraColumnMultilanguageSEOContentViewModelBase
            <MixCmsContext, MixPostContent, int, PostContentViewModel>
    {
        #region Contructors

        public PostContentViewModel()
        {
        }

        public PostContentViewModel(MixPostContent entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public PostContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public string ClassName { get; set; }

        public string DetailUrl => $"/post/{Id}/{SeoName}";

        public Guid? AdditionalDataId { get; set; }

        public AdditionalDataViewModel AdditionalData { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView()
        {
            await LoadAdditionalDataAsync();
            await base.ExpandView();
        }



        #endregion

        #region Public Method

        public T Property<T>(string fieldName)
        {
            return AdditionalData != null 
                ? AdditionalData.Property<T>(fieldName)
                : default;
        }

        #endregion
        #region Private Methods
        private async Task LoadAdditionalDataAsync()
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

        #endregion
    }
}
