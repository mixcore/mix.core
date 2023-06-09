using Microsoft.AspNetCore.Http;
using Mix.Shared.Models;
using System.Linq.Expressions;

namespace Mix.Lib.Models.Common
{
    public class SearchQueryModel<TEntity, TPrimaryKey>
         where TPrimaryKey : IComparable
        where TEntity : EntityBase<TPrimaryKey>
    {
        #region Properties

        public string Keyword { get; set; }
        public string Culture { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public MixContentStatus? Status { get; set; }
        public ExpressionMethod? SearchMethod { get; set; }
        public string Columns { get; set; }
        public string SearchColumns { get; set; }

        public int? MixTenantId { get; set; }
        public PagingRequestModel PagingData { get; set; } = new PagingRequestModel();
        public Expression<Func<TEntity, bool>> Predicate { get; set; }

        protected Expression<Func<TEntity, bool>> AndPredicate { get; set; }
        protected Expression<Func<TEntity, bool>> OrPredicate { get; set; }

        #endregion

        public SearchQueryModel(
            HttpRequest httpRequest,
            SearchRequestDto request = null,
            int? tenantId = default,
            Expression<Func<TEntity, bool>> andPredicate = null,
            Expression<Func<TEntity, bool>> orPredicate = null)
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
                    SortBy = request.OrderBy,
                    SortDirection = request.Direction
                };
                BuildAndPredicate(request, httpRequest);
            }
            else
            {
                PagingData = new(httpRequest);
            }

            OrPredicate = orPredicate;
            AndPredicate = AndPredicate.AndAlso(andPredicate);


            BuildPredicate();
        }

        protected void Init(HttpRequest request, int? tenantId, int defaultPageSize = 1000)
        {
            Culture = request.Query[MixRequestQueryKeywords.Specificulture];
            MixTenantId = tenantId;
            FromDate = DateTime.TryParse(request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate)
                ? fromDate : null;
            ToDate = DateTime.TryParse(request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate)
                ? toDate : null;
            Status = Enum.TryParse(request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status)
                ? status : null;
            Keyword = request.Query.TryGetValue(MixRequestQueryKeywords.Keyword, out var orderBy)
                ? orderBy : string.Empty;
            PagingData = new PagingRequestModel(request, defaultPageSize);
        }

        private void BuildPredicate()
        {
            Predicate = m => true;
            Predicate = Predicate.AndAlsoIf(Status != null, model => model.Status == Status);
            Predicate = Predicate.AndAlsoIf(FromDate != null, model => model.CreatedDateTime >= FromDate);
            Predicate = Predicate.AndAlsoIf(ToDate != null, model => model.CreatedDateTime <= ToDate);
            Predicate = Predicate.AndAlsoIf(AndPredicate != null, AndPredicate);
            Predicate = Predicate.OrIf(OrPredicate != null, OrPredicate);

        }

        protected virtual void BuildAndPredicate(SearchRequestDto req, HttpRequest request)
        {
            if (req.Culture != null)
            {
                AndPredicate = AndPredicate.AndAlso(ReflectionHelper.GetExpression<TEntity>(
                        MixRequestQueryKeywords.Specificulture, req.Culture, ExpressionMethod.Equal));
            }

            if (ReflectionHelper.HasProperty(typeof(TEntity), MixRequestQueryKeywords.TenantId))
            {
                AndPredicate = AndPredicate.AndAlsoIf(MixTenantId.HasValue,
                        ReflectionHelper.GetExpression<TEntity>(
                        MixRequestQueryKeywords.TenantId, MixTenantId.Value, ExpressionMethod.Equal));
            }

            if (!string.IsNullOrEmpty(req.SearchColumns) && !string.IsNullOrEmpty(req.Keyword) && req.SearchMethod.HasValue)
            {
                Expression<Func<TEntity, bool>> searchPredicate = m => false;
                foreach (var col in req.SearchColumns.Split(',', StringSplitOptions.TrimEntries))
                {
                    if (SearchMethod.Value == ExpressionMethod.In)
                    {

                    }
                    searchPredicate = searchPredicate.Or(ReflectionHelper.GetExpression<TEntity>(
                        col.ToTitleCase(), req.Keyword, req.SearchMethod.Value));
                }
                AndPredicate = AndPredicate.AndAlso(searchPredicate);
            }

        }

    }
}
