using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Service.Models;
using Mix.Shared.Extensions;
using Mix.Storage.Lib.Helpers;
using Mix.Storage.Lib.ViewModels;

namespace Mix.Storage.Lib.Engines.Base
{
    public abstract class UploaderBase : IMixUploader
    {
        protected IHttpContextAccessor? _httpContextAccessor;
        protected ISession? Session;
        protected readonly IConfiguration Configuration;
        protected UnitOfWorkInfo CmsUow;
        private MixTenantSystemModel _currentTenant;
        protected MixTenantSystemModel CurrentTenant
        {
            get
            {
                _currentTenant ??= Session?.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant) ?? new MixTenantSystemModel()
                {
                    Id = 1
                };
                return _currentTenant;
            }
        }
        protected virtual string GetHost()
        {
            return string.Empty;
        }
        protected string GetUploadFolder(string ext, string? fileFolder, string? createdBy)
        {
            string folder = $"{MixFolders.UploadFiles}/{CurrentTenant.SystemName}/{MixFolders.UploadsFolder}/{ext.TrimStart('.')}";
            if (!string.IsNullOrEmpty(createdBy))
            {
                folder = $"{folder}/{createdBy}";
            }
            else
            {
                folder = $"{folder}/guest";
            }

            return $"{folder.ToLower()}/{DateTime.Now:yyyy-MM}".Replace(' ', '-');
        }
        public FileModel GetFileModel(string fileName, Stream fileStream, string? folder, string? createdBy)
        {
            var name = fileName.Substring(0, fileName.LastIndexOf('.')).Replace(" ", string.Empty).ToLower();
            var ext = fileName.Substring(fileName.LastIndexOf('.')).ToLower();
            folder = GetUploadFolder(ext, folder, createdBy);

            if (ImageHelper.IsImageResizeable(ext))
            {
                return new FileModel($"{name}{ext}", fileStream, $"{folder}/{DateTime.Now.Ticks}");
            }
            else
            {
                return new FileModel(fileName, fileStream, folder);
            }
        }

        protected UploaderBase(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, UnitOfWorkInfo<MixCmsContext> cmsUow)
        {
            CmsUow = cmsUow;
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            Session = httpContextAccessor?.HttpContext?.Session;
        }

        public async Task CreateMedia(string fullname, int tenantId, string? createdBy, CancellationToken cancellationToken = default)
        {
            string filePath = fullname[..fullname.LastIndexOf('/')];
            string fileFolder = !string.IsNullOrEmpty(GetHost())
                        ? filePath.Replace(GetHost(), string.Empty)
                        : filePath;
            string fileName = fullname[(fullname.LastIndexOf('/') + 1)..];
            var media = new MixMediaViewModel(CmsUow)
            {
                Id = Guid.NewGuid(),
                DisplayName = fileName,
                Status = MixContentStatus.Published,
                FileFolder = fileFolder,
                FileName = fileName[..fileName.LastIndexOf('.')],
                Extension = fileName[fileName.LastIndexOf('.')..],
                TargetUrl = fullname,
                CreatedBy = createdBy,
                TenantId = tenantId,
                CreatedDateTime = DateTime.UtcNow
            };

            await media.SaveAsync(cancellationToken);
        }

        public async Task<string?> UploadFile(IFormFile file, string? folder, string? createdBy, CancellationToken cancellationToken)
        {
            try
            {
                var result = await Upload(file, folder, createdBy, cancellationToken);
                if (!string.IsNullOrEmpty(result))
                {
                    await CreateMedia(result, CurrentTenant.Id, createdBy, cancellationToken);
                }

                return result;
            }
            catch (MixException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        public async Task<string?> UploadFileStream(FileModel file, string? createdBy, CancellationToken cancellationToken)
        {
            try
            {
                var result = await UploadStream(file, createdBy, cancellationToken);
                if (!string.IsNullOrEmpty(result))
                {
                    await CreateMedia(result, CurrentTenant.Id, createdBy, cancellationToken);
                }

                return result;
            }
            catch (MixException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        public abstract Task<string?> Upload(IFormFile file, string? themeName, string? createdBy, CancellationToken cancellationToken = default);
        public abstract Task<string?> UploadStream(FileModel file, string? createdBy, CancellationToken cancellationToken = default);
    }
}
