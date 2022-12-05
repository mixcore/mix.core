using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Storage.Lib.Engines.Base;

namespace Mix.Storage.Lib.Engines.CloudFlare
{
    public class CloudFlareUploader : UploaderBase
    {
        private readonly string _endpoint;
        private readonly HttpService _httpService;
        private readonly CloudFlareSettings _settings;
        public CloudFlareUploader(
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            HttpService httpService)
            : base(httpContext, configuration, cmsUow)
        {
            _httpService = httpService;
            _settings = new();
            Configuration.Bind("StorageSetting:CloudFlareSetting", _settings);
            _endpoint = string.Format(_settings.EndpointTemplate, _settings.AccountId);
        }

        public override async Task<string?> Upload(IFormFile file, string? themeName, string? createdBy, CancellationToken cancellationToken = default)
        {
            var result = await _httpService.PostAsync<CloudFlareResponse, IFormFile>(_endpoint, file, _settings.ApiToken, contentType: "multipart/form-data");
            return result.Result.Variants[0];
        }

        public override Task<string?> UploadStream(FileModel file, string? createdBy, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
