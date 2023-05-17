using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Storage.Lib.Engines.Aws;
using Mix.Storage.Lib.Engines.Base;
using Mix.Storage.Lib.Engines.CloudFlare;
using Mix.Storage.Lib.Engines.Mix;

namespace Mix.Storage.Lib.Services
{
    public class MixStorageService
    {
        private readonly IMixUploader _uploader;
        private readonly IConfiguration _configuration;
        private readonly HttpService _httpService;
        public MixStorageService(IHttpContextAccessor httpContext, UnitOfWorkInfo<MixCmsContext> cmsUow, IConfiguration configuration, HttpService httpService)
        {
            _configuration = configuration;
            _httpService = httpService;
            _uploader = CreateUploader(httpContext, cmsUow);
        }

        private IMixUploader CreateUploader(IHttpContextAccessor httpContext, UnitOfWorkInfo<MixCmsContext> cmsUow)
        {
            var providerSetting = _configuration["StorageSetting:Provider"];
            var provider = Enum.Parse<MixStorageProvider>(providerSetting);
            return provider switch
            {
                MixStorageProvider.CLOUDFLARE => new CloudFlareUploader(httpContext, _configuration, cmsUow,
                    _httpService),
                MixStorageProvider.AWS => new AwsUploader(httpContext, _configuration, cmsUow),
                MixStorageProvider.MIX => new MixUploader(httpContext, _configuration, cmsUow),
                _ => new MixUploader(httpContext, _configuration, cmsUow)
            };
        }
        #region Methods

        public async Task<string?> UploadFile(IFormFile file, string? themeName, string? createdBy)
        {
            return await _uploader.UploadFile(file, themeName, createdBy);
        }

        public async Task<string?> UploadStream(FileModel file, string? createdBy)
        {
            return await _uploader.UploadFileStream(file, createdBy);
        }
        #endregion
    }
}
