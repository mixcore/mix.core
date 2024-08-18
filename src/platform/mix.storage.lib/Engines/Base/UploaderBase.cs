using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Service.Models;
using Mix.Shared.Extensions;
using Mix.Storage.Lib.ViewModels;

namespace Mix.Storage.Lib.Engines.Base
{
    public abstract class UploaderBase : IMixUploader
    {
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

        protected UploaderBase(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, UnitOfWorkInfo<MixCmsContext> cmsUow)
        {
            CmsUow = cmsUow;
            Configuration = configuration;
            Session = httpContextAccessor?.HttpContext?.Session;
        }

        public async Task CreateMedia(string fullname, int tenantId, string? createdBy, CancellationToken cancellationToken = default)
        {
            string filePath = fullname[..fullname.LastIndexOf('/')];
            string fileFolder = CurrentTenant.Configurations.Domain != null
                        ? filePath.Replace(CurrentTenant.Configurations.Domain, string.Empty)
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
                MixTenantId = tenantId,
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
