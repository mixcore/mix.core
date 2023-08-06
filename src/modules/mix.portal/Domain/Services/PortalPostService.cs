using Mix.Lib.Interfaces;
using Mix.Services.Databases.Lib.Abstracts;
using Mix.Services.Databases.Lib.Interfaces;

namespace Mix.Portal.Domain.Services
{
    public sealed class PortalPostService : MixPostServiceBase<MixPostContentViewModel>
    {
        public PortalPostService(
            UnitOfWorkInfo<MixCmsContext> uow,
            IMixMetadataService metadataService,
            IHttpContextAccessor httpContextAccessor,
            MixCacheService cacheService,
            IMixTenantService mixTenantService)
            : base(uow, metadataService, httpContextAccessor, cacheService, mixTenantService)
        {
        }
    }
}
