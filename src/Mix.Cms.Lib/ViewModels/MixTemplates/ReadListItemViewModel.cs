using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;


namespace Mix.Cms.Lib.ViewModels.MixTemplates
{
    public class ReadListItemViewModel
       : ViewModelBase<MixCmsContext, MixTemplate, ReadListItemViewModel>
    {
        #region Properties

        #region Models

        [JsonIgnore]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonIgnore]
        [JsonProperty("templateId")]
        public int TemplateId { get; set; }

        [JsonProperty("templateName")]
        public string TemplateName { get; set; }

        [JsonProperty("folderType")]
        public string FolderType { get; set; }

        [JsonProperty("fileFolder")]
        public string FileFolder { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("extension")]
        public string Extension { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonIgnore]
        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonIgnore]
        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        #endregion Models

        #region Views

        [JsonIgnore]
        [JsonProperty("assetFolder")]
        public string AssetFolder
        {
            get
            {
                return CommonHelper.GetFullPath(new string[] {
                    MixConstants.Folder.FileFolder,
                    MixConstants.Folder.TemplatesAssetFolder,
                    TemplateName });
            }
        }

        [JsonIgnore]
        [JsonProperty("templateFolder")]
        public string TemplateFolder
        {
            get
            {
                return CommonHelper.GetFullPath(new string[] { MixConstants.Folder.TemplatesFolder, TemplateName });
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

        //TO DO Ref swastika core MixTemplateViewModel for spa view

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadListItemViewModel()
            : base()
        {
        }

        public ReadListItemViewModel(MixTemplate model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Expands

        /// <summary>
        /// Gets the template by path.
        /// </summary>
        /// <param name="path">The path.</param> Ex: "Pages/_Home"
        /// <returns></returns>
        public static RepositoryResponse<ReadListItemViewModel> GetTemplateByPath(string path, string culture
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<ReadListItemViewModel> result = new RepositoryResponse<ReadListItemViewModel>();
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

        public static ReadListItemViewModel GetTemplateByPath(int themeId, string path, string type, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            string templateName = path?.Split('/')[1];
            var getView = ReadListItemViewModel.Repository.GetSingleModel(t =>
                    t.ThemeId == themeId && t.FolderType == type
                    && !string.IsNullOrEmpty(templateName) && templateName.Equals($"{t.FileName}{t.Extension}"), _context, _transaction);
            return getView.Data;
        }

        public static ReadListItemViewModel GetDefault(string activedTemplate, string folderType, string folder, string specificulture)
        {
            return new ReadListItemViewModel(new MixTemplate()
            {
                Extension = MixService.GetConfig<string>("TemplateExtension"),
                ThemeId = MixService.GetConfig<int>(MixConstants.ConfigurationKeyword.ThemeId, specificulture),
                ThemeName = activedTemplate,
                FolderType = folderType,
                FileFolder = folder,
                FileName = MixService.GetConfig<string>("DefaultTemplate"),
                Content = "<div></div>"
            });

        }
        #endregion Expands

    }
}
