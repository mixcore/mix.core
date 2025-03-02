using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mix.Mixdb.Helpers;
using Mix.RepoDb.Helpers;
using Mix.Shared.Models;
using System.Linq.Expressions;

namespace Mix.Lib.Models.Common
{
    public class SearchQueryModel<TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Properties

        public string Keyword { get; set; }

        public string Culture { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public MixContentStatus? Status { get; set; }

        public MixCompareOperator? SearchMethod { get; set; }

        public string Columns { get; set; }

        public string SearchColumns { get; set; }

        public int? TenantId { get; set; }

        public PagingRequestModel PagingData { get; set; } = new PagingRequestModel();

        public Expression<Func<TEntity, bool>> Predicate { get; set; }

        protected Expression<Func<TEntity, bool>> AndPredicate { get; set; }

        #endregion

        public SearchQueryModel(
            HttpRequest httpRequest,
            SearchRequestDto request = null,
            int? tenantId = default,
            Expression<Func<TEntity, bool>> andPredicate = null)
        {
            Init(httpRequest, tenantId);

            if (request != null)
            {
                ReflectionHelper.Map(request, this);

                PagingData = new PagingRequestModel()
                {
                    Page = request.PageIndex + 1,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    SortByColumns = new List<Heart.Model.MixSortByColumn>()
                    {
                        new Heart.Model.MixSortByColumn(request.SortBy, request.Direction)
                    }
                };
                BuildAndPredicate(request, httpRequest);
            }
            else
            {
                PagingData = new(httpRequest);
            }

            AndPredicate = AndPredicate.AndAlso(andPredicate);


            BuildPredicate();
        }

        public SearchQueryModel(
            HttpRequest httpRequest,
            Dictionary<string, string> propertyMapping = null,
            SearchRequestDto request = null,
            int? tenantId = default,
            Expression<Func<TEntity, bool>> andPredicate = null)
        {
            Init(httpRequest, tenantId);

            if (request != null)
            {
                ReflectionHelper.Map(request, this);

                PagingData = new PagingRequestModel()
                {
                    Page = request.PageIndex + 1,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    SortByColumns = new List<Heart.Model.MixSortByColumn>()
                    {
                        new Heart.Model.MixSortByColumn(request.SortBy, request.Direction)
                    }
                };
                BuildAndPredicate(request, httpRequest, propertyMapping);
            }
            else
            {
                PagingData = new(httpRequest);
            }

            AndPredicate = AndPredicate.AndAlso(andPredicate);


            BuildPredicate();
        }

        protected void Init(HttpRequest request, int? tenantId, int defaultPageSize = 1000)
        {
            Culture = request.Query[MixRequestQueryKeywords.Specificulture];
            TenantId = tenantId;
            FromDate = DateTime.TryParse(request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate)
                ? fromDate
                : null;
            ToDate = DateTime.TryParse(request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate)
                ? toDate
                : null;
            Status = Enum.TryParse(request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status)
                ? status
                : null;
            Keyword = request.Query.TryGetValue(MixRequestQueryKeywords.Keyword, out var orderBy)
                ? orderBy
                : string.Empty;
            PagingData = new PagingRequestModel(request, defaultPageSize);
        }

        private bool CheckEntityStatus(TEntity model, MixContentStatus? status)
        {
            if (model is EntityBase<TPrimaryKey> entityBase)
            {
                return entityBase.Status == status;
            }

            return true;
        }

        private bool IsGreaterThanOrEqualDate(TEntity model, DateTime? dateTime)
        {
            if (model is EntityBase<TPrimaryKey> entityBase)
            {
                return entityBase.CreatedDateTime >= dateTime;
            }

            return true;
        }

        private bool IsLessThanOrEqualDate(TEntity model, DateTime? dateTime)
        {
            if (model is EntityBase<TPrimaryKey> entityBase)
            {
                return entityBase.CreatedDateTime <= dateTime;
            }

            return true;
        }

        private void BuildPredicate()
        {
            Predicate = m => true;
            Predicate = Predicate.AndAlsoIf(FromDate != null, model => IsGreaterThanOrEqualDate(model, FromDate));
            Predicate = Predicate.AndAlsoIf(ToDate != null, model => IsLessThanOrEqualDate(model, ToDate));
            Predicate = Predicate.AndAlsoIf(AndPredicate != null, AndPredicate);
        }

        protected virtual void BuildAndPredicate(
            SearchRequestDto searchRequest,
            HttpRequest request,
            Dictionary<string, string> columnMapping = null)
        {
            if (searchRequest.Culture != null)
            {
                var specificulturePropertyName = columnMapping is not null
                    ? columnMapping[MixRequestQueryKeywords.Specificulture]
                    : MixRequestQueryKeywords.Specificulture;

                AndPredicate = AndPredicate.AndAlso(ReflectionHelper.GetExpression<TEntity>(
                    specificulturePropertyName, searchRequest.Culture, ExpressionMethod.Equal));
            }

            if (ReflectionHelper.HasProperty(typeof(TEntity), MixRequestQueryKeywords.TenantId))
            {
                var tenantIdPropertyName = columnMapping is not null
                    ? columnMapping[MixRequestQueryKeywords.TenantId]
                    : MixRequestQueryKeywords.TenantId;

                AndPredicate = AndPredicate.AndAlsoIf(TenantId.HasValue,
                    ReflectionHelper.GetExpression<TEntity>(
                        tenantIdPropertyName,
                        TenantId.Value,
                        ExpressionMethod.Equal));
            }

            if (!string.IsNullOrEmpty(searchRequest.SearchColumns)
                && !string.IsNullOrEmpty(searchRequest.Keyword)
                && searchRequest.SearchMethod.HasValue)
            {
                Expression<Func<TEntity, bool>> searchPredicate = m => false;
                foreach (var columnName in searchRequest.SearchColumns.Split(',', StringSplitOptions.TrimEntries))
                {
                    var propertyName = columnMapping is not null
                        ? columnMapping[columnName]
                        : columnName.ToTitleCase();

                    searchPredicate = searchPredicate.Or(ReflectionHelper.GetExpression<TEntity>(
                        propertyName,
                        searchRequest.Keyword,
                        MixCmsHelper.ParseExpressionMethod(searchRequest.SearchMethod)));
                }

                AndPredicate = AndPredicate.AndAlso(searchPredicate);
            }

            if (searchRequest.Queries != null && searchRequest.Queries.Count > 0)
            {
                Expression<Func<TEntity, bool>> queriesPred = null;
                foreach (var query in searchRequest.Queries)
                {
                    var propertyName = columnMapping is not null
                       ? columnMapping[query.FieldName]
                       : query.FieldName;

                    var pre = ReflectionHelper.GetExpression<TEntity>(propertyName, query.Value, ParseOperator(query.CompareOperator));
                    if (searchRequest.Conjunction == MixConjunction.And)
                    {
                        queriesPred = queriesPred.AndAlso(pre);
                    }
                    if (searchRequest.Conjunction == MixConjunction.Or)
                    {
                        queriesPred = queriesPred.Or(pre);
                    }
                }
                AndPredicate = AndPredicate.AndAlso(queriesPred);
            }
        }

        private object ParseSearchKeyword(MixCompareOperator searchMethod, object keyword, MixDataType? dataType = MixDataType.String)
        {
            if (keyword == null)
            {
                return keyword;
            }
            switch (searchMethod)
            {
                case MixCompareOperator.Like:
                case MixCompareOperator.ILike:
                    return $"%{keyword}%";
                case MixCompareOperator.InRange:
                case MixCompareOperator.NotInRange:
                    var arr = keyword.ToString().Split(',', StringSplitOptions.TrimEntries);
                    if (dataType != MixDataType.String)
                    {
                        List<object> result = [];
                        foreach (var item in arr)
                        {
                            result.Add(MixDbHelper.ParseObjectValue(dataType, item));
                        }
                        return result;
                    }
                    return arr;
                default:
                    return MixDbHelper.ParseObjectValue(dataType, keyword);
            }
        }

        private ExpressionMethod ParseOperator(MixCompareOperator compareOperator)
        {
            switch (compareOperator)
            {
                case MixCompareOperator.Equal:
                    return ExpressionMethod.Equal;
                case MixCompareOperator.Like:
                case MixCompareOperator.ILike:
                    return ExpressionMethod.Like;
                case MixCompareOperator.NotEqual:
                    return ExpressionMethod.NotEqual;
                case MixCompareOperator.Contain:
                    return ExpressionMethod.In;
                case MixCompareOperator.NotContain:
                    return ExpressionMethod.NotIn;
                case MixCompareOperator.InRange:
                    return ExpressionMethod.In;
                case MixCompareOperator.NotInRange:
                    return ExpressionMethod.NotIn;
                case MixCompareOperator.GreaterThanOrEqual:
                    return ExpressionMethod.GreaterThanOrEqual;
                case MixCompareOperator.GreaterThan:
                    return ExpressionMethod.GreaterThan;
                case MixCompareOperator.LessThanOrEqual:
                    return ExpressionMethod.LessThanOrEqual;
                case MixCompareOperator.LessThan:
                    return ExpressionMethod.LessThan;
                default:
                    return ExpressionMethod.Equal;
            }
        }
    }
}
