using Mix.Services.Databases.Lib.Abtracts;
using Mix.Services.Databases.Lib.Services;

namespace Mixcore.Domain.Services
{
    public sealed class MixcorePostService : MixPostServiceBase<PostContentViewModel>
    {
        public MixcorePostService(UnitOfWorkInfo<MixCmsContext> uow, MixMetadataService metadataService, IHttpContextAccessor httpContextAccessor) : base(uow, metadataService, httpContextAccessor)
        {
        }
    }
}
