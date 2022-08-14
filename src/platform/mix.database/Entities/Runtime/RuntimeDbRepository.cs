using Mix.Heart.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore.DynamicLinq;

namespace Mix.Database.Entities.Runtime
{
    public class RuntimeDbRepository
    {
        private DbContext _dbContext;
        private string _entityName;
        private IQueryable _query;
        public RuntimeDbRepository(DbContext dbContext, string tableName)
        {
            _dbContext = dbContext;
            _entityName = $"{tableName.ToTitleCase()}";
            _query = _dbContext.Query(_entityName);
        }

        public List<dynamic> GetAll()
        {
            return _query.ToDynamicList();
        }

        public async Task<dynamic> GetSingleByParent(int parentId)
        {
            return parentId > 0 ? await _query.FirstOrDefaultAsync($"parentId = {parentId}")
                    : default;
        }

        public dynamic GetSingleBy(int id)
        {
            return _query.FirstOrDefault($"id = {id}");
        }
    }

    public static class DynamicContextExtensions
    {
        public static IQueryable Query(this DbContext context, string entityName) =>
            context.Query(context.Model.FindEntityType(entityName).ClrType);

        static readonly MethodInfo SetMethod =
            typeof(DbContext).GetMethod(nameof(DbContext.Set), 1, Array.Empty<Type>()) ??
            throw new Exception($"Type not found: DbContext.Set");

        public static IQueryable Query(this DbContext context, Type entityType) =>
            (IQueryable)SetMethod.MakeGenericMethod(entityType)?.Invoke(context, null) ??
            throw new Exception($"Type not found: {entityType.FullName}");
    }
}
