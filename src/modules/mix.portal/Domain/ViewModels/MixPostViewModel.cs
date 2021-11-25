namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixPostViewModel 
        : SiteDataWithContentViewModelBase
        <MixCmsContext, MixPost, int, MixPostViewModel, MixPostContent, MixPostContentViewModel>
    {
        #region Contructors

        public MixPostViewModel()
        {
        }

        public MixPostViewModel(MixPost entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null)
            : base(entity, cacheService, uowInfo)
        {
        }

        public MixPostViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion
    }
}
