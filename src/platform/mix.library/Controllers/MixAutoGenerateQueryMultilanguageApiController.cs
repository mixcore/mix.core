using Microsoft.EntityFrameworkCore;
using System;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Heart.ViewModel;
using Mix.Shared.Services;
using Mix.Lib.Abstracts;

namespace Mix.Lib.Controllers
{
    public class MixAutoGenerateQueryMultilanguageApiController<TView, TDbContext, TEntity, TPrimaryKey>
        : MixQueryMultiLanguageApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView: ViewModelBase<TDbContext, TEntity, TPrimaryKey>
    {
        public MixAutoGenerateQueryMultilanguageApiController(
            Repository<TDbContext, TEntity, TPrimaryKey> repository, 
            MixAppSettingService appSettingService) 
            : base(appSettingService, repository)
        {
        }
    }
}
