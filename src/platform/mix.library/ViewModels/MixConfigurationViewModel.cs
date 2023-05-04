namespace Mix.Lib.ViewModels
{
    [GenerateRestApiController]
    public sealed class MixConfigurationViewModel
        : SiteDataWithContentViewModelBase
        <MixCmsContext, MixConfiguration, int, MixConfigurationViewModel, MixConfigurationContent, MixConfigurationContentViewModel>
    {
        #region Properties

        public string SystemName { get; set; }

        #endregion

        #region Constructors

        public MixConfigurationViewModel()
        {
        }

        public MixConfigurationViewModel(MixConfiguration entity, UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixConfigurationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion
    }
}
