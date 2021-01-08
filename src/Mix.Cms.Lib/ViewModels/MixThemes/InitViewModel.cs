﻿using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixConfigurations;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.ViewModels.MixThemes
{
    public class InitViewModel
      : ViewModelBase<MixCmsContext, MixTheme, InitViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

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
        [JsonProperty("isCreateDefault")]
        public bool IsCreateDefault { get; set; }
        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain"); } }

        [JsonProperty("imageUrl")]
        public string ImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Image) && (Image.IndexOf("http") == -1) && Image[0] != '/')
                {
                    return CommonHelper.GetFullPath(new string[] {
                    Domain,  Image
                });
                }
                else
                {
                    return Image;
                }
            }
        }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("templateAsset")]
        public FileViewModel TemplateAsset { get; set; }

        [JsonProperty("asset")]
        public FileViewModel Asset { get; set; }

        [JsonProperty("assetFolder")]
        public string AssetFolder
        {
            get
            {
                return $"content/templates/{Name}/assets";
            }
        }

        public string UploadsFolder
        {
            get
            {
                return $"content/templates/{Name}/uploads";
            }
        }

        [JsonProperty("templateFolder")]
        public string TemplateFolder
        {
            get
            {
                return $"{MixFolders.TemplatesFolder}/{Name}";
            }
        }

        public List<MixTemplates.InitViewModel> Templates { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public InitViewModel()
            : base()
        {
        }

        public InitViewModel(MixTheme model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixTheme ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Id = 1;
            Name = SeoHelper.GetSEOString(Title);
            CreatedDateTime = DateTime.UtcNow;
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Templates = MixTemplates.InitViewModel.Repository.GetModelListBy(t => t.ThemeId == Id,
                _context: _context, _transaction: _transaction).Data;
            TemplateAsset = new FileViewModel() { FileFolder = $"Import/Themes/{DateTime.UtcNow.ToShortDateString()}/{Name}" };
            Asset = new FileViewModel() { FileFolder = $"wwwroot/{AssetFolder}" };
        }

        #region Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixTheme parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>() { IsSucceed = true };

            if (string.IsNullOrEmpty(TemplateAsset.Filename))
            {
                TemplateAsset = new Lib.ViewModels.FileViewModel()
                {
                    Filename = "default_blank",
                    Extension = ".zip",
                    FileFolder = "Imports/Themes"
                };
            }

            result = await ImportThemeAsync(parent, _context, _transaction);

            // Actived Theme
            if (IsActived)
            {
                result = await ActivedThemeAsync(_context, _transaction);
            }

            return result;
        }

        private async Task<RepositoryResponse<bool>> ImportThemeAsync(MixTheme parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            string filePath = $"wwwroot/{TemplateAsset.FileFolder}/{TemplateAsset.Filename}{TemplateAsset.Extension}";
            if (File.Exists(filePath))
            {
                string outputFolder = $"wwwroot/{TemplateAsset.FileFolder}/Extract";
                FileRepository.Instance.DeleteFolder(outputFolder);
                FileRepository.Instance.CreateDirectoryIfNotExist(outputFolder);
                FileRepository.Instance.UnZipFile(filePath, outputFolder);
                //Move Unzip Asset folder
                FileRepository.Instance.CopyDirectory($"{outputFolder}/Assets", $"wwwroot/{AssetFolder}");
                //Move Unzip Templates folder
                FileRepository.Instance.CopyDirectory($"{outputFolder}/Templates", TemplateFolder);
                //Move Unzip Uploads folder
                FileRepository.Instance.CopyDirectory($"{outputFolder}/Uploads", $"wwwroot/{UploadsFolder}");
                // Get SiteStructure
                var strSchema = FileRepository.Instance.GetFile("schema.json", $"{outputFolder}/Data");
                string parseContent = strSchema.Content.Replace("[ACCESS_FOLDER]", AssetFolder)
                                                       .Replace("[CULTURE]", Specificulture);
                var siteStructures = JObject.Parse(parseContent).ToObject<SiteStructureViewModel>();
                FileRepository.Instance.DeleteFolder(outputFolder);
                //FileRepository.Instance.DeleteFile(filePath);
                //Import Site Structures
                result = await siteStructures.ImportAsync(Specificulture);
                if (result.IsSucceed)
                {
                    // Save template files to db
                    var files = FileRepository.Instance.GetFilesWithContent(TemplateFolder);
                    //TODO: Create default asset
                    foreach (var file in files)
                    {
                        Enum.TryParse(file.FolderName, out MixTemplateFolderType folderType);
                        string content = file.Content.Replace($"/Content/Templates/{siteStructures.ThemeName}/",
                        $"/Content/Templates/{Name}/");
                        MixTemplates.UpdateViewModel template = new MixTemplates.UpdateViewModel(
                            new MixTemplate()
                            {
                                FileFolder = file.FileFolder,
                                FileName = file.Filename,
                                Content = file.Content,
                                Extension = file.Extension,
                                CreatedDateTime = DateTime.UtcNow,
                                LastModified = DateTime.UtcNow,
                                ThemeId = parent.Id,
                                ThemeName = parent.Name,
                                FolderType = folderType,
                                ModifiedBy = CreatedBy
                            });
                        var saveResult = await template.SaveModelAsync(true, _context, _transaction);
                        result.IsSucceed = result.IsSucceed && saveResult.IsSucceed;
                        if (!saveResult.IsSucceed)
                        {
                            result.IsSucceed = false;
                            result.Exception = saveResult.Exception;
                            result.Errors.AddRange(saveResult.Errors);
                            break;
                        }
                    }
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> ActivedThemeAsync(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            SystemConfigurationViewModel config = (await SystemConfigurationViewModel.Repository.GetSingleModelAsync(
                    c => c.Keyword == AppSettingKeywords.ThemeName && c.Specificulture == Specificulture
                    , _context, _transaction)).Data;
            if (config == null)
            {
                config = new SystemConfigurationViewModel()
                {
                    Keyword = AppSettingKeywords.ThemeName,
                    Specificulture = Specificulture,
                    Category = "Site",
                    DataType = MixDataType.Text,
                    Description = "Cms Theme",
                    Value = Name
                };
            }
            else
            {
                config.Value = Name;
            }
            var saveConfigResult = await config.SaveModelAsync(false, _context, _transaction);
            if (saveConfigResult.IsSucceed)
            {
                SystemConfigurationViewModel configFolder = (await SystemConfigurationViewModel.Repository.GetSingleModelAsync(
                c => c.Keyword == AppSettingKeywords.ThemeFolder && c.Specificulture == Specificulture
                , _context, _transaction)).Data;
                configFolder.Value = Name;

                saveConfigResult = await configFolder.SaveModelAsync(false, _context, _transaction);
            }

            ViewModelHelper.HandleResult(saveConfigResult, ref result);

            if (result.IsSucceed)
            {
                SystemConfigurationViewModel configId = (await SystemConfigurationViewModel.Repository.GetSingleModelAsync(
                      c => c.Keyword == AppSettingKeywords.ThemeId && c.Specificulture == Specificulture, _context, _transaction)).Data;
                if (configId == null)
                {
                    configId = new SystemConfigurationViewModel()
                    {
                        Keyword = AppSettingKeywords.ThemeId,
                        Specificulture = Specificulture,
                        Category = "Site",
                        DataType = MixDataType.Text,
                        Description = "Cms Theme Id",
                        Value = Model.Id.ToString()
                    };
                }
                else
                {
                    configId.Value = Model.Id.ToString();
                }
                var saveResult = await configId.SaveModelAsync(false, _context, _transaction);
                ViewModelHelper.HandleResult(saveResult, ref result);
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> CreateDefaultThemeTemplatesAsync(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            string defaultFolder = $"{MixService.GetConfig<string>(AppSettingKeywords.DefaultBlankTemplateFolder) }";

            bool copyResult = FileRepository.Instance.CopyDirectory(defaultFolder, TemplateFolder);

            var files = FileRepository.Instance.GetFilesWithContent(TemplateFolder);
            var id = _context.MixTemplate.Count() + 1;
            //TODO: Create default asset
            foreach (var file in files)
            {
                Enum.TryParse(file.FolderName, out MixTemplateFolderType folderType);
                MixTemplates.InitViewModel template = new MixTemplates.InitViewModel(
                    new MixTemplate()
                    {
                        Id = id,
                        FileFolder = file.FileFolder,
                        FileName = file.Filename,
                        Content = file.Content,
                        Extension = file.Extension,
                        CreatedDateTime = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                        ThemeId = Model.Id,
                        ThemeName = Model.Name,
                        FolderType = folderType,
                        ModifiedBy = CreatedBy
                    }, _context, _transaction);
                var saveResult = await template.SaveModelAsync(true, _context, _transaction);
                ViewModelHelper.HandleResult(saveResult, ref result);
                if (!result.IsSucceed)
                {
                    break;
                }
                else
                {
                    id += 1;
                }
            }
            return result;
        }

        #endregion Async

        #endregion Overrides
    }
}