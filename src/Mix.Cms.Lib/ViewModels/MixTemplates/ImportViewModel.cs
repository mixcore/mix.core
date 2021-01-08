﻿using Microsoft.EntityFrameworkCore.Storage;
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
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;

namespace Mix.Cms.Lib.ViewModels.MixTemplates
{
    public class ImportViewModel
      : ViewModelBase<MixCmsContext, MixTemplate, ImportViewModel>
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
        public MixTemplateFolderType FolderType { get; set; }

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
        public string AssetFolder
        {
            get
            {
                return CommonHelper.GetFullPath(new string[] {
                    MixFolders.FileFolder,
                    MixFolders.TemplatesAssetFolder,
                     SeoHelper.GetSEOString(ThemeName) });
            }
        }

        [JsonProperty("templateFolder")]
        public string TemplateFolder
        {
            get
            {
                return CommonHelper.GetFullPath(new string[] {
                    MixFolders.TemplatesFolder, SeoHelper.GetSEOString(ThemeName) });
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

        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportViewModel()
            : base()
        {
        }

        public ImportViewModel(MixTemplate model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
                var file = FileRepository.Instance.GetFile(FileName, Extension, FileFolder);
                if (!string.IsNullOrWhiteSpace(file?.Content))
                {
                    Content = file.Content;
                }
            }

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

        public override async Task<RepositoryResponse<ImportViewModel>> SaveModelAsync(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
        public static RepositoryResponse<ImportViewModel> GetTemplateByPath(string path, string culture
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<ImportViewModel> result = new RepositoryResponse<ImportViewModel>();
            string[] temp = path?.Split('/');
            if (temp == null || temp.Length < 2)
            {
                result.IsSucceed = false;
                result.Errors.Add("Template Not Found");
            }
            else
            {
                Enum.TryParse(temp[0], out MixTemplateFolderType folderType);
                int activeThemeId = MixService.GetConfig<int>(
                    AppSettingKeywords.ThemeId, culture);
                string name = temp[1].Split('.')[0];
                result = Repository.GetSingleModel(t => t.FolderType == folderType && t.FileName == name && t.ThemeId == activeThemeId
                    , _context, _transaction);
            }
            return result;
        }

        public static ImportViewModel GetTemplateByPath(string path, string specificulture, MixTemplateFolderType folderType, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            string templateName = path?.Split('/')[1];
            int themeId = MixService.GetConfig<int>(AppSettingKeywords.ThemeId, specificulture);
            string themeName = MixService.GetConfig<string>(AppSettingKeywords.ThemeName, specificulture);
            var getView = ImportViewModel.Repository.GetSingleModel(t =>
                    t.ThemeId == themeId && t.FolderType == folderType
                    && !string.IsNullOrEmpty(templateName) && templateName.Equals($"{t.FileName}{t.Extension}"), _context, _transaction);
            return getView.Data ?? GetDefault(folderType, specificulture);
        }

        public static ImportViewModel GetDefault(MixTemplateFolderType folderType, string specificulture)
        {
            string activedTheme = MixService.GetConfig<string>(AppSettingKeywords.ThemeFolder, specificulture)
                    ?? MixService.GetConfig<string>(AppSettingKeywords.DefaultTheme);
            string folder = CommonHelper.GetFullPath(new string[]
                    {
                    MixFolders.TemplatesFolder
                    , activedTheme
                    , folderType.ToString()
                    });
            var defaulTemplate = MixTemplates.ImportViewModel.Repository.GetModelListBy(
                t => t.Theme.Name == activedTheme && t.FolderType == folderType)
                .Data?.FirstOrDefault();
            return defaulTemplate ?? new ImportViewModel(new MixTemplate()
            {
                ThemeId = MixService.GetConfig<int>(AppSettingKeywords.ThemeId, specificulture),
                ThemeName = MixService.GetConfig<string>(AppSettingKeywords.ThemeFolder, specificulture),
                FileName = MixService.GetConfig<string>(AppSettingKeywords.DefaultTemplate),
                Extension = MixService.GetConfig<string>(AppSettingKeywords.TemplateExtension),
                Content = MixService.GetConfig<string>(AppSettingKeywords.DefaultTemplateContent),
                FolderType = folderType,
                FileFolder = folder.ToString()
            });
        }

        public async Task<RepositoryResponse<ImportViewModel>> CopyAsync()
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