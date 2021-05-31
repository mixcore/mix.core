using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using Mix.Lib.Constants;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Enums;
using Mix.Lib.Helpers;
using Mix.Lib.Services;
using System;
using System.Threading.Tasks;

namespace Mix.Lib.ViewModels.Cms
{
    public class MixMediaViewModel : ViewModelBase<MixCmsContext, MixMedia, MixMediaViewModel>
    {
        #region Properties

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public string FileFolder { get; set; }
        public string FileName { get; set; }
        public string FileProperties { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string Source { get; set; }
        public string TargetUrl { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }

        public string Domain { get { return MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.Domain); } }

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

        public FileViewModel MediaFile { get; set; }

        public IFormFile File { get; set; }

        #endregion Properties

        #region Contructors

        public MixMediaViewModel() : base()
        {
        }

        public MixMediaViewModel(MixMedia model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixMedia ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (CreatedDateTime == default)
            {
                Id = Id > 0 ? Id : Repository.Max(c => c.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            if (string.IsNullOrEmpty(TargetUrl))
            {
                if (FileFolder[0] == '/') { FileFolder = FileFolder[1..]; }
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void Validate(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (!FileFolder.StartsWith(MixFolders.UploadsFolder))
            {
                FileFolder = $"{MixFolders.UploadsFolder}/{FileFolder}";
            }
            if (MediaFile?.FileStream != null)
            {
                FileFolder = $"{MixCmsHelper.GetUploadFolder(Specificulture)}";
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
            FileType ??= "image";
            base.Validate(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            MediaFile = new FileViewModel();
        }

        public override RepositoryResponse<bool> RemoveRelatedModels(MixMediaViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>
            {
                IsSucceed = MixFileRepository.Instance.DeleteWebFile(FileName, Extension, FileFolder)
            };
            result.IsSucceed = Repository.RemoveListModel(false, m => m.Id == Id && m.Specificulture != Specificulture, _context, _transaction).IsSucceed;
            return result;
        }

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(MixMediaViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            // Remove local file
            if (FileFolder.IndexOf("http") < 0)
            {
                MixFileRepository.Instance.DeleteWebFile(FileName, Extension, FileFolder);
            }
            await Repository.RemoveListModelAsync(false, m => m.Id == Id && m.Specificulture != Specificulture, _context, _transaction);
            return await base.RemoveRelatedModelsAsync(view, _context, _transaction);
        }

        #endregion Overrides

        #region Expand

        #endregion Expand
    }
}
