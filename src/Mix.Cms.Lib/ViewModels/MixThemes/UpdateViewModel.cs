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
using System.Text;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.ViewModels.MixThemes
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixTheme, UpdateViewModel>
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
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

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
        [JsonProperty("isCloneFromCurrentTheme")]
        public bool IsCloneFromCurrentTheme { get; set; }

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

        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl
        {
            get
            {
                if (Thumbnail != null && Thumbnail.IndexOf("http") == -1 && Thumbnail[0] != '/')
                {
                    return CommonHelper.GetFullPath(new string[] {
                    Domain,  Thumbnail
                });
                }
                else
                {
                    return string.IsNullOrEmpty(Thumbnail) ? ImageUrl : Thumbnail;
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

        public List<MixTemplates.UpdateViewModel> Templates { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel()
            : base()
        {
        }

        public UpdateViewModel(MixTheme model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixTheme ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(m => m.Id).Data + 1;
                Name = SeoHelper.GetSEOString(Title);
                CreatedDateTime = DateTime.UtcNow;
                //Import From existing Theme (zip)
                if (string.IsNullOrEmpty(TemplateAsset.Filename))
                {
                    TemplateAsset = new Lib.ViewModels.FileViewModel()
                    {
                        Filename = "default_blank",
                        Extension = ".zip",
                        FileFolder = "Imports/Themes"
                    };
                }
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Templates = MixTemplates.UpdateViewModel.Repository.GetModelListBy(t => t.ThemeId == Id,
                _context: _context, _transaction: _transaction).Data;
            TemplateAsset = new FileViewModel() { FileFolder = $"import/themes/{DateTime.UtcNow.ToShortDateString()}/{Name}" };
            Asset = new FileViewModel() { FileFolder = AssetFolder };
        }

        #region Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixTheme parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>() { IsSucceed = true };

            if (IsCloneFromCurrentTheme)
            {
                if (result.IsSucceed)
                {
                    RepositoryResponse<bool> saveTemplate = await SaveTemplatesAsync(parent, TemplateFolder, _context, _transaction);
                    ViewModelHelper.HandleResult(saveTemplate, ref result);
                }
            }
            // Import Assets
            if (result.IsSucceed && !string.IsNullOrEmpty(Asset.Filename))
            {
                result = ImportAssetsAsync(_context, _transaction);
            }

            if (result.IsSucceed && !string.IsNullOrEmpty(TemplateAsset.Filename))
            {
                result = await ImportThemeAsync(parent, _context, _transaction);
            }



            //// New themes without import existing theme => create from default folder
            //if (result.IsSucceed && !Directory.Exists(TemplateFolder) && string.IsNullOrEmpty(TemplateAsset.Filename))
            //{
            //    result = await CreateDefaultThemeTemplatesAsync(_context, _transaction);
            //}

            // Actived Theme
            if (IsActived)
            {
                result = await ActivedThemeAsync(_context, _transaction);
            }
            return result;
        }

        #endregion Async

        #region Sync


        #endregion Sync

        #endregion Overrides

        #region Expand

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
                    RepositoryResponse<bool> saveTemplate = await SaveTemplatesAsync(parent, TemplateFolder, _context, _transaction);
                    ViewModelHelper.HandleResult(saveTemplate, ref result);
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveTemplatesAsync(MixTheme parent, string themeName, MixCmsContext context, IDbContextTransaction transaction)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>();
            // Save template files to db
            var files = FileRepository.Instance.GetFilesWithContent(TemplateFolder);
            //TODO: Create default asset
            foreach (var file in files)
            {
                Enum.TryParse(file.FolderName, out MixTemplateFolderType folderType);
                string content = file.Content.Replace($"Content/Templates/{themeName}/",
                $"Content/Templates/{Name}/");
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
                var saveResult = await template.SaveModelAsync(true, context, transaction);
                result.IsSucceed = result.IsSucceed && saveResult.IsSucceed;
                if (!saveResult.IsSucceed)
                {
                    result.IsSucceed = false;
                    result.Exception = saveResult.Exception;
                    result.Errors.AddRange(saveResult.Errors);
                    break;
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

            //CommonHelper.GetFullPath(new string[] {
            //    MixFolders.TemplatesFolder,
            //    MixService.GetConfig<string>(AppSettingKeywords.DefaultTemplateFolder) });
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

        private RepositoryResponse<bool> ImportAssetsAsync(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>();
            string fullPath = $"wwwroot/{Asset.FileFolder}/{Asset.Filename}{Asset.Extension}";
            if (File.Exists(fullPath))
            {
                FileRepository.Instance.UnZipFile(fullPath, $"wwwroot/{Asset.FileFolder}");
                //InitAssetStyle();
                result.IsSucceed = true;
            }
            else
            {
                result.Errors.Add("Cannot saved asset file");
            }

            return result;
        }

        private async Task InitAssetStyleAsync(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var files = FileRepository.Instance.GetWebFiles($"wwwroot/{AssetFolder}");
            StringBuilder strStyles = new StringBuilder();

            foreach (var css in files.Where(f => f.Extension == ".css"))
            {
                strStyles.Append($"   <link href='{css.FileFolder}/{css.Filename}{css.Extension}' rel='stylesheet'/>");
            }
            StringBuilder strScripts = new StringBuilder();
            foreach (var js in files.Where(f => f.Extension == ".js"))
            {
                strScripts.Append($"  <script src='{js.FileFolder}/{js.Filename}{js.Extension}'></script>");
            }
            var layout = MixTemplates.InitViewModel.Repository.GetSingleModel(
                t => t.FileName == "_Layout" && t.ThemeId == Model.Id
                , _context, _transaction);
            layout.Data.Content = layout.Data.Content.Replace("<!--[STYLES]-->"
                , string.Format(@"{0}"
                , strStyles));
            layout.Data.Content = layout.Data.Content.Replace("<!--[SCRIPTS]-->"
                , string.Format(@"{0}"
                , strScripts));

            await layout.Data.SaveModelAsync(true, _context, _transaction);
        }

        #endregion Expand
    }
}