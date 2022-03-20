namespace Mix.Lib.ViewModels
{
    [GeneratePublisher]
    public class MixThemeViewModel
        : TenantDataViewModelBase<MixCmsContext, MixTheme, int, MixThemeViewModel>
    {
        #region Properties
        public string SystemName { get; set; }
        public string PreviewUrl { get; set; }
        public string MixDatabaseName { get; set; }
        public Guid? MixDataContentId { get; set; }

        public string AssetFolder
        {
            get
            {
                return $"{MixFolders.SiteContentAssetsFolder}/{SystemName}";
            }
        }

        public string UploadsFolder
        {
            get
            {
                return MixFolders.UploadsFolder;
            }
        }

        public string TemplateFolder
        {
            get
            {
                return $"{MixFolders.TemplatesFolder}/{SystemName}";
            }
        }
        #endregion

        #region Contructors

        public MixThemeViewModel()
        {
        }

        public MixThemeViewModel(MixTheme entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixThemeViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        public override Task<MixTheme> ParseEntity()
        {
            if (string.IsNullOrEmpty(SystemName))
            {
                SystemName = SeoHelper.GetSEOString(DisplayName);
            }
            return base.ParseEntity();
        }

        protected override Task SaveEntityRelationshipAsync(MixTheme parentEntity)
        {
            // Create default extra data
            MixDataContentViewModel extraData = new();

            return base.SaveEntityRelationshipAsync(parentEntity);
        }

        #endregion

        #region Expands
        #endregion
    }
}
