namespace Mix.Portal.Domain.ViewModels
{
    public sealed class MixModulePostAssociationViewModel
        : AssociationViewModelBase<MixCmsContext, MixModulePostAssociation, int, MixModulePostAssociationViewModel>
    {
        #region Properties
        public MixPostContentViewModel Post { get; set; }
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
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Post = await MixPostContentViewModel.GetRepository(UowInfo, CacheService).GetSingleAsync(ChildId);
        }
        #endregion

        #region Expands

        #endregion
    }
}
