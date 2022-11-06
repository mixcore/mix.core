using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Storage.Lib.Engines.Base;
using Mix.Storage.Lib.Engines.CloudFlare;
using Mix.Storage.Lib.Engines.Mix;

namespace Mix.Storage.Lib.Services
{
    public class MixStorageService
    {
        private IMixUploader _uploader;
        private readonly IConfiguration _configuration;
        private readonly HttpService _httpService;
        public MixStorageService(IHttpContextAccessor httpContext, UnitOfWorkInfo<MixCmsContext> cmsUOW, IConfiguration configuration, HttpService httpService)
        {
            _configuration = configuration;
            _httpService = httpService;
            _uploader = CreateUploader(httpContext, cmsUOW);
        }

        private IMixUploader CreateUploader(IHttpContextAccessor httpContext, UnitOfWorkInfo<MixCmsContext> cmsUOW)
        {
            var providerSetting = _configuration["StorageSetting:Provider"];
            var provider = Enum.Parse<MixStorageProvider>(providerSetting);
            switch (provider)
            {
                case MixStorageProvider.CLOUDFLARE:
                    return new CloudFlareUploader(httpContext, _configuration, cmsUOW, _httpService);
                case MixStorageProvider.MIX:
                default:
                    return new MixUploader(httpContext, _configuration, cmsUOW);
            }
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
