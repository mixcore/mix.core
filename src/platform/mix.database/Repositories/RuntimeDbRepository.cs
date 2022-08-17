using Mix.Heart.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using Mix.Heart.Services;
using Mix.Heart.Helpers;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using System.Linq.Expressions;
using Mix.Heart.Models;
using Mix.Heart.Exceptions;

namespace Mix.Database.Repositories
{
    public class RuntimeDbRepository: RepositoryBase<DbContext>
    {
        #region Properties

        private DbContext _dbContext;
        private string _entityName;
        private Type _entityType;
        private IQueryable Table;

        protected MixCacheService CacheService { get; set; }

        public string CacheFilename { get; protected set; } = "full";

        public string[] SelectedMembers { get; protected set; }

        protected string[] KeyMembers { get { return ReflectionHelper.GetKeyMembers(Context, _entityType); } }

        #endregion

        #region Contructors

        public RuntimeDbRepository(DbContext dbContext, string tableName) : base(dbContext) {
            _dbContext = dbContext;
            _entityName = tableName.ToTitleCase();
            _entityType = _dbContext.Model.FindEntityType(_entityName).ClrType;
            Table = _dbContext.Query(_entityName);
        }

        public RuntimeDbRepository()
        {
            SelectedMembers = FilterSelectedFields();
        }

        public RuntimeDbRepository(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
            SelectedMembers = FilterSelectedFields();
        }
        #endregion

        #region IQueryable

        public IQueryable GetListQuery(LambdaExpression predicate)
        {
            return Table.Where(predicate);
        }

        public IQueryable GetPagingQuery(LambdaExpression predicate,
                       PagingModel paging)
        {
            var query = GetListQuery(predicate);
            paging.Total = query.Count();
            dynamic sortBy = GetLambda(paging.SortBy);

            switch (paging.SortDirection)
            {
                case SortDirection.Asc:
                    query = Queryable.OrderBy(query, sortBy);
                    break;
                case SortDirection.Desc:
                    query = Queryable.OrderByDescending(query, sortBy);
                    break;
            }

            if (paging.PageSize.HasValue)
            {
                query = query.Skip(paging.PageIndex * paging.PageSize.Value).Take(paging.PageSize.Value);
            }

            return query;
        }

        #endregion

        #region Queries Async
        public virtual bool CheckIsExists(int id)
        {
            return Table.Any($"id = {id}");
        }

        public virtual bool CheckIsExists(LambdaExpression predicate)
        {
            return Table.Any(predicate);
        }
        public async Task<PagingResponseModel<dynamic>> GetPagingEntitiesAsync(LambdaExpression predicate,
                      PagingModel paging)
        {
            var source = GetPagingQuery(predicate, paging);
            return await ToPagingEntityAsync(source, paging);
        }

        public async Task<dynamic> GetSingleAsync(LambdaExpression predicate)
        {
            return await Table.Where(predicate).SingleOrDefaultAsync();
        }

        public virtual async Task<dynamic> GetByIdAsync(int id)
        {
            return await Table.SingleOrDefaultAsync($"id = {id}");
        }

        public virtual int MaxAsync(LambdaExpression predicate)
        {
            return (int)Table.Max(predicate);
        }

        public Task<dynamic> GetFirstAsync(LambdaExpression predicate)
        {
            return Table.Where(predicate).FirstOrDefaultAsync();
        }

        #endregion

        #region Helpers

        
        protected async Task<PagingResponseModel<dynamic>> ToPagingEntityAsync(
          IQueryable source,
          PagingModel pagingData)
        {
            try
            {
                var entities = await source.ToDynamicListAsync();

                return new PagingResponseModel<dynamic>(entities, pagingData);
            }
            catch (Exception ex)
            {
                throw new MixException(ex.Message);
            }
        }

        protected LambdaExpression GetLambda(string propName, bool isGetDefault = true)
        {
            var parameter = Expression.Parameter(_entityType);
            var prop = Array.Find(_entityType.GetProperties(), p => p.Name == propName);
            if (prop == null && isGetDefault)
            {
                propName = "Id";
            }
            var memberExpression = Expression.Property(parameter, propName);
            return Expression.Lambda(memberExpression, parameter);
        }


        private string[] FilterSelectedFields()
        {
            var properties = _entityType.GetProperties();
            return properties.Where(p => p.PropertyType.IsSerializable).Select(p => p.Name).ToArray();
        }
        #endregion
    }

    #region Extensions

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
    #endregion
}
