namespace Mix.Portal.Domain.ViewModels
{
    public class MixPagePostAssociationViewModel
        : AssociationViewModelBase<MixCmsContext, MixPagePostAssociation, int, MixPagePostAssociationViewModel>
    {
        #region Properties
        public MixPostContentViewModel Post { get; set; }
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

        public async Task LoadPost()
        {
            Post = await MixPostContentViewModel.GetRepository(UowInfo).GetSingleAsync(ChildId);
        }
        #endregion
    }
}
