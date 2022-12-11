using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Repository;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Services.Databases.Lib.Abtracts;
using Mix.Services.Databases.Lib.Enums;
using Mix.Services.Databases.Lib.Services;
using System.Linq.Expressions;

namespace Mixcore.Domain.Services
{
    public sealed class MixcorePostService : MixPostServiceBase<PostContentViewModel>
    {
        public MixcorePostService(UnitOfWorkInfo<MixCmsContext> uow, MixMetadataService metadataService, IHttpContextAccessor httpContextAccessor) : base(uow, metadataService, httpContextAccessor)
        {
        }
    }
}
