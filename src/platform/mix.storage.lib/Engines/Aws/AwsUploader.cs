using System.Net;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Storage.Lib.Engines.Base;

namespace Mix.Storage.Lib.Engines.Aws
{
    public class AwsUploader : UploaderBase
    {
        private readonly AwsSetting _setting;

        private readonly IAmazonS3 _client;

        public AwsUploader(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, UnitOfWorkInfo<MixCmsContext> cmsUow) : base(httpContextAccessor, configuration, cmsUow)
        {
            _setting = new AwsSetting();
            Configuration.Bind("StorageSetting:AwsSetting", _setting);
            _client = new AmazonS3Client(
                _setting.AccessKeyId,
                _setting.SecretAccessKey,
                new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(_setting.Region)
                });
        }

        public override async Task<string?> Upload(IFormFile file, string? themeName, string? createdBy, CancellationToken cancellationToken = default)
        {
            using (var inputStream = new MemoryStream())
            {
                await file.CopyToAsync(inputStream, cancellationToken);

                var request = new PutObjectRequest
                {
                    BucketName = _setting.BucketName,
                    Key = string.IsNullOrEmpty(themeName) ? file.FileName : Path.Combine(themeName, file.FileName),
                    InputStream = inputStream,
                    CannedACL = S3CannedACL.PublicRead
                };

                return await GetStorageUrl(request, cancellationToken);
            }
        }

        public override async Task<string?> UploadStream(FileModel file, string? createdBy, CancellationToken cancellationToken = default)
        {
            var base64String = file.FileBase64.Split(',')[1];
            var bytes = Convert.FromBase64String(base64String);
            using (var inputStream = new MemoryStream(bytes))
            {
                var request = new PutObjectRequest
                {
                    BucketName = _setting.BucketName,
                    Key = string.IsNullOrEmpty(file.FileFolder) ? $"{file.Filename}{file.Extension}" : file.FullPath,
                    InputStream = inputStream,
                    CannedACL = S3CannedACL.PublicRead
                };

                return await GetStorageUrl(request, cancellationToken);
            }
        }

        private async Task<string> GetStorageUrl(PutObjectRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _client.PutObjectAsync(request, cancellationToken);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                return string.Empty;
            }

            var storageUrl = string.IsNullOrEmpty(_setting.CloudFrontUrl) ? _setting.BucketUrl : _setting.CloudFrontUrl;
            return $"{storageUrl}/{request.Key}";
        }
    }
}
