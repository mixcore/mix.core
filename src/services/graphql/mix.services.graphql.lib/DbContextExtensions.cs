using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Mix.Services.Graphql.Lib
{
    public static class DbContextExtensions
    {
        public static IQueryable? Query(this DbContext context, string entityName) =>
         context.Query(context.Model.FindEntityType(entityName).ClrType);
        static readonly MethodInfo? SetMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), new Type[] { });
        public static IQueryable? Query(this DbContext context, Type entityType) =>
        (IQueryable?)SetMethod!.MakeGenericMethod(entityType).Invoke(context, null);
    }
}
