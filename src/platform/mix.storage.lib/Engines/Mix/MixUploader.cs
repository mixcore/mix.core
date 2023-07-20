using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Configuration;
using Mix.Heart.Models;
using Mix.Shared.Helpers;
using Mix.Storage.Lib.Engines.Base;
using Mix.Storage.Lib.Helpers;
using Mix.Storage.Lib.Models;
using System.IO;

namespace Mix.Storage.Lib.Engines.Mix
{
    public class MixUploader : UploaderBase
    {
        protected readonly IQueueService<MessageQueueModel> _queueService;
        public StorageSettingsModel Settings { get; set; } = new();
        public MixUploader(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, cmsUow)
        {
            Configuration.Bind("StorageSetting", Settings);
            _queueService = queueService;
        }

        public override Task<string?> UploadStream(FileModel file, string? createdBy, CancellationToken cancellationToken = default)
        {
            string? result = null;
            file = GetFileModel($"{file.Filename}{file.Extension}", null, null, createdBy);
            var saveResult = MixFileHelper.SaveFile(file);
            if (saveResult)
            {
                result = $"{CurrentTenant.Configurations.Domain}/{file.FileFolder}/{file.Filename}";
            }

            return Task.FromResult(result);
        }

        public override Task<string?> Upload(IFormFile file, string? folder, string? createdBy, CancellationToken cancellationToken = default)
        {
            using (var fileStream = file.OpenReadStream())
            {
                string? result = null;
                FileModel fileModel = GetFileModel(file.FileName, fileStream, folder, createdBy);
                var saveResult = MixFileHelper.SaveFile(fileModel);

                if (Settings.IsAutoScaleImage && ImageHelper.IsImageResizeable(fileModel.Extension))
                {
                    _queueService.PushQueue(CurrentTenant.Id, MixQueueTopics.MixBackgroundTasks, MixQueueActions.ScaleImage, fileModel.FullPath);
                }

                if (saveResult)
                {
                    result = $"{CurrentTenant.Configurations.Domain}/{fileModel.FileFolder}/{fileModel.Filename}{fileModel.Extension}";
                }
                return Task.FromResult(result);
            }
        }

        public FileModel GetFileModel(string fileName, Stream fileStream, string? folder, string? createdBy)
        {
            var name = fileName.Substring(0, fileName.LastIndexOf('.')).Replace(" ", string.Empty).ToLower();
            var ext = fileName.Substring(fileName.LastIndexOf('.')).ToLower();
            folder = GetUploadFolder(ext, folder, createdBy);

            if (ImageHelper.IsImageResizeable(ext))
            {
                return new FileModel($"{name}{ext}", fileStream, $"{folder}/{DateTime.Now.Ticks}");
            }
            else
            {
                return new FileModel(fileName, fileStream, folder);
            }
        }

        private string GetUploadFolder(string ext, string? fileFolder, string? createdBy)
        {
            string folder = $"{MixFolders.StaticFiles}/{CurrentTenant.SystemName}/{MixFolders.UploadsFolder}/{ext.TrimStart('.')}";
            if (!string.IsNullOrEmpty(createdBy))
            {
                folder = $"{folder}/{createdBy}";
            }
            else
            {
                folder = $"{folder}/guest";
            }

            return $"{folder}/{DateTime.Now:yyyy-MM}".Replace(" ", string.Empty);
        }
    }
}
