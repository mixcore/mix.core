using Microsoft.EntityFrameworkCore;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Lib.Services;
using Microsoft.Extensions.Configuration;

namespace Mix.Lib.Controllers
{
    public class MixAutoGenerateQueryApiController<TView, TDbContext, TEntity, TPrimaryKey>
        : MixQueryApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView: ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        public MixAutoGenerateQueryApiController(
            IConfiguration configuration,
            MixService mixService, 
            TranslatorService translator, 
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository, 
            MixIdentityService mixIdentityService,
            TDbContext context,
            MixCacheService cacheService,
            IQueueService<MessageQueueModel> queueService)
            : base(configuration, mixService, translator, cultureRepository, mixIdentityService, context, cacheService, queueService)
        {
        }
    }
}
