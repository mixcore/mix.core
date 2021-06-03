using Microsoft.EntityFrameworkCore.Storage;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using Mix.Shared.Constants;
using Mix.Lib.Entities.Cms;
using Mix.Shared.Enums;
using Mix.Lib.Services;
using Mix.Lib.ViewModels.Cms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Theme.Domain.ViewModels.Init
{
    public class InitThemeViewModel
      : ViewModelBase<MixCmsContext, MixTheme, InitThemeViewModel>
    {
        #region Properties

        #region Models

        public int Id { get; set; }

        public string Specificulture { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public int Priority { get; set; }

        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        public bool IsCreateDefault { get; set; }

        public string Domain { get { return MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.Domain); } }

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

        public bool IsActived { get; set; }

        public FileViewModel TemplateAsset { get; set; }

        public FileViewModel Asset { get; set; }

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

        public string TemplateFolder
        {
            get
            {
                return $"{MixFolders.TemplatesFolder}/{Name}";
            }
        }

        public List<InitTemplateViewModel> Templates { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public InitThemeViewModel()
            : base()
        {
        }

        public InitThemeViewModel(MixTheme model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
            Templates = InitTemplateViewModel.Repository.GetModelListBy(t => t.ThemeId == Id,
                _context: _context, _transaction: _transaction).Data;
            TemplateAsset = new FileViewModel() { FileFolder = $"{MixFolders.ImportFolder}/{DateTime.UtcNow.ToShortDateString()}/{Name}" };
            Asset = new FileViewModel() { FileFolder = $"{MixFolders.WebRootPath}/{AssetFolder}" };
        }

        #region Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixTheme parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>() { IsSucceed = true };

            if (string.IsNullOrEmpty(TemplateAsset.Filename))
            {
                TemplateAsset = new FileViewModel()
                {
                    Filename = "default_blank",
                    Extension = MixFileExtensions.Zip,
                    FileFolder = MixFolders.ImportFolder
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
            string filePath = $"{MixFolders.WebRootPath}/{TemplateAsset.FileFolder}/{TemplateAsset.Filename}{TemplateAsset.Extension}";
            if (File.Exists(filePath))
            {
                string outputFolder = $"{MixFolders.WebRootPath}/{TemplateAsset.FileFolder}/Extract";
                MixFileRepository.Instance.DeleteFolder(outputFolder);
                MixFileRepository.Instance.CreateDirectoryIfNotExist(outputFolder);
                MixFileRepository.Instance.UnZipFile(filePath, outputFolder);
                //Move Unzip Asset folder
                MixFileRepository.Instance.CopyDirectory($"{outputFolder}/Assets", $"{MixFolders.WebRootPath}/{AssetFolder}");
                //Move Unzip Templates folder
                MixFileRepository.Instance.CopyDirectory($"{outputFolder}/Templates", TemplateFolder);
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
                //MixFileRepository.Instance.DeleteFile(filePath);
                //Import Site Structures
                result = await siteStructures.ImportAsync(Specificulture, _context, _transaction);
                if (result.IsSucceed)
                {
                    // Save template files to db
                    var files = MixFileRepository.Instance.GetFilesWithContent(TemplateFolder);
                    //TODO: Create default asset
                    foreach (var file in files)
                    {
                        var template = new InitTemplateViewModel(
                            new MixTemplate()
                            {
                                CreatedBy = CreatedBy,
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
                            }, _context, _transaction);
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
            MixConfigurationViewModel config = (await MixConfigurationViewModel.Repository.GetSingleModelAsync(
                    c => c.Keyword == MixAppSettingKeywords.ThemeName && c.Specificulture == Specificulture
                    , _context, _transaction)).Data;
            if (config == null)
            {
                config = new MixConfigurationViewModel()
                {
                    Keyword = MixAppSettingKeywords.ThemeName,
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
                MixConfigurationViewModel configFolder = (await MixConfigurationViewModel.Repository.GetSingleModelAsync(
                c => c.Keyword == MixAppSettingKeywords.ThemeFolder && c.Specificulture == Specificulture
                , _context, _transaction)).Data;
                configFolder.Value = Name;

                saveConfigResult = await configFolder.SaveModelAsync(false, _context, _transaction);
            }

            ViewModelHelper.HandleResult(saveConfigResult, ref result);

            if (result.IsSucceed)
            {
                MixConfigurationViewModel configId = (await MixConfigurationViewModel.Repository.GetSingleModelAsync(
                      c => c.Keyword == MixAppSettingKeywords.ThemeId && c.Specificulture == Specificulture, _context, _transaction)).Data;
                if (configId == null)
                {
                    configId = new MixConfigurationViewModel()
                    {
                        Keyword = MixAppSettingKeywords.ThemeId,
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
            string defaultFolder = $"{MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.DefaultBlankTemplateFolder) }";

            bool copyResult = MixFileRepository.Instance.CopyDirectory(defaultFolder, TemplateFolder);

            var files = MixFileRepository.Instance.GetFilesWithContent(TemplateFolder);
            var id = _context.MixTemplate.Count() + 1;
            //TODO: Create default asset
            foreach (var file in files)
            {
                InitTemplateViewModel template = new InitTemplateViewModel(
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

        #endregion Async

        #endregion Overrides
    }
}
