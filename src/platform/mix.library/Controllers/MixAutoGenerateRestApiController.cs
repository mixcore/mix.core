using Microsoft.EntityFrameworkCore;
using System;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Heart.ViewModel;
using Mix.Shared.Services;
using Mix.Lib.Abstracts;
using Mix.Database.Entities.Cms.v2;

namespace Mix.Lib.Controllers
{
    public class MixAutoGenerateRestApiController<TView, TDbContext, TEntity, TPrimaryKey>
        : MixRestApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView: ViewModelBase<TDbContext, TEntity, TPrimaryKey>
    {
        public MixAutoGenerateRestApiController(
            Repository<TDbContext, TEntity, TPrimaryKey> repository, 
            MixAppSettingService appSettingService,
            Repository<MixCmsContext, MixCulture, int> cultureRepository) : base(appSettingService, repository, cultureRepository)
        {
        }
    }
}
