namespace Mix.Portal.Domain.ViewModels
{
    public sealed class MixCultureViewModel
        : TenantDataViewModelBase<MixCmsContext, MixCulture, int, MixCultureViewModel>
    {
        #region Properties
        public string Alias { get; set; }
        public string Icon { get; set; }
        public string Lcid { get; set; }
        public string Specificulture { get; set; }
        #endregion

        #region Constructors

        public MixCultureViewModel()
        {
        }

        public MixCultureViewModel(MixCulture entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixCultureViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        protected override async Task DeleteHandlerAsync(CancellationToken cancellationToken = default)
        {
            await base.DeleteHandlerAsync(cancellationToken);
            await MixPageContentViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.Specificulture == Specificulture, cancellationToken);
            await MixModuleContentViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.Specificulture == Specificulture, cancellationToken);
            await MixPostContentViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.Specificulture == Specificulture, cancellationToken);
        }

        #endregion

        #region Expands

        #endregion
    }
}
