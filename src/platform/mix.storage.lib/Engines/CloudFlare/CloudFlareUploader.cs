using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Storage.Lib.Engines.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Storage.Lib.Engines.CloudFlare
{
    public class CloudFlareUploader : UploaderBase
    {
        private string _endpoint;
        private HttpService _httpService;
        private CloudFlareSettings settings;
        public CloudFlareUploader(
            IHttpContextAccessor httpContext, 
            IConfiguration configuration, 
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            HttpService httpService)
            : base(httpContext, configuration, cmsUOW)
        {
            _httpService = httpService;
            settings = new();
            _configuration.Bind("StorageSetting:CloudFlareSetting", settings);
            _endpoint = string.Format(settings.EndpointTemplate, settings.AccountId);
        }

        public override async Task<string?> Upload(IFormFile file, string? themeName, string? createdBy)
        {
            var result = await _httpService.PostAsync<CloudFlareResponse, IFormFile>(_endpoint, file, settings.ApiToken, contentType: "multipart/form-data");
            return result.Result.Variants[0];
        }

        public override Task<string?> UploadStream(FileModel file, string? createdBy)
        {
            throw new NotImplementedException();
        }
    }
}
