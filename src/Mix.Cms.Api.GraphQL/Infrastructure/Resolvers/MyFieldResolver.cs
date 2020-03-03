using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Extensions;
using Mix.Domain.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;

namespace Mix.Cms.Api.GraphQL.Infrastructure
{
    public class MyFieldResolver: IFieldResolver
    {
        private TableMetadata _tableMetadata;
        private Type _type;
        //private DefaultModelRepository<DbContext _repo;
        private DbContext _dbContext; 
        public MyFieldResolver(TableMetadata tableMetadata, DbContext dbContext)
        {
            var assem = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a=>a.ManifestModule.Name=="Mix.Cms.Lib.dll");
            _type = assem.GetType(tableMetadata.AssemblyFullName);
            _tableMetadata = tableMetadata;
            _dbContext = dbContext;
        }
        public object Resolve(ResolveFieldContext context)
        {
            //var genericType = typeof(DefaultModelRepository<,>).MakeGenericType(_dbContext.GetType(), _type);
            //var repo = Activator.CreateInstance(genericType);            
            var queryable = _dbContext.Query(_tableMetadata.AssemblyFullName);
            var id = context.GetArgument<string>("id");

            // Get filters
            string predicates = string.Empty;
            int paramsCount = -1;
            var filters = context.Arguments.Where(c => c.Key != "first" && c.Key != "offset");
            object[] args = new object[filters.Count()];
            

            foreach (var item in filters)
            {
                paramsCount++;
                if (!string.IsNullOrEmpty(predicates))
                {
                    predicates += " && ";
                }
                args[paramsCount] = item.Value;
                predicates += $"{item.Key.ToTitleCase()} == @{paramsCount}";

            }

            if (context.FieldName.Contains("_list"))
            {
                var first = context.Arguments["first"] != null ?
                    context.GetArgument("first", int.MaxValue) :
                    int.MaxValue; var offset = context.Arguments["offset"] != null ?
                    context.GetArgument("offset", 0) : 0;

                if (paramsCount >= 0)
                {
                    queryable = queryable.Where(predicates, args);
                }

                return queryable.Skip(offset).Take(first).ToDynamicList<object>();
            }
            else
            {
                return paramsCount >= 0 ? queryable.FirstOrDefault(predicates, args) : null;
            }
        }
    }
}
