namespace Mix.Portal.Domain.ViewModels
{
    public class MixPostPostAssociationViewModel
        : AssociationViewModelBase<MixCmsContext, MixPostPostAssociation, int, MixPostPostAssociationViewModel>
    {
        #region Properties
        #endregion

        #region Constructors

        public MixPostPostAssociationViewModel()
        {
        }

        public MixPostPostAssociationViewModel(MixPostPostAssociation entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixPostPostAssociationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides
        #endregion

        #region Expands

        #endregion
    }
}
