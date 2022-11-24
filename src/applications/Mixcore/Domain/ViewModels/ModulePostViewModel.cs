namespace Mixcore.Domain.ViewModels
{
    public sealed class ModulePostAssociationViewModel
        : AssociationViewModelBase<MixCmsContext, MixModulePostAssociation, int, ModulePostAssociationViewModel>
    {
        #region Properties
        public PostContentViewModel Post { get; set; }
        #endregion

        #region Constructors

        public ModulePostAssociationViewModel()
        {
        }

        public ModulePostAssociationViewModel(MixModulePostAssociation entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public ModulePostAssociationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Post = await PostContentViewModel.GetRepository(UowInfo).GetSingleAsync(ChildId);
        }
        #endregion

        #region Expands

        #endregion
    }
}
