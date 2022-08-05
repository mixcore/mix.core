namespace Mix.Portal.Domain.ViewModels
{
    public class MixPostPostAssociationViewModel
        : AssociationViewModelBase<MixCmsContext, MixPostPostAssociation, int, MixPostPostAssociationViewModel>
    {
        #region Properties
        public MixPostContentViewModel Child { get; set; }
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

        public override async Task ExpandView()
        {
            await LoadChild();
        }

        private async Task LoadChild()
        {
            //Child = await MixPostContentViewModel.GetRepository(UowInfo).GetSingleAsync(m => m.Id == ChildId);
        }

        #endregion

        #region Expands

        #endregion
    }
}
