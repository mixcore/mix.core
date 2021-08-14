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
using Mix.Identity.Services;

namespace Mix.Lib.Controllers
{
    public class MixAutoGenerateQueryApiController<TView, TDbContext, TEntity, TPrimaryKey>
        : MixQueryApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView: ViewModelBase<TDbContext, TEntity, TPrimaryKey>
    {
        public MixAutoGenerateQueryApiController(
            ILogger<MixApiControllerBase> logger,
            GlobalConfigService globalConfigService,
            MixService mixService, 
            TranslatorService translator, 
            Repository<MixCmsContext, MixCulture, int> cultureRepository, 
            Repository<TDbContext, TEntity, TPrimaryKey> repository,
            MixIdentityService mixIdentityService)
            : base(logger, globalConfigService, mixService, translator, cultureRepository, repository, mixIdentityService)
        {
        }
    }
}
