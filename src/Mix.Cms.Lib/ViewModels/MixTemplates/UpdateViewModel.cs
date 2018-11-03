using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
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
        public string Extension { get; set; }

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

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("layout")]
        public string Layout { get; set; }

        [JsonProperty("assetFolder")]
        public string AssetFolder
        {
            get
            {
                return CommonHelper.GetFullPath(new string[] {
                    MixConstants.Folder.FileFolder,
                    MixConstants.Folder.TemplatesAssetFolder,
                    ThemeName });
            }
        }

        [JsonProperty("templateFolder")]
        public string TemplateFolder
        {
            get
            {
                return CommonHelper.GetFullPath(new string[] { MixConstants.Folder.TemplatesFolder, ThemeName });
            }
        }

        [JsonProperty("templatePath")]
        public string TemplatePath
        {
            get
            {
                return CommonHelper.GetFullPath(new string[]
                {
                    ""
                    , TemplateFolder
                    , FileFolder
                });
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
            var file = FileRepository.Instance.GetFile(FileName, Extension, FileFolder);
            if (!string.IsNullOrWhiteSpace(file?.Content))
            {
                Content = file.Content;
            }
        }

        public override MixTemplate ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                CreatedDateTime = DateTime.UtcNow;
            }
            FileFolder = CommonHelper.GetFullPath(new string[]
                {
                    MixConstants.Folder.TemplatesFolder
                    , ThemeName
                    , FolderType
                });

            Content = Content?.Trim();
            Scripts = Scripts?.Trim();
            Styles = Styles?.Trim();
            return base.ParseModel(_context, _transaction);
        }

        #endregion Common

        #region Async

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
            string[] temp = path.Split('/');
            if (temp.Length < 2)
            {
                result.IsSucceed = false;
                result.Errors.Add("Template Not Found");
            }
            else
            {
                int activeThemeId = MixService.GetConfig<int>(
                    MixConstants.ConfigurationKeyword.ThemeId, culture);

                result = Repository.GetSingleModel(t => t.FolderType == temp[0] && t.FileName == temp[1].Split('.')[0] && t.ThemeId == activeThemeId
                    , _context, _transaction);
            }
            return result;
        }

        public static UpdateViewModel GetTemplateByPath(string path, string specificulture, MixEnums.EnumTemplateFolder folderType, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            string templateName = path?.Split('/')[1];
            int themeId = MixService.GetConfig<int>(MixConstants.ConfigurationKeyword.ThemeId, specificulture);
            string themeName = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeName, specificulture);
            var getView = UpdateViewModel.Repository.GetSingleModel(t =>
                    t.ThemeId == themeId && t.FolderType == folderType.ToString()
                    && !string.IsNullOrEmpty(templateName) && templateName.Equals($"{t.FileName}{t.Extension}"), _context, _transaction);
            return getView.Data ?? GetDefault(folderType, specificulture);
        }

        public static UpdateViewModel GetDefault(MixEnums.EnumTemplateFolder folderType, string specificulture)
        {
            string activedTheme = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeName, specificulture)
                    ?? MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultTheme);
            string folder = CommonHelper.GetFullPath(new string[]
                    {
                    MixConstants.Folder.TemplatesFolder
                    , activedTheme
                    , folderType.ToString()
                    });
            var defaulTemplate = MixTemplates.UpdateViewModel.Repository.GetModelListBy(
                t => t.Theme.Name == activedTheme && t.FolderType == folderType.ToString()).Data?.FirstOrDefault();
            return defaulTemplate ?? new UpdateViewModel(new MixTemplate()
            {
                ThemeId = MixService.GetConfig<int>(MixConstants.ConfigurationKeyword.ThemeId, specificulture),
                ThemeName = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeName, specificulture),
                FileName = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultTemplate),
                Extension = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.TemplateExtension),
                Content = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultTemplateContent),
                FolderType = folderType.ToString(),
                FileFolder = folder.ToString()
            });

        }
        #endregion
    }
}
