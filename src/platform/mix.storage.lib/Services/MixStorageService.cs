using Microsoft.AspNetCore.Http;
using Mix.Storage.Lib.ViewModels;

namespace Mix.Storage.Lib.Services
{
    public class MixStorageService
    {
        private UnitOfWorkInfo _cmsUOW;
        public MixStorageService(UnitOfWorkInfo<MixCmsContext> cmsUOW)
        {
            _cmsUOW = cmsUOW;
        }
        #region Methods

        public async Task<string> UploadFile(string? folder, IFormFile file, int tenantId, string? createdBy)
        {
            folder ??= DateTime.Now.ToString("yyyy-MMM");
            folder = $"{MixFolders.UploadsFolder}/{folder.TrimStart('/').TrimEnd('/')}";
            string webPath = $"{MixFolders.WebRootPath}/{folder}";
            var result = MixFileHelper.SaveFile(file, webPath);
            if (!string.IsNullOrEmpty(result))
            {
                await CreateMedia(folder, result, tenantId, createdBy);
                return $"{GlobalConfigService.Instance.Domain}/{folder}/{result}";
            }
            return default;
        }

        public async Task CreateMedia(string folder, string result, int tenantId, string createdBy)
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
