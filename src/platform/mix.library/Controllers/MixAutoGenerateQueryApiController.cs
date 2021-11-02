using Microsoft.EntityFrameworkCore;
using System;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Heart.ViewModel;
using Mix.Shared.Services;
using Mix.Lib.Abstracts;
using Mix.Database.Entities.Cms;
using Microsoft.Extensions.Logging;
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
            GlobalConfigService globalConfigService,
            MixService mixService, 
            TranslatorService translator, 
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository, 
            MixIdentityService mixIdentityService,
            TDbContext context)
            : base(configuration, globalConfigService, mixService, translator, cultureRepository, mixIdentityService, context)
        {
        }
    }
}
