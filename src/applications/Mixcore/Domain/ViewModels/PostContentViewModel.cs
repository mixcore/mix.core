using Microsoft.EntityFrameworkCore;
using Mix.Shared.Services;

namespace Mixcore.Domain.ViewModels
{
    [GenerateRestApiController(QueryOnly = true)]
    public class PostContentViewModel
        : ExtraColumnMultilingualSEOContentViewModelBase
            <MixCmsContext, MixPostContent, int, PostContentViewModel>
    {
        #region Constructors

        public PostContentViewModel()
        {
        }

        public PostContentViewModel(MixPostContent entity,

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public PostContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public string ClassName { get; set; }

        public string DetailUrl => $"{GlobalConfigService.Instance.Domain}/post/{Id}/{SeoName}";

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
