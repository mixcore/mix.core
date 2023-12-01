using Mix.Services.Databases.Lib.Interfaces;

namespace Mixcore.Domain.ViewModels
{
    public sealed class PagePostAssociationViewModel
        : AssociationViewModelBase<MixCmsContext, MixPagePostAssociation, int, PagePostAssociationViewModel>
    {
        #region Properties
        public PostContentViewModel Post { get; set; }
        #endregion

        #region Constructors

        public PagePostAssociationViewModel()
        {
        }

        public PagePostAssociationViewModel(MixPagePostAssociation entity,

            UnitOfWorkInfo? uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public PagePostAssociationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides
        #endregion

        #region Expands

        public async Task LoadPost(Mix.RepoDb.Repositories.MixRepoDbRepository mixRepoDbRepository,
            IMixMetadataService metadataService,
            MixCacheService cacheService)
        {
            Post = await PostContentViewModel.GetRepository(UowInfo, CacheService).GetSingleAsync(ChildId);
            await Post.LoadAdditionalDataAsync(mixRepoDbRepository, metadataService, cacheService);
        }
        #endregion
    }
}
