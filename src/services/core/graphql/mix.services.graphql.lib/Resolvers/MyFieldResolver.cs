using GraphQL;
using GraphQL.Resolvers;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Base;
using Mix.Heart.Extensions;
using Mix.Services.Graphql.Lib;
using Mix.Services.Graphql.Lib.Models;
using System.Linq.Dynamic.Core;

namespace Mix.Services.Graphql.Lib.Resolvers
{
    public class MyFieldResolver : IFieldResolver
    {
        private TableMetadata _tableMetadata;
        private DbContext _dbContext;
        public MyFieldResolver(TableMetadata tableMetadata, DbContext dbContext)
        {
            _tableMetadata = tableMetadata;
            _dbContext = dbContext;
        }

        public async ValueTask<object?> ResolveAsync(IResolveFieldContext context)
        {
            var queryable = _dbContext.Query(_tableMetadata.AssemblyFullName);

            if (queryable is null)
            {
                return default;
            }

            // Get filters
            var filters = context.Arguments!.Where(c => c.Key != "first" && c.Key != "offset");
            string predicates = string.Empty;
            object[] args = new object[filters.Count()];
            int paramsCount = -1;

            foreach (var item in filters.Where(f => f.Value.Value != null))
            {
                paramsCount++;
                if (!string.IsNullOrEmpty(predicates))
                {
                    predicates += " and ";
                }

                args[paramsCount] = item.Value!.Value!;
                // Note: check for like function https://github.com/StefH/System.Linq.Dynamic.Core/issues/105                
                predicates += $"{item.Key.ToTitleCase()} == @{paramsCount}";
            }

            if (context.FieldDefinition.Name.Contains("_list"))
            {
                int first = context.GetArgument("first", int.MaxValue);
                int offset = context.GetArgument("offset", 0);

                if (paramsCount >= 0)
                {

                    queryable = queryable.Where(predicates, args);
                }

                var result = await queryable.Skip(offset).Take(first).ToDynamicListAsync<object>();
                return result;
            }
            else
            {
                return paramsCount >= 0 ? queryable?.FirstOrDefault(predicates, args) : null;
            }
        }
    }
}
