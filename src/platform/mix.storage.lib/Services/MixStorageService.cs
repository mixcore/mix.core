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

        public async Task UploadFile(string folder, IFormFile file)
        {
            folder ??= DateTime.Now.ToString("yyyy-MMM");
            folder = $"{MixFolders.UploadsFolder}/{folder.TrimStart('/').TrimEnd('/')}";
            string webPath = $"{MixFolders.WebRootPath}/{folder}";
            var result = MixFileHelper.SaveFile(file, webPath);
            if (!string.IsNullOrEmpty(result))
            {
                await CreateMedia(folder, result);
                //return Ok($"{GlobalConfigService.Instance.Domain}/{folder}/{result}");
            }
        }

        public Task CreateMedia(string folder, string result)
        {
            var media = new MixMediaViewModel(_cmsUOW)
            {
                
            };
            return Task.CompletedTask;
        }

        #endregion
    }
}
