namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public sealed class MixLanguageViewModel
        : SiteDataWithContentViewModelBase
        <MixCmsContext, MixLanguage, int, MixLanguageViewModel, MixLanguageContent, MixLanguageContentViewModel>
    {
        #region Properties

        public string SystemName { get; set; }

        #endregion

        #region Constructors

        public MixLanguageViewModel()
        {
        }

        public MixLanguageViewModel(MixLanguage entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixLanguageViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion
    }
}
