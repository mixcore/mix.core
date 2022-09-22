using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Lib.Extensions;
using Mix.Lib.Models.Common;
using System.Linq.Expressions;

namespace Mix.Lib.Services
{
    public class MixPostService : IDisposable
    {
        protected ISession _session;
        private MixTenantSystemViewModel _currentTenant;
        protected MixTenantSystemViewModel CurrentTenant
        {
            get
            {
                if (_currentTenant == null)
                {
                    _currentTenant = _session.Get<MixTenantSystemViewModel>(MixRequestQueryKeywords.Tenant);
                }
                return _currentTenant;
            }
        }
        private UnitOfWorkInfo _uow;
        public MixPostService(UnitOfWorkInfo<MixCmsContext> cmsUOW, IHttpContextAccessor httpContextAccessor)
        {
            _uow = cmsUOW;
            _session = httpContextAccessor.HttpContext.Session;
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
                var _associationRepo = MixDataContentAssociationViewModel.GetRepository(_uow);
                var _valRepo = MixDataContentValueViewModel.GetRepository(_uow);
                var _postRepo = new Repository<MixCmsContext, MixPostContent, int, TView>(_uow);

                var tasks = new List<Task<TView>>();
                culture ??= CurrentTenant.Configurations.DefaultCulture;
                Expression<Func<MixPostContent, bool>> andPredicate = m => m.Specificulture == culture;

                // Get all post data query
                var postDataContentIdsQuery = _associationRepo.GetListQuery(m => m.ParentType == MixDatabaseParentType.Post);

                IQueryable<Guid> matchedCateDataContentIds = null;
                if (searchRequest.Categories != null && searchRequest.Categories.Count() > 0)
                {
                    // Get matched data Ids from searched categories
                    matchedCateDataContentIds = _valRepo.GetListQuery(
                        m => m.MixDatabaseName == MixDatabaseNames.SYSTEM_CATEGORY
                            && searchRequest.Categories.Any(c => c == m.StringValue))
                        .Select(m => m.ParentId);

                    var postIdsByCate = postDataContentIdsQuery
                       .Where(m => matchedCateDataContentIds.Contains(m.DataContentId))
                       .Select(m => m.IntParentId);

                    andPredicate = andPredicate.AndAlso(m => postIdsByCate.Contains(m.Id));
                }

                if (searchRequest.Tags != null && searchRequest.Tags.Count() > 0)
                {
                    // Get matched data Ids from searched tags
                    var matchedTagDataContentIds = _valRepo.GetListQuery(
                    m => m.MixDatabaseName == MixDatabaseNames.SYSTEM_TAG
                        && searchRequest.Tags.Any(c => c == m.StringValue))
                    .Select(m => m.ParentId);

                    var postIdsByTag = postDataContentIdsQuery
                       .Where(m => matchedCateDataContentIds.Contains(m.DataContentId))
                       .Select(m => m.IntParentId);

                    andPredicate = andPredicate.AndAlso(m => postIdsByTag.Contains(m.Id));
                }

                searchRequest.Predicate = searchRequest.Predicate.AndAlso(andPredicate);

                var result = await _postRepo.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
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
