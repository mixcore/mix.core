using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixThemes
{
    public class InitViewModel
      : ViewModelBase<MixCmsContext, MixTheme, InitViewModel>
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
                return CommonHelper.GetFullPath(new string[] {
                    MixConstants.Folder.WebRootPath,
                    MixConstants.Folder.FileFolder,
                    MixConstants.Folder.TemplatesAssetFolder,
                    Name
                });
            }
        }
        public string UploadsFolder
        {
            get
            {
                return CommonHelper.GetFullPath(new string[] {
                    MixConstants.Folder.FileFolder,
                    MixConstants.Folder.UploadFolder,
                    Name
                });
            }
        }

        [JsonProperty("templateFolder")]
        public string TemplateFolder
        {
            get
            {
                return CommonHelper.GetFullPath(new string[] {
                    MixConstants.Folder.TemplatesFolder,
                    Name
                });
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

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Templates = MixTemplates.InitViewModel.Repository.GetModelListBy(t => t.ThemeId == Id,
                _context: _context, _transaction: _transaction).Data;
            TemplateAsset = new FileViewModel() { FileFolder = $"Import/Themes/{DateTime.UtcNow.ToShortDateString()}/{Name}" };
            Asset = new FileViewModel() { FileFolder = AssetFolder };
        }

        #region Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixTheme parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                // Clone Default templates
                Name = SeoHelper.GetSEOString(Title);
                string defaultTemplateFolder = CommonHelper.GetFullPath(new string[] {
                    MixConstants.Folder.TemplatesFolder,
                    "Default" });
                bool copyResult = FileRepository.Instance.CopyDirectory(defaultTemplateFolder, TemplateFolder);
                string defaultAssetsFolder = CommonHelper.GetFullPath(new string[] {
                    MixConstants.Folder.WebRootPath,
                    "assets",
                    MixConstants.Folder.TemplatesAssetFolder,
                    "default"
                });
                copyResult = FileRepository.Instance.CopyDirectory(defaultAssetsFolder, AssetFolder);

                var files = FileRepository.Instance.GetFilesWithContent(TemplateFolder);
                //TODO: Create default asset
                int id = 0; 
                foreach (var file in files)
                {
                    id++;
                    string content = file.Content.Replace("/assets/templates/default", $"/{ MixConstants.Folder.FileFolder}/{MixConstants.Folder.TemplatesAssetFolder}/{Name}");
                    MixTemplates.InitViewModel template = new MixTemplates.InitViewModel(
                        new MixTemplate()
                        {
                            Id = id,
                            FileFolder = file.FileFolder,
                            FileName = file.Filename,
                            Content = content,
                            Extension = file.Extension,
                            CreatedDateTime = DateTime.UtcNow,
                            LastModified = DateTime.UtcNow,
                            ThemeId = Model.Id,
                            ThemeName = Model.Name,
                            FolderType = file.FolderName,
                            ModifiedBy = CreatedBy
                        }, _context, _transaction);
                    var saveResult = await template.SaveModelAsync(true, _context, _transaction);
                    result.IsSucceed = result.IsSucceed && saveResult.IsSucceed;
                    if (!saveResult.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        result.Errors.AddRange(saveResult.Errors);
                        break;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Errors.Add(ex.Message);
                result.Exception = ex;
                return result;
            }

        }

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(InitViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = await MixTemplates.InitViewModel.Repository.RemoveListModelAsync(false,  t => t.ThemeId == Id);
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

        #endregion Overrides


    }
}
