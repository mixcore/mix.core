using Microsoft.AspNetCore.Http;
using Mix.Lib.Extensions;
using Mix.Lib.Models;
using Mix.Lib.Models.Common;
using System.Linq.Expressions;

namespace Mix.Lib.Services
{
    public class MixPostService : IDisposable
    {
        protected ISession Session;
        private MixTenantSystemModel _currentTenant;
        protected MixTenantSystemModel CurrentTenant
        {
            get
            {
                if (_currentTenant == null)
                {
                    _currentTenant = Session.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant);
                }
                return _currentTenant;
            }
        }
        private UnitOfWorkInfo _uow;
        public MixPostService(UnitOfWorkInfo<MixCmsContext> cmsUOW, IHttpContextAccessor httpContextAccessor)
        {
            _uow = cmsUOW;
            Session = httpContextAccessor.HttpContext?.Session;
        }

        public void SetUnitOfWork(UnitOfWorkInfo uow)
        {
            if (uow != null)
            {
                _uow = uow;
            }
        }

        public async Task<PagingResponseModel<TView>> Search<TView>(
            SearchPostQueryModel searchRequest,
            string culture = null)
           where TView : ViewModelBase<MixCmsContext, MixPostContent, int, TView>
        {
            try
            {
                var associationRepo = MixDataContentAssociationViewModel.GetRepository(_uow);
                var valRepo = MixDataContentValueViewModel.GetRepository(_uow);
                var postRepo = new Repository<MixCmsContext, MixPostContent, int, TView>(_uow);

                var tasks = new List<Task<TView>>();
                culture ??= CurrentTenant.Configurations.DefaultCulture;
                Expression<Func<MixPostContent, bool>> andPredicate = m => m.Specificulture == culture;

                // Get all post data query
                var postDataContentIdsQuery = associationRepo.GetListQuery(m => m.ParentType == MixDatabaseParentType.Post);

                IQueryable<Guid> matchedCateDataContentIds = null;
                if (searchRequest.Categories != null && searchRequest.Categories.Count() > 0)
                {
                    // Get matched data Ids from searched categories
                    matchedCateDataContentIds = valRepo.GetListQuery(
                        m => m.MixDatabaseName == MixDatabaseNames.SYSTEM_CATEGORY
                            && searchRequest.Categories.Any(c => c == m.StringValue))
                        .Select(m => m.ParentId);

                    var postIdsByCate = postDataContentIdsQuery
                       .Where(m => matchedCateDataContentIds.Contains(m.DataContentId))
                       .Select(m => m.IntParentId);

                    andPredicate = andPredicate.AndAlso(m => postIdsByCate.Contains(m.Id));
                }

                if (searchRequest.Tags != null && searchRequest.Tags.Any())
                {
                    var postIdsByTag = postDataContentIdsQuery
                       .Where(m => matchedCateDataContentIds.Contains(m.DataContentId))
                       .Select(m => m.IntParentId);

                    andPredicate = andPredicate.AndAlso(m => postIdsByTag.Contains(m.Id));
                }

                searchRequest.Predicate = searchRequest.Predicate.AndAlso(andPredicate);

                var result = await postRepo.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
                return result;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
