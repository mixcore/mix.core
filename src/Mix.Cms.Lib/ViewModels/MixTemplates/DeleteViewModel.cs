using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
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
    public class DeleteViewModel
       : ViewModelBase<MixCmsContext, MixTemplate, DeleteViewModel>
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

        public DeleteViewModel()
            : base()
        {
        }

        public DeleteViewModel(MixTemplate model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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

        #endregion Async

        #endregion Overrides
    }
}