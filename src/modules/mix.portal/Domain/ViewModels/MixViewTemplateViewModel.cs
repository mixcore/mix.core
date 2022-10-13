namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public sealed class MixViewTemplateViewModel
        : ViewModelBase<MixCmsContext, MixTemplate, int, MixViewTemplateViewModel>
    {
        #region Properties

        public string Content { get; set; }
        public string Extension { get; set; }
        public string FileFolder { get; set; }
        public string FileName { get; set; }
        public MixTemplateFolderType FolderType { get; set; }
        public string Scripts { get; set; }
        public string SpaContent { get; set; }
        public string MobileContent { get; set; }
        public string Styles { get; set; }

        #endregion

        #region Constructors

        public MixViewTemplateViewModel()
        {
        }

        public MixViewTemplateViewModel(MixTemplate entity,

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixViewTemplateViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion
    }
}
