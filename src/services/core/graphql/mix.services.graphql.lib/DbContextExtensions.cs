using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Mix.Services.Graphql.Lib
{
    public static class DbContextExtensions
    {
        public static IQueryable? Query(this DbContext context, string entityName)
        {
            var entityType = context.Model.FindEntityType(entityName)?.ClrType;
            return entityType != null ? context.Query(entityType) : default;
        }
        static readonly MethodInfo? SetMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), new Type[] { });
        public static IQueryable? Query(this DbContext context, Type entityType) =>
        (IQueryable?)SetMethod!.MakeGenericMethod(entityType).Invoke(context, null);
    }
}
