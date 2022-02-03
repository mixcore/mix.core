using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixTemplates
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixTemplate, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("themeId")]
        public int ThemeId { get; set; }

        [JsonProperty("themeName")]
        public string ThemeName { get; set; }

        [JsonProperty("folderType")]
        public string FolderType { get; set; }

        [JsonProperty("fileFolder")]
        public string FileFolder { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("extension")]
        public string Extension { get; set; } = ".cshtml";

        [Required]
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("mobileContent")]
        public string MobileContent { get; set; } = "{}";

        [JsonProperty("spaContent")]
        public string SpaContent { get; set; } = "";

        [JsonProperty("scripts")]
        public string Scripts { get; set; }

        [JsonProperty("styles")]
        public string Styles { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("layout")]
        public string Layout { get; set; }

        [JsonProperty("assetFolder")]
        public string AssetFolder {
            get {
                return $"{MixFolders.SiteContentAssetsFolder}/{ThemeName}";
            }
        }

        [JsonProperty("templateFolder")]
        public string TemplateFolder {
            get {
                return $"{MixFolders.TemplatesFolder}/{ThemeName}";
            }
        }

        [JsonProperty("templatePath")]
        public string TemplatePath {
            get {
                return $"/{FileFolder}/{FileName}{Extension}";
            }
        }

        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel()
            : base()
        {
        }

        public UpdateViewModel(MixTemplate model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        #region Common

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                var file = MixFileRepository.Instance.GetFile(FileName, Extension, FileFolder);
                if (!string.IsNullOrWhiteSpace(file?.Content))
                {
                    Content = file.Content;
                }
            }
            Scripts ??= "<script>\r\n\r\n</script>";
            Styles ??= "<style>\r\n\r\n</style>";
        }

        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid)
            {
                if (Id == 0)
                {
                    if (_context.MixTemplate.Any(t => t.FileName == FileName && t.FolderType == FolderType && t.ThemeId == ThemeId))
                    {
                        IsValid = false;
                        Errors.Add($"{FileName} is existed");
                    }
                }
                if (string.IsNullOrEmpty(ThemeName) && ThemeId > 0)
                {
                    ThemeName = _context.MixTheme.FirstOrDefault(m => m.Id == ThemeId)?.Name;
                }
            }
        }

        public override MixTemplate ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(m => m.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            FileFolder = $"{MixFolders.TemplatesFolder}/{ThemeName}/{FolderType}";
            Content = Content?.Trim();
            Scripts = Scripts?.Trim();
            Styles = Styles?.Trim();
            return base.ParseModel(_context, _transaction);
        }

        #endregion Common

        #region Async

        public override async Task<RepositoryResponse<UpdateViewModel>> SaveModelAsync(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = await base.SaveModelAsync(isSaveSubModels, _context, _transaction);
            if (result.IsSucceed)
            {
                // Save other themes if not exist
            }
            return result;
        }

        public override RepositoryResponse<MixTemplate> RemoveModel(bool isRemoveRelatedModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = base.RemoveModel(isRemoveRelatedModels, _context, _transaction);
            if (result.IsSucceed)
            {
                TemplateRepository.Instance.DeleteTemplate(FileName, FileFolder);
            }
            return result;
        }

        public override RepositoryResponse<bool> SaveSubModels(MixTemplate parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            TemplateRepository.Instance.SaveTemplate(new TemplateViewModel()
            {
                Filename = FileName,
                Extension = Extension,
                Content = Content,
                FileFolder = FileFolder
            });
            return base.SaveSubModels(parent, _context, _transaction);
        }

        #endregion Async

        #region Async

        public override async Task<RepositoryResponse<MixTemplate>> RemoveModelAsync(bool isRemoveRelatedModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = await base.RemoveModelAsync(isRemoveRelatedModels, _context, _transaction);
            if (result.IsSucceed)
            {
                TemplateRepository.Instance.DeleteTemplate(FileName, FileFolder);
            }
            return result;
        }

        public override Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixTemplate parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            TemplateRepository.Instance.SaveTemplate(new TemplateViewModel()
            {
                Filename = FileName,
                Extension = Extension,
                Content = Content,
                FileFolder = FileFolder
            });
            return base.SaveSubModelsAsync(parent, _context, _transaction);
        }

        #endregion Async

        #endregion Overrides

        #region Expands

        /// <summary>
        /// Gets the template by path.
        /// </summary>
        /// <param name="path">The path.</param> Ex: "Pages/_Home"
        /// <returns></returns>
        public static RepositoryResponse<UpdateViewModel> GetTemplateByPath(string path, string culture
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<UpdateViewModel> result = new RepositoryResponse<UpdateViewModel>();
            string[] temp = path?.Split('/');
            if (temp == null || temp.Length < 2)
            {
                result.IsSucceed = false;
                result.Errors.Add("Template Not Found");
            }
            else
            {
                int activeThemeId = MixService.GetConfig<int>(
                    MixAppSettingKeywords.ThemeId, culture);
                string name = temp[1].Split('.')[0];
                result = Repository.GetSingleModel(t => t.FolderType == temp[0] && t.FileName == name && t.ThemeId == activeThemeId
                    , _context, _transaction);
            }
            return result;
        }

        public static UpdateViewModel GetTemplateByPath(string path, string specificulture, string folderType
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            string templateName = !string.IsNullOrEmpty(path) ? path.Substring(path.LastIndexOf('/') + 1) : null;
            string filename = templateName?.Substring(0, templateName.LastIndexOf('.'));
            string ext = templateName?.Substring(templateName.LastIndexOf('.'));
            int themeId = MixService.GetConfig<int>(MixAppSettingKeywords.ThemeId, specificulture);
            string themeName = MixService.GetConfig<string>(MixAppSettingKeywords.ThemeName, specificulture);
            var getView = UpdateViewModel.Repository.GetSingleModel(t =>
                    t.ThemeId == themeId && t.FolderType == folderType.ToString()
                    && !string.IsNullOrEmpty(templateName) && t.FileName == filename && t.Extension == ext
                    , _context, _transaction);
            return getView.Data ?? GetDefault(folderType, specificulture, _context, _transaction);
        }

        public static UpdateViewModel GetDefault(string folderType, string specificulture
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            string activedTheme = MixService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, specificulture)
                    ?? MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultTheme);
            string folder = $"{MixFolders.TemplatesFolder}/{activedTheme}/{folderType}";
            var defaulTemplate = MixTemplates.UpdateViewModel.Repository.GetModelListBy(
                t => t.Theme.Name == activedTheme && t.FolderType == folderType.ToString()
                , _context, _transaction
                ).Data?.FirstOrDefault();
            return defaulTemplate ?? new UpdateViewModel(new MixTemplate()
            {
                ThemeId = MixService.GetConfig<int>(MixAppSettingKeywords.ThemeId, specificulture),
                ThemeName = MixService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, specificulture),
                FileName = MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultTemplate),
                Extension = MixService.GetAppSetting<string>(MixAppSettingKeywords.TemplateExtension),
                Content = MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultTemplateContent),
                FolderType = folderType.ToString(),
                FileFolder = folder.ToString()
            });
        }

        public async Task<RepositoryResponse<UpdateViewModel>> CopyAsync()
        {
            var result = await Repository.GetSingleModelAsync(m => m.Id == Id);
            result.Data.Id = 0;
            result.Data.FileName = $"Copy_{result.Data.FileName}";
            // Not write file to disk
            return await result.Data.SaveModelAsync(false);
        }

        #endregion Expands
    }
}