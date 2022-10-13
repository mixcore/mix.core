namespace Mix.Portal.Domain.ViewModels
{
    public sealed class MixPageModuleViewModel
        : AssociationViewModelBase<MixCmsContext, MixPageModuleAssociation, int, MixPageModuleViewModel>
    {
        #region Properties
        #endregion

        #region Constructors

        public MixPageModuleViewModel()
        {
        }

        public MixPageModuleViewModel(MixPageModuleAssociation entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixPageModuleViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides
        #endregion

        #region Expands

        #endregion
    }
}
