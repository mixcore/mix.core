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
using Newtonsoft.Json.Linq;
using Mix.Heart.Infrastructure.Exceptions;

namespace Mix.Database.Repositories
{
    public class RuntimeDbRepository : RepositoryBase<DbContext>
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

        public RuntimeDbRepository(DbContext dbContext, string tableName) : base(dbContext)
        {
            _dbContext = dbContext;
            _entityName = tableName.ToLower().ToTitleCase();
            _entityType = _dbContext.Model.FindEntityType(_entityName).ClrType;
            CacheService = new();
            Table = _dbContext.Query(_entityName);
            SelectedMembers = FilterSelectedFields();
        }
        #endregion

        #region IQueryable

        public IQueryable GetListQuery(string queries)
        {
            return string.IsNullOrEmpty(queries) ? Table : Table.Where(queries);
        }

        public IQueryable GetPagingQuery(List<SearchQueryField> searchFields,
                       PagingModel paging)
        {
            IQueryable query = Table;
            foreach (var field in searchFields)
            {
                query = query.ApplyQueryField(field);
            }
            return GetPagingQuery(query, paging);
        }

        public IQueryable GetPagingQuery(string queryString,
                   PagingModel paging)
        {
            var query = GetListQuery(queryString);
            return GetPagingQuery(query, paging);
        }

        public IQueryable GetPagingQuery(IQueryable query,
                   PagingModel paging)
        {
            paging.Total = query.Count();
            query = query.OrderBy($"{paging.SortBy} {paging.SortDirection}");

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
        public async Task<PagingResponseModel<JObject>> GetPagingEntitiesAsync(List<SearchQueryField> searchFields,
                      PagingModel paging)
        {
            var source = GetPagingQuery(searchFields, paging);
            return await ToPagingEntityAsync(source, paging);
        }

        public async Task<JObject> GetSingleAsync(LambdaExpression predicate)
        {
            var obj = await Table.Where(predicate).SingleOrDefaultAsync();
            return obj != null ? ReflectionHelper.ParseObject(obj) : null;
        }

        public async Task<JObject> GetFirstAsync(LambdaExpression predicate)
        {
            var obj = await Table.Where(predicate).FirstOrDefaultAsync();
            return obj != null ? ReflectionHelper.ParseObject(obj) : null;
        }

        public virtual async Task<JObject> GetSingleAsync(int id)
        {
            JObject result = null;
            if (CacheService != null && CacheService.IsCacheEnabled)
            {
                result = await CacheService.GetAsync<JObject>($"{id}/{_entityType.FullName}", _entityType, CacheFilename);
                if (result != null)
                {
                    return result;
                }
            }

            var obj = await Table.SingleOrDefaultAsync($"id = {id}");
            if (obj != null)
            {
                result = ReflectionHelper.ParseObject(obj);
                if (result != null && CacheService != null)
                {
                    if (CacheFilename == "full")
                    {
                        await CacheService.SetAsync($"{obj.Id}/{_entityType.FullName}", result, _entityType, CacheFilename);
                    }
                    else
                    {
                        result = ReflectionHelper.GetMembers(obj, SelectedMembers);
                        await CacheService.SetAsync($"{obj.Id}/{_entityType.FullName}", obj, _entityType, CacheFilename);
                    }
                }
            }
            return result;
        }

        public virtual int MaxAsync(LambdaExpression predicate)
        {
            return (int)Table.Max(predicate);
        }

        #endregion

        #region Commands Async

        public virtual async Task<object> CreateAsync(JObject model)
        {
            try
            {
                BeginUow();
                var entity = model.ToObject(_entityType);
                Context.Entry(entity).State = EntityState.Added;
                await Context.SaveChangesAsync();
                await CompleteUowAsync();
                return entity;
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex);
                return null;
            }
            finally
            {
                await CloseUowAsync();
            }
        }

