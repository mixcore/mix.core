using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Mix.Storage.Lib.Engines.Base;
using Mix.Storage.Lib.Engines.CloudFlare;
using Org.BouncyCastle.Ocsp;

namespace Mix.Storage.Lib.Engines.AzureStorage
{
    public class AzureStorageUploader : UploaderBase
    {
        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG" };
        public static readonly List<string> VideoExtensions = new List<string> { ".MP4", ".MOV", ".FLV", ".WMV", ".AVI", ".AVCHD", ".WEBM", "MKV" };
        private readonly BlobContainerClient _blobClient;
        private readonly AzureStorageSettings _settings;
        public AzureStorageUploader(
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            UnitOfWorkInfo<MixCmsContext> cmsUow)
            : base(httpContext, configuration, cmsUow)
        {
            _settings = new();
            Configuration.Bind("StorageSetting:AzureStorageSetting", _settings);
            _blobClient = new BlobContainerClient(_settings.AzureWebJobStorage, _settings.ContainerName);
            if (string.IsNullOrEmpty(_settings.CdnUrl))
            {
                _settings.CdnUrl = $"https://{_settings.StorageAccountName}.blob.core.windows.net";
            }

        }

        public override async Task<string?> Upload(IFormFile file, string? folder, string? createdBy, CancellationToken cancellationToken = default)
        {
            using (Stream myBlob = file.OpenReadStream())
            {
                var fileFolder = GetUploadFolder(file.FileName, folder, createdBy);
                var fileName = $"{fileFolder}/{DateTime.Now.Ticks}-{file.FileName}";
                var blob = _blobClient.GetBlobClient(fileName);
                BlobHttpHeaders h = new BlobHttpHeaders();
                h.ContentType = GetContentType(Path.GetExtension(file.FileName));
                await blob.UploadAsync(myBlob, h);
                return $"{_settings.CdnUrl}/{_settings.ContainerName}/{fileName}";
            }
        }

        private string GetContentType(string fileExtension)
        {
            if (ImageExtensions.Contains(fileExtension.ToUpperInvariant()))
            {
                return $"image/{fileExtension.Replace(".", string.Empty)}";
            }
            if (VideoExtensions.Contains(fileExtension.ToUpperInvariant()))
            {
                return $"video/{fileExtension.Replace(".", string.Empty)}";
            }
            return "application/octet-stream";
        }

        public override async Task<string?> UploadStream(FileModel file, string? createdBy, CancellationToken cancellationToken = default)
        {
            if (file.FileBase64.IsBase64())
            {
                var fileFolder = GetUploadFolder(file.Extension, file.FolderName, createdBy);
                var bytes = Convert.FromBase64String(file.FileBase64.ToBase64Stream());
                using (var stream = new MemoryStream(bytes))
                {
                    var fileName = $"{fileFolder}/{DateTime.Now.Ticks}-{file.Filename}{file.Extension}";
                    var blob = _blobClient.GetBlobClient(fileName);
                    await _blobClient.UploadBlobAsync(file.Filename, stream, cancellationToken);
                    return $"{_settings.CdnUrl}/{_settings.ContainerName}/{fileName}";
                }
            }
            throw new MixException("Azure Storage Blob Uploader: Invalid Base64 string");
        }

        private string GetUploadFolder(string filename, string? fileFolder, string? createdBy)
        {
            string ext = filename.Split('.')[1].ToLower();
            string folder = $"{CurrentTenant.SystemName}/{MixFolders.UploadsFolder}/{ext}";
            if (!string.IsNullOrEmpty(fileFolder))
            {
                folder = $"{folder}/{fileFolder}";
            }
            if (!string.IsNullOrEmpty(createdBy))
            {
                folder = $"{folder}/{createdBy}";
            }

            return $"{folder}/{DateTime.Now.ToString("yyyy-MM")}";
        }
    }
}
