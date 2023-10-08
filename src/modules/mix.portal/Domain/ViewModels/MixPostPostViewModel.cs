namespace Mix.Portal.Domain.ViewModels
{
    public sealed class MixPostPostAssociationViewModel
        : AssociationViewModelBase<MixCmsContext, MixPostPostAssociation, int, MixPostPostAssociationViewModel>
    {
        #region Properties
        public MixPortalPostContentViewModel Child { get; set; }
        #endregion

        #region Constructors

        public MixPostPostAssociationViewModel()
        {
        }

        public MixPostPostAssociationViewModel(MixPostPostAssociation entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }

        public MixPostPostAssociationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Child = await MixPortalPostContentViewModel.GetRepository(UowInfo, CacheService).GetSingleAsync(m => m.Id == ChildId, cancellationToken);
        }
        #endregion

        #region Expands

        #endregion
    }
}
