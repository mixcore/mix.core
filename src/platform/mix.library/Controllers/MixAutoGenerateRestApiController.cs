using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Services;
using Mix.SignalR.Interfaces;

namespace Mix.Lib.Controllers
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class MixAutoGenerateRestApiController<TView, TDbContext, TEntity, TPrimaryKey>
        : MixRestfulApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        public MixAutoGenerateRestApiController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixCacheService cacheService, TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<TDbContext> uow, IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub) : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, uow, queueService, portalHub)
        {
        }
    }
}
