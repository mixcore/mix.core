using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Repository;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Services.Databases.Lib.Enums;
using Mix.Services.Databases.Lib.Services;
using System.Linq.Expressions;

namespace Mixcore.Domain.Services
{
    public sealed class PostService : TenantServiceBase
    {
        private readonly UnitOfWorkInfo<MixCmsContext> _uow;
        private readonly MixMetadataService _metadataService;

        public PostService(IHttpContextAccessor httpContextAccessor, UnitOfWorkInfo<MixCmsContext> uow, MixMetadataService metadataService) : base(httpContextAccessor)
        {
            _uow = uow;
            _metadataService = metadataService;
        }

        public async Task<PagingResponseModel<PostContentViewModel>> Search(HttpRequest httpRequest)
        {
            return await SearchPosts(new(httpRequest));
        }

        public async Task<PagingResponseModel<PostContentViewModel>> SearchPosts(SearchPostQueryModel searchRequest)
        {
            try
            {
                using var postRepo = new Repository<MixCmsContext, MixPostContent, int, PostContentViewModel>(_uow);
                searchRequest.Predicate = searchRequest.Predicate.AndAlso(ApplyMetadataQueries(searchRequest));
                var result = await postRepo.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
                return result;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        private Expression<Func<MixPostContent, bool>> ApplyMetadataQueries(SearchPostQueryModel searchRequest)
        {
            Expression<Func<MixPostContent, bool>> predicate = m => true;
            if (searchRequest.MetadataQueries.Count == 0)
            {
                return predicate;
            }

            IQueryable<int> allowedIdQuery = default;
            foreach (var item in searchRequest.MetadataQueries)
            {
                var query = _metadataService.GetQueryableContentIdByMetadataSeoContent(item.Value, MetadataParentType.Post);
                allowedIdQuery = allowedIdQuery == default ?
                    query
                    : allowedIdQuery.Union(query);
            }
            var allowedIds = allowedIdQuery.ToList();
            predicate = predicate.AndAlsoIf(allowedIdQuery != null, m => allowedIds.Contains(m.Id));
            return predicate;
        }
    }
}
