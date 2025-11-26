namespace Mix.Common.Domain.ViewModels
{
    public class MixLanguageContentViewModel
        : MultilingualUniqueNameContentViewModelBase<MixCmsContext, MixLanguageContent, int, MixLanguageContentViewModel>
    {
        #region Properties

        public string DefaultContent { get; set; }
        public string Category { get; set; }
        public MixDataType DataType { get; set; }

        #endregion

        #region Constructors
        public MixLanguageContentViewModel()
        {
        }

        public MixLanguageContentViewModel(MixLanguageContent entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixLanguageContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion
    }
}
