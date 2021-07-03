using Microsoft.EntityFrameworkCore;
using System;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Heart.ViewModel;

namespace Mix.Lib.Controllers
{
    public class MixRestAutoGenerateApiController<TView, TDbContext, TEntity, TPrimaryKey>
        : MixRestApiControllerBase<TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : class, IEntity<TPrimaryKey>
        where TView: ViewModelBase<TDbContext, TEntity, TPrimaryKey>
    {
        public MixRestAutoGenerateApiController(QueryRepository<TDbContext, TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
}
