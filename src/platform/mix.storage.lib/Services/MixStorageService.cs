using Microsoft.AspNetCore.Http;
using Mix.Storage.Lib.ViewModels;

namespace Mix.Storage.Lib.Services
{
    public class MixStorageService
    {
        private UnitOfWorkInfo _cmsUOW;
        private string? _tenantName;
        private int _tenantId;

        public MixStorageService(IHttpContextAccessor httpContext, UnitOfWorkInfo<MixCmsContext> cmsUOW)
        {
            _cmsUOW = cmsUOW;
            if (httpContext.HttpContext!.Session.GetInt32(MixRequestQueryKeywords.TenantId).HasValue)
            {
                _tenantId = httpContext.HttpContext.Session.GetInt32(MixRequestQueryKeywords.TenantId)!.Value;
            }
            _tenantName = httpContext.HttpContext?.Session.GetString(MixRequestQueryKeywords.TenantName);
        }
        #region Methods

        public async Task<string?> UploadFile(IFormFile file, string? themeName, string? createdBy)
        {
            var folder = $"{MixFolders.StaticFiles}/{_tenantName}/{themeName}/{MixFolders.UploadsFolder}/{DateTime.Now.ToString("yyyy-MMM")}";
            var result = MixFileHelper.SaveFile(file, folder);
            if (!string.IsNullOrEmpty(result))
            {
                await CreateMedia(folder, result, _tenantId, createdBy);
                return $"{GlobalConfigService.Instance.Domain}/{folder}/{result}";
            }
            return default;
        }

        public async Task CreateMedia(string folder, string result, int tenantId, string? createdBy)
        {
            var media = new MixMediaViewModel(_cmsUOW)
            {
                Id = Guid.NewGuid(),
                DisplayName = result,
                FileFolder = folder,
                FileName = result,
                CreatedBy = createdBy,
                MixTenantId = tenantId,
                CreatedDateTime = DateTime.UtcNow
            };
            await media.SaveAsync();
        }

        #endregion
    }
}
