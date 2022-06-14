namespace Mix.Portal.Domain.ViewModels
{
    public class MixCultureViewModel
        : TenantDataViewModelBase<MixCmsContext, MixCulture, int, MixCultureViewModel>
    {
        #region Properties
        public string Alias { get; set; }
        public string Icon { get; set; }
        public string Lcid { get; set; }
        public string Specificulture { get; set; }
        #endregion

        #region Contructors

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

        protected override async Task DeleteHandlerAsync()
        {
            await base.DeleteHandlerAsync();
            await MixPageContentViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.Specificulture == Specificulture);
            await MixModuleContentViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.Specificulture == Specificulture);
            await MixPostContentViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.Specificulture == Specificulture);
            await MixDataContentViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.Specificulture == Specificulture);
            Context.MixDataContent.RemoveRange(Context.MixDataContent.Where(m => m.Specificulture == Specificulture));
            Context.MixDataContentAssociation.RemoveRange(Context.MixDataContentAssociation.Where(m => m.Specificulture == Specificulture));
            Context.MixDataContentValue.RemoveRange(Context.MixDataContentValue.Where(m => m.Specificulture == Specificulture));
        }

        #endregion

        #region Expands

        #endregion
    }
}
