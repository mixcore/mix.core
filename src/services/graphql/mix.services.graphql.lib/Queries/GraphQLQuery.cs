using Microsoft.EntityFrameworkCore;
using Mix.Database.Services;
using Mix.Heart.Entities;
using Mix.Heart.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Services.Graphql.Lib.Queries
{
    public class GraphQLQuery<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        private readonly QueryRepository<MixDbDbContext, TEntity, TPrimaryKey> _repository;
        private readonly MixDbDbContext _ctx;
        public GraphQLQuery(DatabaseService databaseService)
        {
            _ctx = new(databaseService);
            _repository = new QueryRepository<MixDbDbContext, TEntity, TPrimaryKey>(_ctx);
        }

        public async Task<TEntity?> GetById(TPrimaryKey id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
