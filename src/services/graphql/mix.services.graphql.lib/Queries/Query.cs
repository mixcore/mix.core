using Mix.Database.Services;
using Mix.Mixdb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Services.Graphql.Lib.Queries
{
    public class Query
    {
        private readonly MixDbDbContext _ctx;
        public Query(DatabaseService databaseService)
        {
            _ctx = new(databaseService);
        }
        public MixUserData? GetUserData() =>
            _ctx.MixUserData.FirstOrDefault();
    }
}
