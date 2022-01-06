using Microsoft.EntityFrameworkCore;
using System;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Heart.ViewModel;
using Mix.Shared.Services;
using Mix.Lib.Base;
using Mix.Database.Entities.Cms;
using Microsoft.Extensions.Logging;
using Mix.Lib.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Mix.Lib.Attributes;
using Microsoft.Extensions.Configuration;
using Mix.Heart.Services;

namespace Mix.Lib.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[MixAuthorize]
    public class MixAutoGenerateRestApiController<TView, TDbContext, TEntity, TPrimaryKey>
        : MixRestApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView: ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        public MixAutoGenerateRestApiController(
            IConfiguration configuration,
            MixService mixService, 
            TranslatorService translator, 
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository, 
            MixIdentityService mixIdentityService,
            TDbContext context,
            MixCacheService cacheService,
            IQueueService<MessageQueueModel> queueService)
            : base(configuration, mixService, translator, cultureRepository, mixIdentityService, context, queueService)
        {
        }
    }
}
