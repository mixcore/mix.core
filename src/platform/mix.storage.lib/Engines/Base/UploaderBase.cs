using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Storage.Lib.ViewModels;

namespace Mix.Storage.Lib.Engines.Base
{
    public abstract class UploaderBase : IMixUploader
    {
        protected readonly IConfiguration _configuration;
        protected UnitOfWorkInfo _cmsUOW;
        protected string? _tenantName;
        protected int _tenantId;

        public UploaderBase(IHttpContextAccessor httpContext, IConfiguration configuration, UnitOfWorkInfo<MixCmsContext> cmsUOW)
        {
            _cmsUOW = cmsUOW;
            _configuration = configuration;
            if (httpContext.HttpContext!.Session.GetInt32(MixRequestQueryKeywords.TenantId).HasValue)
            {
                _tenantId = httpContext.HttpContext.Session.GetInt32(MixRequestQueryKeywords.TenantId)!.Value;
            }
            _tenantName = httpContext.HttpContext?.Session.GetString(MixRequestQueryKeywords.TenantName);
        }

        public async Task CreateMedia(string filePath, int tenantId, string? createdBy)
        {
            var media = new MixMediaViewModel(_cmsUOW)
            {
                Id = Guid.NewGuid(),
                DisplayName = filePath,
                FileName = filePath,
                CreatedBy = createdBy,
                MixTenantId = tenantId,
                CreatedDateTime = DateTime.UtcNow
            };
            await media.SaveAsync();
        }


        public async Task<string?> UploadFile(IFormFile file, string? themeName, string? createdBy)
        {
            var result = await Upload(file, themeName, createdBy);
            if (!string.IsNullOrEmpty(result))
            {
                await CreateMedia(result, _tenantId, createdBy);
            }
            return result;
        }
        public abstract Task<string?> Upload(IFormFile file, string? themeName, string? createdBy);
    }
}
