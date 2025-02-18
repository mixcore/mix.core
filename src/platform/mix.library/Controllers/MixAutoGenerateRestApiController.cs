using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;
using Mix.SignalR.Interfaces;

namespace Mix.Lib.Controllers
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class MixAutoGenerateRestApiController<TView, TDbContext, TEntity, TPrimaryKey>
        : MixRestfulApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : class, IEntity<TPrimaryKey>
        where TView : SimpleViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        public MixAutoGenerateRestApiController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixCacheService cacheService, MixIdentityService mixIdentityService, UnitOfWorkInfo<TDbContext> uow, IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
        }
    }
}
