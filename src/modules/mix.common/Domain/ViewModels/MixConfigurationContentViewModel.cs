namespace Mix.Common.Domain.ViewModels
{
    [GenerateRestApiController(QueryOnly = true)]
    public class MixConfigurationContentViewModel
        : MultilingualUniqueNameContentViewModelBase<MixCmsContext, MixConfigurationContent, int, MixConfigurationContentViewModel>
    {
        #region Properties

        public string DefaultContent { get; set; }
        public string Category { get; set; }
        public MixDataType DataType { get; set; }

        #endregion

        #region Constructors
        public MixConfigurationContentViewModel()
        {
        }

        public MixConfigurationContentViewModel(MixConfigurationContent entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixConfigurationContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion
    }
}
