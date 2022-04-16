namespace Mix.Portal.Culture.ViewModels
{
    [GenerateRestApiController(IsAuthorized = true)]
    public class MixCultureViewModel
        : TenantDataViewModelBase<MixCmsContext, MixCulture, int, MixCultureViewModel>
    {
        #region Contructors

        public MixCultureViewModel()
        {
        }

        public MixCultureViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixCultureViewModel(MixCulture entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public string Alias { get; set; }
        public string Icon { get; set; }
        public string Lcid { get; set; }
        public string Specificulture { get; set; }

        #endregion

        #region Overrides

        #endregion
    }
}
