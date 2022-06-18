using System.ComponentModel.DataAnnotations;

namespace Mix.Lib.ViewModels
{
    [GeneratePublisher]
    public class MixTemplateViewModel
        : ViewModelBase<MixCmsContext, MixTemplate, int, MixTemplateViewModel>
    {
        #region Properties
        public int MixTenantId { get; set; }
        public string Content { get; set; }
        public string Extension { get; set; }
        public string FileFolder { get; set; }
        public string FileName { get; set; }
        public MixTemplateFolderType FolderType { get; set; }
        public string Scripts { get; set; }
        public string Styles { get; set; }

        public string MixThemeName { get; set; }
        public int MixThemeId { get; set; }

        #endregion

        #region Constructors

        public MixTemplateViewModel()
        {
        }

        public MixTemplateViewModel(MixTemplate entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixTemplateViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        public override Task ExpandView()
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                var file = MixFileHelper.GetFile(FileName, Extension, FileFolder);
                if (!string.IsNullOrWhiteSpace(file?.Content))
                {
                    Content = file.Content;
                }
            }
            Scripts ??= "<script>\r\n\r\n</script>";
            Styles ??= "<style>\r\n\r\n</style>";
            return Task.CompletedTask;
        }

        public override async Task<MixTemplate> ParseEntity()
        {
            if (Id == 0)
            {
                CreatedDateTime = DateTime.UtcNow;
                MixThemeName = Context.MixTheme.First(m => m.Id == MixThemeId).SystemName;
            }

            FileFolder = $"{MixFolders.TemplatesFolder}/{MixThemeName}/{FolderType}";
            Content = Content?.Trim();
            Scripts = Scripts?.Trim();
            Styles = Styles?.Trim();
            return await base.ParseEntity();
        }

        public override async Task Validate()
        {
            await base.Validate();
            if (IsValid)
            {
                if (Id == 0)
                {
                    if (Context.MixViewTemplate.Any(
                            t => t.FileName == FileName
                                && t.FolderType == FolderType
                                && t.MixThemeId == MixThemeId))
                    {
                        IsValid = false;
                        Errors.Add(new ValidationResult($"{FileName} is existed") { });
                    }
                }
                if (string.IsNullOrEmpty(MixThemeName) && MixThemeId > 0)
                {
                    MixThemeName = Context.MixTheme.FirstOrDefault(m => m.Id == MixThemeId)?.SystemName;
                }
            }
        }

        #endregion

        #region Expands

        public async Task<MixTemplateViewModel> CopyAsync()
        {
            var result = await Repository.GetSingleAsync(m => m.Id == Id);
            result.Id = 0;
            result.FileName = $"Copy_{result.FileName}";
            // Not write file to disk
            result.Id = await result.SaveAsync();
            return result;
        }

        #endregion
    }
}
