using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Extensions;
using Mix.Storage.Lib.ViewModels;

namespace Mix.Storage.Lib.Engines.Base
{
    public abstract class UploaderBase : IMixUploader
    {
        protected readonly IConfiguration _configuration;
        protected UnitOfWorkInfo _cmsUOW;
        protected MixTenantSystemViewModel _currentTenant;

        public UploaderBase(IHttpContextAccessor httpContext, IConfiguration configuration, UnitOfWorkInfo<MixCmsContext> cmsUOW)
        {
            _cmsUOW = cmsUOW;
            _configuration = configuration;
            
            if (httpContext.HttpContext!.Session.GetInt32(MixRequestQueryKeywords.Tenant).HasValue)
            {
                _currentTenant = httpContext.HttpContext.Session.Get<MixTenantSystemViewModel>(MixRequestQueryKeywords.Tenant);
            }
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
                await CreateMedia(result, _currentTenant.Id, createdBy);
            }
            return result;
        }
        public abstract Task<string?> Upload(IFormFile file, string? themeName, string? createdBy);
    }
}
