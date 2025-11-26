namespace Mix.Portal.Domain.ViewModels
{
    public sealed class MixPageViewModel
        : SiteDataWithContentViewModelBase
        <MixCmsContext, MixPage, int, MixPageViewModel, MixPageContent, MixPageContentViewModel>
    {
        #region Constructors

        public MixPageViewModel()
        {
        }

        public MixPageViewModel(MixPage entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixPageViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion
    }
}