        public virtual async Task<object> UpdateAsync(JObject model)
        {
            try
            {
                BeginUow();
                var id = model.Value<int>("id");
                if (!CheckIsExists(id))
                {
                    await HandleExceptionAsync(new EntityNotFoundException(id.ToString()));
                    return null;
                }

                var entity = model.ToObject(_entityType);
                Context.Entry(entity).State = EntityState.Modified;
                await Context.SaveChangesAsync();
                await CompleteUowAsync();
                await CacheService.RemoveCacheAsync(id, _entityType);
                return entity;
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex);
                return null;
            }
            finally
            {
                await CloseUowAsync();
            }
        }

        public virtual async Task SaveAsync(JObject entity)
        {
            try
            {
                BeginUow();
                var id = entity.Value<int>("id");
                if (CheckIsExists(id))
                {
                    await UpdateAsync(entity);
                }
                else { await CreateAsync(entity); }
                await CompleteUowAsync();
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex);
            }
            finally
            {
                await CloseUowAsync();
            }
        }

        public virtual async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await GetSingleAsync(id);
                await DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex);
            }
            finally
            {
                await CloseUowAsync();
            }
        }

        public virtual async Task DeleteAsync(LambdaExpression predicate)
        {
            try
            {
                BeginUow();
                var entity = Table.Single(predicate);
                if (entity == null)
                {
                    await HandleExceptionAsync(new EntityNotFoundException());
                    return;
                }

                Context.Entry(entity).State = EntityState.Deleted;
                await Context.SaveChangesAsync();
                await CompleteUowAsync();
                await CacheService.RemoveCacheAsync(entity.Id, _entityType);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex);
            }
            finally
            {
                await CloseUowAsync();
            }
        }

        public virtual async Task DeleteManyAsync(LambdaExpression predicate)
        {
            try
            {
                var entities = Table.Where(predicate);
                foreach (var entity in entities)
                {
                    await DeleteAsync(ReflectionHelper.ParseObject(entity));
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex);
            }
            finally
            {
                await CloseUowAsync();
            }
        }

        public virtual async Task DeleteAsync(JObject model)
        {
            try
            {
                BeginUow();
                var id = model.Value<int>("id");
                if (!CheckIsExists(id))
                {
                    await HandleExceptionAsync(new EntityNotFoundException(id.ToString()));
                    return;
                }

                Context.Entry(model.ToObject(_entityType)).State = EntityState.Deleted;
                await Context.SaveChangesAsync();
                await CompleteUowAsync();
                await CacheService.RemoveCacheAsync(id, _entityType);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex);
            }
            finally
            {
                await CloseUowAsync();
            }
        }
        #endregion

        #region Helpers


        protected async Task<PagingResponseModel<JObject>> ToPagingEntityAsync(
          IQueryable source,
          PagingModel pagingData)
        {
            try
            {
                var entities = await GetEntities(source);
                List<JObject> data = await ParseEntitiesAsync(entities);
                return new PagingResponseModel<JObject>(data, pagingData);
            }
            catch (Exception ex)
            {
                throw new MixException(ex.Message);
            }
        }

        protected async Task<List<dynamic>> GetEntities(IQueryable source)
        {
            return await source.ToDynamicListAsync();
        }

        protected async Task<List<JObject>> ParseEntitiesAsync(List<dynamic> entities)
        {
            List<JObject> data = new();

            foreach (var entity in entities)
            {
                var view = await GetSingleAsync(entity.Id);
                data.Add(view);
            }
            return data;
        }

        public LambdaExpression GetLambda(string propName, bool isGetDefault = true)
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
        public static IQueryable ApplyQueryField(this IQueryable query, SearchQueryField field)
        {
            return field.CompareOperator switch
            {
                MixCompareOperator.Contain => query.Where($"{field.FieldName}.Contains(@0)", field.Value),
                MixCompareOperator.Equal => query.Where($"{field.FieldName} = @0", field.Value),
                MixCompareOperator.Like => field.Value!=null && field.FieldName!=null ? query.Where($"{field.FieldName}.Contains(@0)", field.Value): query,
                MixCompareOperator.NotEqual => query.Where($"{field.FieldName} <> @0", field.Value),
                MixCompareOperator.NotContain => query.Where($"{field.FieldName} = @0", field.Value),
                MixCompareOperator.GreaterThanOrEqual => query.Where($"{field.FieldName} >= @0", field.Value),
                MixCompareOperator.GreaterThan => query.Where($"{field.FieldName} > @0", field.Value),
                MixCompareOperator.LessThanOrEqual => query.Where($"{field.FieldName} <= @0", field.Value),
                MixCompareOperator.LessThan => query.Where($"{field.FieldName} < @0", field.Value),
                MixCompareOperator.NotInRange => query,
                MixCompareOperator.InRange => query,
                _ => query
            };
        }
        public static string AndAlso(this string queryString, string query)
        {
            queryString = string.IsNullOrEmpty(queryString) ? query : $"{queryString} And ({query})";
            return queryString;
        }
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
