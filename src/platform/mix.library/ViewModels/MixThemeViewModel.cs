namespace Mix.Lib.ViewModels
{
    public sealed class MixThemeViewModel
        : TenantDataViewModelBase<MixCmsContext, MixTheme, int, MixThemeViewModel>
    {
        #region Properties
        public string SystemName { get; set; }
        public string PreviewUrl { get; set; }
        public string ImageUrl { get; set; }
        public string MixDatabaseName { get; set; }
        public int? MixDbId { get; set; }
        public JObject ExtraData { get; set; }
        public string AssetFolder { get; set; }
        public string TemplateFolder { get; set; }

        public bool IsActive { get; set; }
        #endregion

        #region Constructors

        public MixThemeViewModel()
        {
        }

        public MixThemeViewModel(MixTheme entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        public MixThemeViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        public override Task<MixTheme> ParseEntity(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(SystemName))
            {
                SystemName = SeoHelper.GetSEOString(DisplayName);
            }

            return base.ParseEntity(cancellationToken);
        }

        #endregion

        #region Expands
        #endregion
    }
}
