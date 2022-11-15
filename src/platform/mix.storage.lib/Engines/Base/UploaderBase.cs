using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Extensions;
using Mix.Storage.Lib.ViewModels;

namespace Mix.Storage.Lib.Engines.Base
{
    public abstract class UploaderBase : IMixUploader
    {
        protected ISession? _session;
        protected readonly IConfiguration _configuration;
        protected UnitOfWorkInfo _cmsUOW;
        protected MixTenantSystemViewModel _currentTenant;
        protected MixTenantSystemViewModel CurrentTenant
        {
            get
            {
                if (_currentTenant == null)
                {
                    _currentTenant = _session.Get<MixTenantSystemViewModel>(MixRequestQueryKeywords.Tenant);
                }
                return _currentTenant;
            }
        }

        public UploaderBase(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, UnitOfWorkInfo<MixCmsContext> cmsUOW)
        {
            _cmsUOW = cmsUOW;
            _configuration = configuration;
            _session = httpContextAccessor?.HttpContext?.Session;
        }

        public async Task CreateMedia(string fullname, int tenantId, string? createdBy, CancellationToken cancellationToken = default)
        {
            string filePath = fullname.Substring(0, fullname.LastIndexOf('/'));
            string fileName = fullname.Substring(fullname.LastIndexOf('/') + 1);
            var media = new MixMediaViewModel(_cmsUOW)
            {
                Id = Guid.NewGuid(),
                DisplayName = fileName,
                Status = MixContentStatus.Published,
                FileFolder = filePath.Replace(CurrentTenant.Configurations.Domain, string.Empty),
                FileName = fileName.Substring(0, fileName.LastIndexOf('.')),
                Extension = fileName.Substring(fileName.LastIndexOf('.')),
                CreatedBy = createdBy,
                MixTenantId = tenantId,
                CreatedDateTime = DateTime.UtcNow
            };

            await media.SaveAsync(cancellationToken);
        }

        public async Task<string?> UploadFile(IFormFile file, string? folder, string? createdBy, CancellationToken cancellationToken)
        {
            var result = await Upload(file, folder, createdBy, cancellationToken);
            if (!string.IsNullOrEmpty(result))
            {
                await CreateMedia(result, _currentTenant.Id, createdBy, cancellationToken);
            }

            return result;
        }

        public async Task<string?> UploadFileStream(FileModel file, string? createdBy, CancellationToken cancellationToken)
        {
            var result = await UploadStream(file, createdBy, cancellationToken);
            if (!string.IsNullOrEmpty(result))
            {
                await CreateMedia(result, _currentTenant.Id, createdBy, cancellationToken);
            }

            return result;
        }

        public abstract Task<string?> Upload(IFormFile file, string? themeName, string? createdBy, CancellationToken cancellationToken = default);
        public abstract Task<string?> UploadStream(FileModel file, string? createdBy, CancellationToken cancellationToken = default);
    }
}
