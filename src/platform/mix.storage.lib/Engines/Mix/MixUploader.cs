﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Heart.Models;
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
            file.FileFolder = GetUploadFolder(file.Extension, file.FolderName, createdBy);
            var saveResult = MixFileHelper.SaveFile(file);
            if (saveResult != null)
            {
                result = $"{CurrentTenant.Configurations.Domain}/{file.FileFolder}/{saveResult.Filename}.{saveResult.Extension}";
            }

            return Task.FromResult(result);
        }

        public override Task<string?> Upload(IFormFile file, string? folder, string? createdBy, CancellationToken cancellationToken = default)
        {
            string? result = null;
            if (string.IsNullOrEmpty(folder))
            {
                folder = GetUploadFolder(file.FileName, folder, createdBy);
            }

            var saveResult = MixFileHelper.SaveFile(file, folder);
            if (saveResult != null)
            {
                result = $"{CurrentTenant.Configurations.Domain}/{saveResult.Filename}.{saveResult.Extension}";
            }
            return Task.FromResult(result);
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

            return $"{folder}/{DateTime.Now.ToString("yyyy-MMM")}";
        }
    }
}
