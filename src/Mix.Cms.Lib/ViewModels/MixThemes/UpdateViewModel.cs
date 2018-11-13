using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Mix.Cms.Lib.ViewModels.MixSystem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixThemes
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixTheme, UpdateViewModel>
    {
        public const int templatePageSize = 10;

        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }
        #endregion Models

        #region Views

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
                return CommonHelper.GetFullPath(new string[] {
                    MixConstants.Folder.FileFolder,
                    MixConstants.Folder.TemplatesAssetFolder,
                    Name });
            }
        }

        [JsonProperty("templateFolder")]
        public string TemplateFolder
        {
            get
            {
                return CommonHelper.GetFullPath(new string[] { MixConstants.Folder.TemplatesFolder, Name });
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
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Templates = MixTemplates.UpdateViewModel.Repository.GetModelListBy(t => t.ThemeId == Id,
                _context: _context, _transaction: _transaction).Data;
            TemplateAsset = new FileViewModel() { FileFolder = $"Import/Themes/{DateTime.UtcNow.ToShortDateString()}" };
            Asset = new FileViewModel() { FileFolder = AssetFolder };
        }



        #region Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixTheme parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>() { IsSucceed = true };

            if (TemplateAsset.Content != null || TemplateAsset.FileStream != null)
            {
                ImportTheme(_context, _transaction);
            }
            if (Asset.Content != null || Asset.FileStream != null)
            {
                Asset.FileFolder = AssetFolder;
                Asset.Filename = "assets";
                string fullPath = CommonHelper.GetFullPath(new string[] {
                    MixConstants.Folder.WebRootPath,
                    Asset.FileFolder
                });
                FileRepository.Instance.EmptyFolder(fullPath);
                var isSaved = FileRepository.Instance.SaveWebFile(Asset);
                result.IsSucceed = isSaved;
                if (isSaved)
                {
                    result.IsSucceed = FileRepository.Instance.UnZipFile(Asset);
                    if (!result.IsSucceed)
                    {
                        result.Errors.Add("Cannot unzip file");
                    }

                }
                else
                {
                    result.Errors.Add("Cannot saved asset file");
                }

            }
            if (Id == 0)
            {
                string defaultFolder = CommonHelper.GetFullPath(new string[] { MixConstants.Folder.TemplatesFolder, Name == "Default" ? "Default" 
                    : MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultTemplateFolder) });
                bool copyResult = FileRepository.Instance.CopyDirectory(defaultFolder, TemplateFolder);
                var files = copyResult ? FileRepository.Instance.GetFilesWithContent(TemplateFolder) : new List<FileViewModel>();
                //TODO: Create default asset
                foreach (var file in files)
                {
                    MixTemplates.InitViewModel template = new MixTemplates.InitViewModel(
                        new MixTemplate()
                        {
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
                        });
                    var saveResult = await template.SaveModelAsync(true, _context, _transaction);
                    result.IsSucceed = result.IsSucceed && saveResult.IsSucceed;
                    if (!saveResult.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        result.Errors.AddRange(saveResult.Errors);
                        break;
                    }
                }
            }

            // Actived Theme
            if (IsActived)
            {
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
                if (!saveConfigResult.IsSucceed)
                {
                    Errors.AddRange(saveConfigResult.Errors);
                }
                else
                {
                    //MixCmsService.Instance.RefreshConfigurations(_context, _transaction);
                }
                result.IsSucceed = result.IsSucceed && saveConfigResult.IsSucceed;

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
                if (!saveResult.IsSucceed)
                {
                    Errors.AddRange(saveResult.Errors);
                }
                result.IsSucceed = result.IsSucceed && saveResult.IsSucceed;
            }

            if (Asset.Content != null || Asset.FileStream != null)
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

            return result;
        }

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = await MixTemplates.InitViewModel.Repository.RemoveListModelAsync(t => t.ThemeId == Id);
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

        public override RepositoryResponse<bool> SaveSubModels(MixTheme parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>() { IsSucceed = true };
            // import templates  + assets
            if (TemplateAsset.Content != null || TemplateAsset.FileStream != null)
            {
                result = ImportTheme(_context, _transaction);
            }

            // Create default template if create new without import template assets
            if (result.IsSucceed && Id == 0 && TemplateAsset.Content == null)
            {
                string defaultFolder = CommonHelper.GetFullPath(new string[] { MixConstants.Folder.TemplatesFolder, Name == "Default" ? "Default" :
                    MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultTemplateFolder) });
                bool copyResult = FileRepository.Instance.CopyDirectory(defaultFolder, TemplateFolder);
                var files = copyResult ? FileRepository.Instance.GetFilesWithContent(TemplateFolder) : new List<FileViewModel>();
                //TODO: Create default asset
                foreach (var file in files)
                {
                    MixTemplates.InitViewModel template = new MixTemplates.InitViewModel(
                        new MixTemplate()
                        {
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
                    var saveResult = template.SaveModel(true, _context, _transaction);
                    result.IsSucceed = result.IsSucceed && saveResult.IsSucceed;
                    if (!saveResult.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        result.Errors.AddRange(saveResult.Errors);
                        break;
                    }
                }
            }

            // Actived Theme
            if (result.IsSucceed && IsActived)
            {
                SystemConfigurationViewModel config = (SystemConfigurationViewModel.Repository.GetSingleModel(
                    c => c.Keyword == MixConstants.ConfigurationKeyword.ThemeName && c.Specificulture == Specificulture
                    , _context, _transaction)).Data;

                if (config == null)
                {
                    config = new SystemConfigurationViewModel(new MixConfiguration()
                    {
                        Keyword = MixConstants.ConfigurationKeyword.ThemeName,
                        Specificulture = Specificulture,
                        Category = "Site",
                        DataType = (int)DataType.Text,
                        Description = "Cms Theme",
                        Value = Name
                    }, _context, _transaction)
                    ;
                }
                else
                {
                    config.Value = Name;
                }

                var saveConfigResult = config.SaveModel(false, _context, _transaction);
                if (!saveConfigResult.IsSucceed)
                {
                    Errors.AddRange(saveConfigResult.Errors);
                }
                else
                {
                    //MixCmsService.Instance.RefreshConfigurations(_context, _transaction);
                }
                result.IsSucceed = result.IsSucceed && saveConfigResult.IsSucceed;

                SystemConfigurationViewModel configId = (SystemConfigurationViewModel.Repository.GetSingleModel(
                      c => c.Keyword == MixConstants.ConfigurationKeyword.ThemeId && c.Specificulture == Specificulture, _context, _transaction)).Data;
                if (configId == null)
                {
                    configId = new SystemConfigurationViewModel(new MixConfiguration()
                    {
                        Keyword = MixConstants.ConfigurationKeyword.ThemeId,
                        Specificulture = Specificulture,
                        Category = "Site",
                        DataType = (int)DataType.Text,
                        Description = "Cms Theme Id",
                        Value = Model.Id.ToString()
                    }, _context, _transaction)
                    ;
                }
                else
                {
                    configId.Value = Model.Id.ToString();
                }
                var saveResult = configId.SaveModel(false, _context, _transaction);
                if (!saveResult.IsSucceed)
                {
                    Errors.AddRange(saveResult.Errors);
                }
                else
                {
                    //MixCmsService.Instance.RefreshConfigurations(_context, _transaction);
                }
                result.IsSucceed = result.IsSucceed && saveResult.IsSucceed;
            }


            if (result.IsSucceed && TemplateAsset.Content != null || TemplateAsset.FileStream != null)
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

                layout.Data.SaveModel(true, _context, _transaction);
            }

            return result;
        }

        public override RepositoryResponse<bool> RemoveRelatedModels(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = MixTemplates.InitViewModel.Repository.RemoveListModel(t => t.ThemeId == Id);
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

        #endregion

        #endregion Overrides

        #region Expand

        RepositoryResponse<bool> ImportTheme(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            TemplateAsset.Filename = Name;
            if (FileRepository.Instance.SaveWebFile(TemplateAsset))
            {
                FileRepository.Instance.UnZipFile($"{TemplateAsset.Filename}{TemplateAsset.Extension}", TemplateAsset.FileFolder);
                FileRepository.Instance.CopyWebDirectory($"{TemplateAsset.FileFolder}/Assets", AssetFolder);
                FileRepository.Instance.CopyWebDirectory($"{TemplateAsset.FileFolder}/Templates", TemplateFolder);
                FileRepository.Instance.DeleteWebFolder(TemplateAsset.FileFolder);

                // Save template files to db                
                var files = FileRepository.Instance.GetFilesWithContent(TemplateFolder);
                //TODO: Create default asset
                foreach (var file in files)
                {
                    MixTemplates.UpdateViewModel template = new MixTemplates.UpdateViewModel(
                        new MixTemplate()
                        {
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
                        });
                    var saveResult = template.SaveModel(true, _context, _transaction);
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
            return result;
        }
        #endregion
    }
}
