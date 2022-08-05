namespace Mix.Portal.Domain.ViewModels
{
    public class MixModulePostAssociationViewModel
        : AssociationViewModelBase<MixCmsContext, MixModulePostAssociation, int, MixModulePostAssociationViewModel>
    {
        #region Properties
        #endregion

        #region Constructors

        public MixModulePostAssociationViewModel()
        {
        }

        public MixModulePostAssociationViewModel(MixModulePostAssociation entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixModulePostAssociationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides
        #endregion

        #region Expands

        #endregion
    }
}
