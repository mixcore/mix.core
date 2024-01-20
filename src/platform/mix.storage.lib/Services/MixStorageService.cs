using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Heart.Models;
using Mix.Mq.Lib.Models;
using Mix.Storage.Lib.Engines.Aws;
using Mix.Storage.Lib.Engines.AzureStorage;
using Mix.Storage.Lib.Engines.Base;
using Mix.Storage.Lib.Engines.CloudFlare;
using Mix.Storage.Lib.Engines.Mix;
using Mix.Storage.Lib.Helpers;
using Mix.Storage.Lib.Models;
using MySqlX.XDevAPI.Common;
using System.IO;

namespace Mix.Storage.Lib.Services
{
    public class MixStorageService
    {
        protected readonly IMemoryQueueService<MessageQueueModel> _queueService;
        private readonly IMixUploader _uploader;
        private readonly IConfiguration _configuration;
        private readonly HttpService _httpService;
        public StorageSettingsModel Settings { get; set; } = new();
        public MixStorageService(
            IHttpContextAccessor httpContext, 
            UnitOfWorkInfo<MixCmsContext> cmsUow, 
            IConfiguration configuration, 
            HttpService httpService, IMemoryQueueService<MessageQueueModel> queueService)
        {
            _configuration = configuration;
            _httpService = httpService;
            _queueService = queueService;
            _uploader = CreateUploader(httpContext, cmsUow);
        }

        private IMixUploader CreateUploader(IHttpContextAccessor httpContext, UnitOfWorkInfo<MixCmsContext> cmsUow)
        {
            _configuration.Bind("StorageSetting", Settings);

            return Settings.Provider switch
            {
                MixStorageProvider.CLOUDFLARE => new CloudFlareUploader(httpContext, _configuration, cmsUow,
                    _httpService),
                MixStorageProvider.AWS => new AwsUploader(httpContext, _configuration, cmsUow),
                MixStorageProvider.AZURE_STORAGE_BLOB => new AzureStorageUploader(httpContext, _configuration, cmsUow),
                MixStorageProvider.MIX => new MixUploader(httpContext, _configuration, cmsUow, _queueService),
                _ => new MixUploader(httpContext, _configuration, cmsUow, _queueService)
            };
        }
        #region Methods

        public async Task<string?> UploadFile(IFormFile file, string? themeName, string? createdBy)
        {
            try
            {
                return await _uploader.UploadFile(file, themeName, createdBy);
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        public async Task<string?> UploadStream(FileModel file, string? createdBy)
        {
            return await _uploader.UploadFileStream(file, createdBy);
        }

        public async Task ScaleImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }

            imageUrl = imageUrl.TrimStart('/');
            if (File.Exists(imageUrl))
            {
                using (var fileStream = File.OpenRead(imageUrl))
                {
                    var fileModel = MixFileHelper.GetFileByFullName(imageUrl);
                    if (ImageHelper.IsImageResizeable(fileModel.Extension))
                    {
                        foreach (var size in Settings.ImageSizes)
                        {
                            fileStream.Seek(0, SeekOrigin.Begin);
                            await ImageHelper.SaveImageAsync(fileStream, fileModel, size);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
