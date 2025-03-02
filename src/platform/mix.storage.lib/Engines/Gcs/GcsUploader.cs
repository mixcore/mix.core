using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Mix.Shared.Models.Configurations;
using Mix.Storage.Lib.Engines.Base;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using System.IO;

namespace Mix.Storage.Lib.Engines.Gcs
{
    public class GcsUploader : UploaderBase
    {
        private static GoogleSettingModel _settings;

        private static StorageClient _client;

        public GcsUploader(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, UnitOfWorkInfo<MixCmsContext> cmsUow) : base(httpContextAccessor, configuration, cmsUow)
        {
            LoadClient(configuration);
        }

        private void LoadClient(IConfiguration configuration)
        {
            _settings = new GoogleSettingModel();
            configuration.GetSection(MixAppSettingsSection.Google).Bind(_settings);
            if (_settings.Storage.Credential != null)
            {
                var credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(_settings.Storage.Credential));
                _client = StorageClient.Create(credential);
            }
        }

        public Bucket CreateBucket(string bucketName)
        {
            var storage = StorageClient.Create();
            var bucket = storage.CreateBucket(_settings.ProjectId, bucketName);
            return bucket;
        }

        public async Task DownloadFile(string bucketName, string objectName, string localPath)
        {
            using var outputFile = File.OpenWrite(localPath);
            await _client.DownloadObjectAsync(bucketName, objectName, outputFile);
        }

        public override async Task<string?> Upload(IFormFile file, string? folder, string? createdBy, CancellationToken cancellationToken = default)
        {
            using (var inputStream = new MemoryStream())
            {
                await file.CopyToAsync(inputStream, cancellationToken);
                FileModel fileModel = GetFileModel(file.FileName, inputStream, folder, createdBy);
                var result = await _client.UploadObjectAsync(
                    _settings.Storage.BucketName,
                    $"{fileModel.FileFolder}/{fileModel.Filename.ToSEOString('-')}{fileModel.Extension}",
                    GetMimeType(file.FileName), inputStream);

                return $"{GetHost()}/{result.Name}";
            }
        }

        public override async Task<string?> UploadStream(FileModel file, string? createdBy, CancellationToken cancellationToken = default)
        {
            var base64String = file.FileBase64.Split(',')[1];
            var bytes = Convert.FromBase64String(base64String);
            using (var inputStream = new MemoryStream(bytes))
            {
                file.FileFolder = GetUploadFolder(file.Extension, file.FolderName, createdBy);
                var result = await _client.UploadObjectAsync(
                    _settings.Storage.BucketName,
                    $"{file.FileFolder}/{file.Filename.ToSEOString('-')}{file.Extension}",
                    GetMimeType(file.Filename), inputStream);
                return $"{GetHost()}/{result.Name}";
            }
        }
        private string GetMimeType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
        protected override string GetHost()
        {
            return $"https://storage.googleapis.com/{_settings.Storage.BucketName}";
        }
    }
}
