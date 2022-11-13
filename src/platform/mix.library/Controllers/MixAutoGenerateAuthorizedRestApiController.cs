using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Services;

namespace Mix.Lib.Controllers
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [MixAuthorize]
    public class MixAutoGenerateAuthorizedRestApiController<TView, TDbContext, TEntity, TPrimaryKey>
        : MixAutoGenerateRestApiController<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        public MixAutoGenerateAuthorizedRestApiController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<TDbContext> uow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, uow, queueService)
        {
        }
    }
}
