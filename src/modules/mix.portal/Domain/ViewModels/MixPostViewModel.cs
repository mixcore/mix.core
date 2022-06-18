namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixPostViewModel
        : SiteDataWithContentViewModelBase
        <MixCmsContext, MixPost, int, MixPostViewModel, MixPostContent, MixPostContentViewModel>
    {
        #region Constructors

        public MixPostViewModel()
        {
        }

        public MixPostViewModel(MixPost entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
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
