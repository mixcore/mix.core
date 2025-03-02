using Microsoft.AspNetCore.Http;
using Mix.Database.Entities.Account;
using Mix.Heart.Extensions;


using Mix.Shared.Models;
using System;
using System.Linq.Expressions;

namespace Mix.Identity.Models
{
    public class SearchAccountQueryModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public MixContentStatus? Status { get; set; }
        public string Keyword { get; set; }
        public PagingRequestModel PagingData { get; set; }
        public Expression<Func<MixUser, bool>> Predicate { get; set; }

        protected Expression<Func<MixUser, bool>> AndPredicate { get; set; }
        protected Expression<Func<MixUser, bool>> OrPredicate { get; set; }

        public SearchAccountQueryModel()
        {

        }

        public SearchAccountQueryModel(
            HttpRequest request,
            Expression<Func<MixUser, bool>> andPredicate = null,
            Expression<Func<MixUser, bool>> orPredicate = null)
        {
            AndPredicate = andPredicate;
            OrPredicate = orPredicate;
            Init(request, default);
        }

        public SearchAccountQueryModel(
            SearchRequestDto request,
            Expression<Func<MixUser, bool>> andPredicate = null,
            Expression<Func<MixUser, bool>> orPredicate = null)
        {
            AndPredicate = andPredicate;
            OrPredicate = orPredicate;

            FromDate = request.FromDate;
            ToDate = request.ToDate;
            Status = request.Status;
            Keyword = request.Keyword;
            PagingData = new PagingRequestModel(request);

            BuildPredicate();
        }

        private void Init(HttpRequest request, int defaultPageSize = 1000)
        {
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
            Predicate = Predicate.AndAlsoIf(FromDate != null, model => model.CreatedDateTime >= FromDate);
            Predicate = Predicate.AndAlsoIf(ToDate != null, model => model.CreatedDateTime <= ToDate);
            Predicate = Predicate.AndAlsoIf(AndPredicate != null, AndPredicate);
            Predicate = Predicate.OrIf(OrPredicate != null, OrPredicate);
        }
    }
}
