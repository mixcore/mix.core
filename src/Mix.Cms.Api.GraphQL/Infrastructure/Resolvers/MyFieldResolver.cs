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
using System.Linq.Expressions;
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
            _tableMetadata = tableMetadata;
            _dbContext = dbContext;
        }
        public object Resolve(ResolveFieldContext context)
        {
            var queryable = _dbContext.Query(_tableMetadata.AssemblyFullName);

            // Get filters
            string predicates = string.Empty;
            int paramsCount = -1;
            var filters = context.Arguments.Where(c => c.Key != "first" && c.Key != "offset");
            object[] args = new object[filters.Count()];
            LambdaExpression lamda = null;

            foreach (var item in filters)
            {
                paramsCount++;
                if (!string.IsNullOrEmpty(predicates))
                {
                    predicates += " and ";
                }
                args[paramsCount] = item.Value;
                predicates += $"{item.Key.ToTitleCase()} == @{paramsCount}";

            }

            if (context.FieldName.Contains("_list"))
            {
                var first = context.Arguments["first"] != null ?
                    context.GetArgument("first", int.MaxValue) : int.MaxValue; 
                var offset = context.Arguments["offset"] != null ?
                    context.GetArgument("offset", 0) : 0;

                if (paramsCount >= 0)
                {
                    
                    queryable = queryable.Where(lamda);
                }

                return queryable.Skip(offset).Take(first).ToDynamicList<object>();
            }
            else
            {
                return paramsCount >= 0 ? queryable.FirstOrDefault(predicates, args) : null;
            }
        }
        protected LambdaExpression GetLambda(string propName, bool isGetDefault = false)
        {
            var parameter = Expression.Parameter(_type);
            var prop = Array.Find(_type.GetProperties(), p => p.Name == propName);
            if (prop == null && isGetDefault)
            {
                propName = _type.GetProperties().FirstOrDefault()?.Name;
            }
            var memberExpression = Expression.Property(parameter, propName);
            return Expression.Lambda(memberExpression, parameter);
        }
    }
}
