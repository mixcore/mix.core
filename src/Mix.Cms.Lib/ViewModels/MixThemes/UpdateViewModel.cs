using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixSystem;
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
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixThemes
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixTheme, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

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

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain"); } }

        [JsonProperty("imageUrl")]
        public string ImageUrl {
            get {
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
        public string ThumbnailUrl {
            get {
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
        public string AssetFolder {
            get {
                return $"content/templates/{Name}/assets";
            }
        }

        public string UploadsFolder {
            get {
                return $"content/templates/{Name}/uploads";
            }
        }

        [JsonProperty("templateFolder")]
        public string TemplateFolder {
            get {
                return $"{MixConstants.Folder.TemplatesFolder}/{Name}";
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
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Templates = MixTemplates.UpdateViewModel.Repository.GetModelListBy(t => t.ThemeId == Id,
                _context: _context, _transaction: _transaction).Data;
            TemplateAsset = new FileViewModel() { FileFolder = $"Import/Themes/{DateTime.UtcNow.ToShortDateString()}/{Name}" };
            Asset = new FileViewModel() { FileFolder = AssetFolder };
        }

        #region Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixTheme parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>() { IsSucceed = true };

            //Import From existing Theme (zip)
            if (!string.IsNullOrEmpty(TemplateAsset.Filename))
            {
                result = await ImportThemeAsync(parent, _context, _transaction);
            }

            // Import Assets
            if (result.IsSucceed && !string.IsNullOrEmpty(Asset.Filename))
            {
                result = ImportAssetsAsync(_context, _transaction);
            }

            // New themes without import existing theme => create from default folder
            if (result.IsSucceed && !Directory.Exists(TemplateFolder) && string.IsNullOrEmpty(TemplateAsset.Filename))
            {
                result = await CreateDefaultThemeTemplatesAsync(_context, _transaction);
            }

            // Actived Theme
            if (IsActived)
            {
                result = await ActivedThemeAsync(_context, _transaction);
            }
            return result;
        }

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = await MixTemplates.InitViewModel.Repository.RemoveListModelAsync(false, t => t.ThemeId == Id);
            if (result.IsSucceed)
            {
                FileRepository.Instance.DeleteWebFolder(AssetFolder);
                FileRepository.Instance.DeleteFolder(TemplateFolder);
            }
            return new RepositoryResponse<bool>()
            {
                IsSucceed = result.IsSucceed,
                Errors = result.Errors,
                Exception = result.Exception
            };
        }

        #endregion Async

        #region Sync

        public override RepositoryResponse<bool> RemoveRelatedModels(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = MixTemplates.InitViewModel.Repository.RemoveListModel(false, t => t.ThemeId == Id);
            if (result.IsSucceed)
            {
                FileRepository.Instance.DeleteWebFolder(AssetFolder);
                FileRepository.Instance.DeleteFolder(TemplateFolder);
            }
            return new RepositoryResponse<bool>()
            {
                IsSucceed = result.IsSucceed,
                Errors = result.Errors,
                Exception = result.Exception
            };
        }

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
                var siteStructures = JObject.Parse(strSchema.Content).ToObject<SiteStructureViewModel>();
                FileRepository.Instance.DeleteFolder(outputFolder);
                FileRepository.Instance.DeleteFile(filePath);
                //Import Site Structures
                result = await siteStructures.ImportAsync(Specificulture);
                if (result.IsSucceed)
                {
                    // Save template files to db
                    var files = FileRepository.Instance.GetFilesWithContent(TemplateFolder);
                    //TODO: Create default asset
                    foreach (var file in files)
                    {
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
                                FolderType = file.FolderName,
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
                    c => c.Keyword == MixConstants.ConfigurationKeyword.ThemeName && c.Specificulture == Specificulture
                    , _context, _transaction)).Data;
            if (config == null)
            {
                config = new SystemConfigurationViewModel()
                {
                    Keyword = MixConstants.ConfigurationKeyword.ThemeName,
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
                c => c.Keyword == MixConstants.ConfigurationKeyword.ThemeFolder && c.Specificulture == Specificulture
                , _context, _transaction)).Data;
                configFolder.Value = Name;

                saveConfigResult = await configFolder.SaveModelAsync(false, _context, _transaction);
            }

            ViewModelHelper.HandleResult(saveConfigResult, ref result);

            if (result.IsSucceed)
            {
                SystemConfigurationViewModel configId = (await SystemConfigurationViewModel.Repository.GetSingleModelAsync(
                      c => c.Keyword == MixConstants.ConfigurationKeyword.ThemeId && c.Specificulture == Specificulture, _context, _transaction)).Data;
                if (configId == null)
                {
                    configId = new SystemConfigurationViewModel()
                    {
                        Keyword = MixConstants.ConfigurationKeyword.ThemeId,
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
            string defaultFolder = $"{MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultBlankTemplateFolder) }";

            //CommonHelper.GetFullPath(new string[] {
            //    MixConstants.Folder.TemplatesFolder,
            //    MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultTemplateFolder) });
            bool copyResult = FileRepository.Instance.CopyDirectory(defaultFolder, TemplateFolder);

            var files = FileRepository.Instance.GetFilesWithContent(TemplateFolder);
            var id = _context.MixTemplate.Count() + 1;
            //TODO: Create default asset
            foreach (var file in files)
            {
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
                        FolderType = file.FolderName,
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
            string fullPath = $"{MixConstants.Folder.WebRootPath}/{Asset.FileFolder}/{Asset.Filename}{Asset.Extension}";
            if (File.Exists(fullPath))
            {
                FileRepository.Instance.UnZipFile(fullPath, Asset.FileFolder);
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
            var files = FileRepository.Instance.GetWebFiles(AssetFolder);
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