using Microsoft.EntityFrameworkCore;
using System;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Heart.ViewModel;
using Mix.Shared.Services;

namespace Mix.Lib.Controllers
{
    public class MixRestAutoGenerateApiController<TView, TDbContext, TEntity, TPrimaryKey>
        : MixRestApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView: ViewModelBase<TDbContext, TEntity, TPrimaryKey>
    {
        public MixRestAutoGenerateApiController(
            CommandRepository<TDbContext, TEntity, TPrimaryKey> repository, 
            MixAppSettingService appSettingService) 
            : base(repository, appSettingService)
        {
        }
    }
}
