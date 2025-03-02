using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Configuration;
using Mix.Heart.Models;
using Mix.Mq.Lib.Models;
using Mix.Shared.Helpers;
using Mix.Storage.Lib.Engines.Base;
using Mix.Storage.Lib.Helpers;
using Mix.Storage.Lib.Models;
using System.IO;

namespace Mix.Storage.Lib.Engines.Mix
{
    public class MixUploader : UploaderBase
    {
        protected readonly IMemoryQueueService<MessageQueueModel> _queueService;
        public StorageSettingsModel Settings { get; set; } = new();
        public string? _httpScheme;
        public MixUploader(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMemoryQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, cmsUow)
        {
            Configuration.Bind("StorageSettings", Settings);
            _queueService = queueService;
            _httpScheme = configuration.GetValue<string>("HttpScheme");
        }

        public override Task<string?> UploadStream(FileModel file, string? createdBy, CancellationToken cancellationToken = default)
        {
            string? result = null;
            file = GetFileModel($"{file.Filename}{file.Extension}", file.FileStream, null, createdBy);
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
                    _queueService.PushMemoryQueue(CurrentTenant.Id, MixQueueTopics.MixBackgroundTasks, MixQueueActions.ScaleImage, fileModel.FullPath);
                }

                if (saveResult)
                {
                    result = $"{_httpScheme}://{CurrentTenant.Configurations.Domain}/{fileModel.FileFolder.Replace(MixFolders.WebRootPath, string.Empty)}/{fileModel.Filename}{fileModel.Extension}";
                }
                return Task.FromResult(result);
            }
        }

       
    }
}
