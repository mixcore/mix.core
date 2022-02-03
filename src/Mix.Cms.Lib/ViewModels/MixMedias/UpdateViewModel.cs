using Microsoft.AspNetCore.Http;
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
using System;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixMedias
{
    public class UpdateViewModel
        : ViewModelBase<MixCmsContext, MixMedia, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("extension")]
        public string Extension { get; set; }

        [JsonProperty("fileFolder")]
        public string FileFolder { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("fileType")]
        public string FileType { get; set; }

        [JsonProperty("fileSize")]
        public int FileSize { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("targetUrl")]
        public string TargetUrl { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

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

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain); } }

        [JsonProperty("fullPath")]
        public string FullPath
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName) && string.IsNullOrEmpty(TargetUrl))
                {
                    return FileFolder.IndexOf("http") > 0 ? $"{FileFolder}/{FileName}{Extension}"
                        : $"{Domain}/{FileFolder}/{FileName}{Extension}";
                }
                else
                {
                    return TargetUrl;
                }
            }
        }

        [JsonProperty("filePath")]
        public string FilePath
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName) && string.IsNullOrEmpty(TargetUrl))
                {
                    return FileFolder.IndexOf("http") > 0 ? $"{FileFolder}/{FileName}{Extension}"
                        : $"/{FileFolder}/{FileName}{Extension}";
                }
                else
                {
                    return TargetUrl;
                }
            }
        }

        [JsonProperty("mediaFile")]
        public FileViewModel MediaFile { get; set; }

        [JsonProperty("file")]
        public IFormFile File { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixMedia model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixMedia ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (CreatedDateTime == default)
            {
                Id = Id > 0 ? Id : UpdateViewModel.Repository.Max(c => c.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            if (string.IsNullOrEmpty(TargetUrl))
            {
                if (FileFolder[0] == '/') { FileFolder = FileFolder.Substring(1); }
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void Validate(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            base.Validate(_context, _transaction);
            FileFolder = $"{MixService.GetTemplateUploadFolder(Specificulture)}";
            if (MediaFile?.FileStream != null)
            {
                MediaFile.Filename = $"{SeoHelper.GetSEOString(MediaFile.Filename).ToLower()}-{ DateTime.UtcNow.Ticks}";
                MediaFile.FileFolder = FileFolder;
                var isSaved = MixFileRepository.Instance.SaveWebFile(MediaFile);
                if (isSaved)
                {
                    Extension = MediaFile.Extension.ToLower();
                    FileName = MediaFile.Filename;
                    FileFolder = MediaFile.FileFolder;
                    if (string.IsNullOrEmpty(Title))
                    {
                        Title = FileName;
                    }
                }
                else
                {
                    IsValid = false;
                }
            }
            else
            {
                if (File != null)
                {
                    FileName = $"{SeoHelper.GetSEOString(File.FileName[..File.FileName.LastIndexOf('.')]).ToLower()}-{ DateTime.UtcNow.Ticks}";
                    Extension = File.FileName[File.FileName.LastIndexOf('.')..].ToLower();
                    MixFileRepository.Instance.CreateDirectoryIfNotExist($"{MixFolders.WebRootPath}/{FileFolder}");
                    var saveFile = MixFileRepository.Instance.SaveWebFile(File, FileFolder);
                    if (saveFile == null)
                    {
                        IsValid = false;
                        Errors.Add("Cannot save file");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(Title))
                        {
                            Title = FileName;
                        }
                        FileName = saveFile.Filename;
                        Extension = saveFile.Extension;
                    }
                }
            }
            FileType = FileType ?? "image";
            
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            MediaFile = new FileViewModel();
        }

        public override RepositoryResponse<bool> RemoveRelatedModels(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>
            {
                IsSucceed = MixFileRepository.Instance.DeleteWebFile(FileName, Extension, FileFolder)
            };
            result.IsSucceed = Repository.RemoveListModel(false, m => m.Id == Id && m.Specificulture != Specificulture, _context, _transaction).IsSucceed;
            return result;
        }

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            // Remove local file
            if (FileFolder.IndexOf("http") < 0)
            {
                MixFileRepository.Instance.DeleteWebFile(FileName, Extension, FileFolder);
                MixFileRepository.Instance.DeleteWebFile($"{FileName}_XL", Extension, FileFolder);
                MixFileRepository.Instance.DeleteWebFile($"{FileName}_L", Extension, FileFolder);
                MixFileRepository.Instance.DeleteWebFile($"{FileName}_M", Extension, FileFolder);
                MixFileRepository.Instance.DeleteWebFile($"{FileName}_S", Extension, FileFolder);
                MixFileRepository.Instance.DeleteWebFile($"{FileName}_XS", Extension, FileFolder);
                MixFileRepository.Instance.DeleteWebFile($"{FileName}_XXS", Extension, FileFolder);
            }
            await Repository.RemoveListModelAsync(false, m => m.Id == Id && m.Specificulture != Specificulture, _context, _transaction);
            return await base.RemoveRelatedModelsAsync(view, _context, _transaction);
        }

        #endregion Overrides

        #region Expand
        #endregion Expand
    }
}