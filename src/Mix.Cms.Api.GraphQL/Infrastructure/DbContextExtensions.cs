using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mix.Cms.Api.GraphQL.Infrastructure
{
    public static class DbContextExtensions
    {
        public static IQueryable Query(this DbContext context, string entityName) =>
         context.Query(context.Model.FindEntityType(entityName).ClrType); 
        static readonly MethodInfo SetMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set)); 
        public static IQueryable Query(this DbContext context, Type entityType) =>
        (IQueryable)SetMethod.MakeGenericMethod(entityType).Invoke(context, null);
    }
}
