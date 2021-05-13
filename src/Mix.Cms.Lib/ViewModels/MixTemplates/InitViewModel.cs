using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Heart.Models;
using Mix.Heart.Infrastructure.ViewModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixTemplates
{
    public class InitViewModel
       : ViewModelBase<MixCmsContext, MixTemplate, InitViewModel>
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

        public string TemplatePath {
            get {
                return $"/{FileFolder}/{FileName}{Extension}";
            }
        }

        #endregion Views

        #endregion Properties

        #region Contructors

        public InitViewModel()
            : base()
        {
        }

        public InitViewModel(MixTemplate model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        #region Common

        public override MixTemplate ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
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
    }
}