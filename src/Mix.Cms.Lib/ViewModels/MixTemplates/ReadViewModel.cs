using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mix.Cms.Lib.ViewModels.MixTemplates
{
    public class ReadViewModel
       : ViewModelBase<MixCmsContext, MixTemplate, ReadViewModel>
    {
        #region Properties

        #region Models

        [JsonIgnore]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonIgnore]
        [JsonProperty("templateId")]
        public int TemplateId { get; set; }

        [JsonIgnore]
        [JsonProperty("themeName")]
        public string ThemeName { get; set; }

        [JsonIgnore]
        [JsonProperty("folderType")]
        public string FolderType { get; set; }

        [JsonIgnore]
        [JsonProperty("fileFolder")]
        public string FileFolder { get; set; }

        [JsonIgnore]
        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonIgnore]
        [JsonProperty("extension")]
        public string Extension { get; set; }

        [JsonIgnore]
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonIgnore]
        [JsonProperty("mobileContent")]
        public string MobileContent { get; set; }

        [JsonProperty("spaContent")]
        public string SpaContent { get; set; }

        [JsonProperty("spaView")]
        public XElement SpaView {
            get {
                return !string.IsNullOrEmpty(SpaContent)
                    ? XElement.Parse(Regex.Replace(SpaContent, "(?<!\r)\n|\r\n|\t", "").Trim())
                    : new XElement("div");
            }
        }

        [JsonProperty("scripts")]
        public string Scripts { get; set; }

        [JsonProperty("styles")]
        public string Styles { get; set; }

        [JsonIgnore]
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
        public string AssetFolder {
            get {
                return CommonHelper.GetFullPath(new string[] {
                    MixConstants.Folder.FileFolder,
                    MixConstants.Folder.TemplatesAssetFolder,
                    ThemeName });
            }
        }

        [JsonIgnore]
        [JsonProperty("templateFolder")]
        public string TemplateFolder {
            get {
                return CommonHelper.GetFullPath(new string[] {
                    MixConstants.Folder.TemplatesFolder,
                    ThemeName
                    });
            }
        }

        [JsonProperty("templatePath")]
        public string TemplatePath {
            get {
                return $"/{FileFolder}/{FileName}{Extension}";
            }
        }

        //TO DO Ref swastika core MixTemplateViewModel for spa view

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadViewModel()
            : base()
        {
        }

        public ReadViewModel(MixTemplate model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
        public static RepositoryResponse<ReadViewModel> GetTemplateByPath(string path, string culture
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<ReadViewModel> result = new RepositoryResponse<ReadViewModel>();
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
                string name = temp[1].Split('.')[0];
                result = Repository.GetSingleModel(t => t.FolderType == temp[0] && t.FileName == name && t.ThemeId == activeThemeId
                    , _context, _transaction);
            }
            return result;
        }

        /// <summary>
        /// Gets the template by path.
        /// </summary>
        /// <param name="path">The path.</param> Ex: "Pages/_Home"
        /// <returns></returns>
        public static async Task<RepositoryResponse<ReadViewModel>> GetTemplateByPathAsync(string path, string culture
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<ReadViewModel> result = new RepositoryResponse<ReadViewModel>();
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
                string name = temp[1].Split('.')[0];
                result = await Repository.GetSingleModelAsync(t => t.FolderType == temp[0] && t.FileName == name && t.ThemeId == activeThemeId
                    , _context, _transaction);
            }
            return result;
        }

        public static ReadViewModel GetTemplateByPath(int themeId, string path, string type, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            string templateName = path?.Split('/')[1];
            var getView = ReadViewModel.Repository.GetSingleModel(t =>
                    t.ThemeId == themeId && t.FolderType == type
                    && !string.IsNullOrEmpty(templateName) && templateName.Equals($"{t.FileName}{t.Extension}"), _context, _transaction);
            return getView.Data;
        }

        public static ReadViewModel GetDefault(string activedTemplate, string folderType, string folder, string specificulture)
        {
            return new ReadViewModel(new MixTemplate()
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