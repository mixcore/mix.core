using Microsoft.AspNetCore.Http;
using Mix.Shared.Models;
using System.Linq.Expressions;

namespace Mix.Lib.Models.Common
{
    public class SearchQueryModel<TEntity, TPrimaryKey>
         where TPrimaryKey : IComparable
        where TEntity : EntityBase<TPrimaryKey>
    {
        public int MixTenantId { get; set; }
        public string Specificulture { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public MixContentStatus? Status { get; set; }
        public string Keyword { get; set; }
        public PagingRequestModel PagingData { get; set; }
        public Expression<Func<TEntity, bool>> Predicate { get; set; }

        protected Expression<Func<TEntity, bool>> AndPredicate { get; set; }
        protected Expression<Func<TEntity, bool>> OrPredicate { get; set; }

        public SearchQueryModel(int tenantId)
        {
            MixTenantId = tenantId;
        }

        public SearchQueryModel(
            int tenantId,
            HttpRequest request,
            Expression<Func<TEntity, bool>> andPredicate = null,
            Expression<Func<TEntity, bool>> orPredicate = null)
        {
            MixTenantId = tenantId;
            AndPredicate = AndPredicate.AndAlso(andPredicate);
            OrPredicate = orPredicate;
            Init(request, default);
        }

        public SearchQueryModel(
            int tenantId,
            SearchRequestDto request,
            HttpRequest httpRequest,
            Expression<Func<TEntity, bool>> andPredicate = null,
            Expression<Func<TEntity, bool>> orPredicate = null)
        {
            MixTenantId = tenantId;
            AndPredicate = AndPredicate.AndAlso(andPredicate);
            OrPredicate = orPredicate;

            Specificulture = request.Culture;
            FromDate = request.FromDate;
            ToDate = request.ToDate;
            Status = request.Status;
            Keyword = request.Keyword;
            PagingData = new PagingRequestModel()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                SortDirection = request.Direction,
                SortBy = request.OrderBy
            };

            BuildAndPredicate(request, httpRequest);
            BuildPredicate();
        }

        private void Init(HttpRequest request, int defaultPageSize = 1000)
        {
            Specificulture = request.Query[MixRequestQueryKeywords.Specificulture];
            FromDate = DateTime.TryParse(request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate)
                ? fromDate : null;
            ToDate = DateTime.TryParse(request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate)
                ? toDate : null;
            Status = Enum.TryParse(request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status)
                ? status : null;
            Keyword = request.Query.TryGetValue(MixRequestQueryKeywords.Keyword, out var orderBy)
                ? orderBy : string.Empty;
            PagingData = new PagingRequestModel(request, defaultPageSize);

            BuildPredicate();
            
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
                        MixRequestQueryKeywords.Specificulture, req.Culture, Heart.Enums.ExpressionMethod.Eq));
            }

            if (ReflectionHelper.HasProperty(typeof(TEntity), MixRequestQueryKeywords.MixTenantId))
            {
                AndPredicate = ReflectionHelper.GetExpression<TEntity>(
                        MixRequestQueryKeywords.MixTenantId, MixTenantId, ExpressionMethod.Eq);
            }

            if (!string.IsNullOrEmpty(req.SearchColumns) && !string.IsNullOrEmpty(req.Keyword) && req.SearchMethod.HasValue)
            {
                Expression<Func<TEntity, bool>> searchPredicate = m => false;
                foreach (var col in req.SearchColumns.Replace(" ", string.Empty).Split(','))
                {
                    searchPredicate = searchPredicate.Or(ReflectionHelper.GetExpression<TEntity>(
                        col.ToTitleCase(), req.Keyword, req.SearchMethod.Value));
                }
                AndPredicate = AndPredicate.AndAlso(searchPredicate);
            }

        }

    }
}
