using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Domain { get { return MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain); } }

        [JsonProperty("imageUrl")]
        public string ImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Image) && (Image.IndexOf("http") == -1) && Image[0] != '/')
                {
                    return $"{Domain}/{Image}";
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
                    return $"{Domain}/{Thumbnail}";
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
                return $"{MixFolders.SiteContentAssetsFolder}/{Name}/assets";
            }
        }

        public string UploadsFolder
        {
            get
            {
                return $"{MixFolders.SiteContentAssetsFolder}/{Name}/uploads";
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
                Id = Repository.Max(m => m.Id, _context, _transaction).Data + 1;
                Name = SeoHelper.GetSEOString(Title);
                CreatedDateTime = DateTime.UtcNow;
                Status = MixContentStatus.Published;
                //Import From existing Theme (zip)
                if (string.IsNullOrEmpty(TemplateAsset.Filename))
                {
                    TemplateAsset = new FileViewModel()
                    {
                        Filename = "_blank",
                        Extension = MixFileExtensions.Zip,
                        FileFolder = MixFolders.DataFolder
                    };
                }
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Templates = MixTemplates.UpdateViewModel.Repository.GetModelListBy(t => t.ThemeId == Id,
                _context: _context, _transaction: _transaction).Data;
            TemplateAsset = new FileViewModel()
            {
                FileFolder = MixFolders.ThemePackage
            };
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
            if (result.IsSucceed && !string.IsNullOrEmpty(Asset?.Filename))
            {
                result = ImportAssetsAsync(_context, _transaction);
            }

            if (result.IsSucceed && !string.IsNullOrEmpty(TemplateAsset.Filename))
            {
                result = await ImportThemeAsync(parent, _context, _transaction);
            }

            // Actived Theme
            if (IsActived)
            {
                result = await Helper.ActivedThemeAsync(Model.Id, Name, Specificulture, _context, _transaction);
            }
            return result;
        }

        #endregion Async

        #endregion Overrides

        #region Expand

        private async Task<RepositoryResponse<bool>> ImportThemeAsync(MixTheme parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            string filePath = $"{TemplateAsset.FileFolder}/{TemplateAsset.Filename}{TemplateAsset.Extension}";
            if (File.Exists(filePath))
            {
                string outputFolder = $"{TemplateAsset.FileFolder}/Extract";
                MixFileRepository.Instance.DeleteFolder(outputFolder);
                MixFileRepository.Instance.CreateDirectoryIfNotExist(outputFolder);
                MixFileRepository.Instance.UnZipFile(filePath, outputFolder);

                //Move Unzip Asset folder
                MixFileRepository.Instance.CopyDirectory($"{outputFolder}/Assets", $"{MixFolders.WebRootPath}/{AssetFolder}");
                //Move Unzip Uploads folder
                MixFileRepository.Instance.CopyDirectory($"{outputFolder}/Uploads", $"{MixFolders.WebRootPath}/{UploadsFolder}");
                // Get SiteStructure
                var strSchema = MixFileRepository.Instance.GetFile("schema.json", $"{outputFolder}/Data");
                string parseContent = strSchema.Content.Replace("[ACCESS_FOLDER]", AssetFolder)
                                                       .Replace("[CULTURE]", Specificulture)
                                                       .Replace("[THEME_NAME]", parent.Name);
                var siteStructures = JObject.Parse(parseContent).ToObject<SiteStructureViewModel>();
                siteStructures.CreatedBy = CreatedBy;
                MixFileRepository.Instance.DeleteFolder(outputFolder);
                MixFileRepository.Instance.DeleteFolder(MixFolders.ThemePackage);
                //Import Site Structures
                result = await siteStructures.ImportAsync(parent.Id, parent.Name, Specificulture, _context, _transaction);
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveTemplatesAsync(MixTheme parent, string themeName, MixCmsContext context, IDbContextTransaction transaction)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>() { IsSucceed = true };
            // Save template files to db
            var files = MixFileRepository.Instance.GetFilesWithContent(TemplateFolder);
            //TODO: Create default asset
            foreach (var file in files)
            {
                string content = file.Content.Replace($"{MixFolders.SiteContentAssetsFolder}/{themeName}/",
                $"{MixFolders.SiteContentAssetsFolder}/{Name}/");
                MixTemplates.UpdateViewModel template = new MixTemplates.UpdateViewModel(
                    new MixTemplate()
                    {
                        FileFolder = file.FileFolder,
                        FileName = file.Filename,
                        Content = content,
                        Extension = file.Extension,
                        CreatedDateTime = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                        ThemeId = parent.Id,
                        ThemeName = parent.Name,
                        FolderType = file.FolderName,
                        ModifiedBy = CreatedBy
                    }, context, transaction);
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

        private RepositoryResponse<bool> ImportAssetsAsync(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>();
            string fullPath = $"{MixFolders.WebRootPath}/{Asset.FileFolder}/{Asset.Filename}{Asset.Extension}";
            if (File.Exists(fullPath))
            {
                MixFileRepository.Instance.UnZipFile(fullPath, $"{MixFolders.WebRootPath}/{Asset.FileFolder}");
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
            var files = MixFileRepository.Instance.GetWebFiles($"{MixFolders.WebRootPath}/{AssetFolder}");
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