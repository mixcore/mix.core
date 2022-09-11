using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Storage.Lib.Engines.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Storage.Lib.Engines.Mix
{
    public class MixUploader : UploaderBase
    {
        public MixUploader(IHttpContextAccessor httpContext, IConfiguration configuration, UnitOfWorkInfo<MixCmsContext> cmsUOW) 
            : base(httpContext, configuration, cmsUOW)
        {
        }

        public override Task<string?> UploadStream(FileModel file, string? createdBy)
        {
            string? result = null;
            file.FileFolder = GetUploadFolder(file.FolderName, createdBy);
            var fileName = MixFileHelper.SaveFile(file);
            if (!string.IsNullOrEmpty(fileName))
            {
                result = $"{GlobalConfigService.Instance.Domain}/{file.FileFolder}/{fileName}";
            }
            return Task.FromResult(result);
        }
        
        public override Task<string?> Upload(IFormFile file, string? fileFolder, string? createdBy)
        {
            string? result = null;
            string folder = GetUploadFolder(fileFolder, createdBy);
            var fileName = MixFileHelper.SaveFile(file, folder);
            if (!string.IsNullOrEmpty(fileName))
            {
                result = $"{GlobalConfigService.Instance.Domain}/{folder}/{fileName}";
            }
            return Task.FromResult(result);
        }

        private string GetUploadFolder(string? fileFolder, string? createdBy)
        {
            string folder = $"{MixFolders.StaticFiles}/{_currentTenant.SystemName}/{MixFolders.UploadsFolder}";
            if (!string.IsNullOrEmpty(fileFolder))
            {
                folder = $"{folder}/{fileFolder}";
            }
            if (!string.IsNullOrEmpty(createdBy))
            {
                folder = $"{folder}/{createdBy}";
            }
            return $"{folder}/{DateTime.Now.ToString("yyyy-MMM")}";
        }
    }
}
