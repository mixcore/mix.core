using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Mix.Cms.Api.GraphQL.Infrastructure
{
    public class MyFieldResolver : IFieldResolver
    {
        private TableMetadata _tableMetadata;
        private DbContext _dbContext; public MyFieldResolver(TableMetadata tableMetadata, DbContext dbContext)
        {
            _tableMetadata = tableMetadata;
            _dbContext = dbContext;
        }
        public object Resolve(ResolveFieldContext context)
        {
            var queryable = _dbContext.Query(_tableMetadata.AssemblyFullName);
            var id = context.GetArgument<string>("id");
            if (string.IsNullOrEmpty(id))
            {

                var first = context.Arguments["first"] != null ?
                    context.GetArgument("first", int.MaxValue) :
                    int.MaxValue; var offset = context.Arguments["offset"] != null ?
                    context.GetArgument("offset", 0) : 0; 
                return queryable.Skip(offset).Take(first).ToDynamicList<object>();
            }
            else
            {
                return queryable.FirstOrDefault($"Id == @0", id);
            }
        }
    }
}
