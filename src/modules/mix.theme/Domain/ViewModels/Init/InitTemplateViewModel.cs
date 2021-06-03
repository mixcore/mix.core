using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Models;
using Mix.Heart.Infrastructure.ViewModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Mix.Lib.Entities.Cms;
using Mix.Shared.Enums;
using Mix.Shared.Constants;
using Mix.Lib.Repositories;
using Mix.Lib.Models.Common;

namespace Mix.Theme.Domain.ViewModels.Init
{
    public class InitTemplateViewModel
       : ViewModelBase<MixCmsContext, MixTemplate, InitTemplateViewModel>
    {
        #region Properties

        #region Models

        public int Id { get; set; }

        public int ThemeId { get; set; }

        public string ThemeName { get; set; }

        public string FolderType { get; set; }

        public string FileFolder { get; set; }

        public string FileName { get; set; }

        public string Extension { get; set; }

        [Required]
        public string Content { get; set; }

        public string MobileContent { get; set; } = "{}";

        public string SpaContent { get; set; } = "";

        public string Scripts { get; set; }

        public string Styles { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public int Priority { get; set; }

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

        public InitTemplateViewModel()
            : base()
        {
        }

        public InitTemplateViewModel(MixTemplate model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
            TemplateRepository.Instance.SaveTemplate(new TemplateModel()
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
            TemplateRepository.Instance.SaveTemplate(new TemplateModel()
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