using Mix.Services.Databases.Lib.Abstracts;
using Mix.Services.Databases.Lib.Interfaces;

namespace Mixcore.Domain.Services
{
    public sealed class MixcorePostService : MixPostServiceBase<PostContentViewModel>
    {
        public MixcorePostService(UnitOfWorkInfo<MixCmsContext> uow, IMixMetadataService metadataService, IHttpContextAccessor httpContextAccessor) : base(uow, metadataService, httpContextAccessor)
        {
        }
    }
}
