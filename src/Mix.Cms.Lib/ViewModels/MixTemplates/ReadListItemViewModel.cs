using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixTemplates
{
    public class ReadListItemViewModel
       : ViewModelBase<MixCmsContext, MixTemplate, ReadListItemViewModel>
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

        [JsonProperty("assetFolder")]
        public string AssetFolder
        {
            get
            {
                return $"{MixFolders.SiteContentAssetsFolder}/{ThemeName}";
            }
        }

        [JsonProperty("templateFolder")]
        public string TemplateFolder
        {
            get
            {
                return $"{MixFolders.TemplatesFolder}/{ThemeName}";
            }
        }

        [JsonProperty("templatePath")]
        public string TemplatePath
        {
            get
            {
                return $"/{FileFolder}/{FileName}{Extension}";
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
            if (!string.IsNullOrEmpty(path))
            {
                string[] temp = path.Split('/');
                if (temp.Length < 2)
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
                Extension = MixService.GetAppSetting<string>("TemplateExtension"),
                ThemeId = MixService.GetConfig<int>(MixAppSettingKeywords.ThemeId, specificulture),
                ThemeName = activedTemplate,
                FolderType = folderType,
                FileFolder = folder,
                FileName = MixService.GetAppSetting<string>("DefaultTemplate"),
                Content = "<div></div>"
            });
        }

        #endregion Expands
    }
}