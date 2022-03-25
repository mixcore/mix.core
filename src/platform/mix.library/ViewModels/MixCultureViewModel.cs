namespace Mix.Lib.ViewModels
{
    [GenerateRestApiController]
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

        #endregion

        #region Expands

        #endregion
    }
}
