namespace Mix.Lib.ViewModels.ReadOnly
{
    public sealed class TemplateViewModel
        : ViewModelBase<MixCmsContext, MixTemplate, int, TemplateViewModel>
    {
        #region Properties
        public string Extension { get; set; }
        public string FileFolder { get; set; }
        public string FileName { get; set; }
        public MixTemplateFolderType FolderType { get; set; }
        public string Scripts { get; set; }
        public string Styles { get; set; }
        public string MixThemeName { get; set; }
        public int MixThemeId { get; set; }
        public string FilePath { get; set; }
        #endregion

        #region Constructors

        public TemplateViewModel()
        {
        }

        public TemplateViewModel(MixTemplate entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public TemplateViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        public override Task ExpandView(CancellationToken cancellationToken = default)
        {
            FilePath = $"/{FileFolder}/{FileName}{Extension}";
            return base.ExpandView(cancellationToken);
        }
        #endregion
    }
}
