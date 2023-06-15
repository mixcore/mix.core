using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Storage.Lib.Engines.Base;

namespace Mix.Storage.Lib.Engines.Mix
{
    public class MixUploader : UploaderBase
    {
        public MixUploader(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            UnitOfWorkInfo<MixCmsContext> cmsUow)
            : base(httpContextAccessor, configuration, cmsUow)
        {
        }

        public override Task<string?> UploadStream(FileModel file, string? createdBy, CancellationToken cancellationToken = default)
        {
            string? result = null;
            var fileName = $"{DateTime.Now.Ticks}-{file.Filename}";
            file.FileFolder = GetUploadFolder(file.Extension, file.FolderName, createdBy);
            var saveResult = MixFileHelper.SaveFile(file);
            if (saveResult)
            {
                result = $"{CurrentTenant.Configurations.Domain}/{file.FileFolder}/{fileName}{file.Extension}";
            }

            return Task.FromResult(result);
        }

        public override Task<string?> Upload(IFormFile file, string? folder, string? createdBy, CancellationToken cancellationToken = default)
        {
            using (var fileStream = file.OpenReadStream())
            {
                string? result = null;
                var fileName = $"{DateTime.Now.Ticks}-{file.FileName}";
                if (string.IsNullOrEmpty(folder))
                {
                    folder = GetUploadFolder(fileName, folder, createdBy);
                }

                var fileModel = new FileModel(fileName, fileStream, folder);
                var saveResult = MixFileHelper.SaveFile(fileModel);
                if (saveResult)
                {
                    result = $"{CurrentTenant.Configurations.Domain}/{folder}/{fileName}";
                }

                return Task.FromResult(result);
            }
        }

        private string GetUploadFolder(string filename, string? fileFolder, string? createdBy)
        {
            string ext = filename.Split('.')[1].ToLower();
            string folder = $"{MixFolders.StaticFiles}/{CurrentTenant.SystemName}/{MixFolders.UploadsFolder}/{ext}";
            if (!string.IsNullOrEmpty(fileFolder))
            {
                folder = $"{folder}/{fileFolder}";
            }
            if (!string.IsNullOrEmpty(createdBy))
            {
                folder = $"{folder}/{createdBy}";
            }

            return $"{folder}/{DateTime.Now:yyyy-MM}";
        }
    }
}
