namespace Mix.Portal.Domain.ViewModels
{
    public class MixPagePostAssociationViewModel
        : AssociationViewModelBase<MixCmsContext, MixPagePostAssociation, int, MixPagePostAssociationViewModel>
    {
        #region Properties
        #endregion

        #region Constructors

        public MixPagePostAssociationViewModel()
        {
        }

        public MixPagePostAssociationViewModel(MixPagePostAssociation entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixPagePostAssociationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides
        #endregion

        #region Expands

        #endregion
    }
}
