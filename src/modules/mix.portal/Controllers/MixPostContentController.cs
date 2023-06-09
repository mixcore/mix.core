using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Helpers;
using Mix.Portal.Domain.Services;
using Mix.RepoDb.Repositories;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-post-content")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixPostContentController
        : MixBaseContentController<MixPostContentViewModel, MixPostContent, int>
    {
        private readonly PortalPostService _postService;
        private readonly MixRepoDbRepository _mixRepoDbRepository;
        public MixPostContentController(
            MixIdentityService identityService,
            TenantUserManager userManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IQueueService<MessageQueueModel> queueService,
            MixRepoDbRepository mixRepoDbRepository,
            PortalPostService postService)
            : base(MixContentType.Post, identityService, userManager, httpContextAccessor, configuration, cacheService, translator,
                  mixIdentityService, cmsUow, queueService)
        {
            _mixRepoDbRepository = mixRepoDbRepository;
            _postService = postService;
        }

        #region Routes

        [HttpPost("filter")]
        public async Task<ActionResult<PagingResponseModel<MixPostContentViewModel>>> Filter([FromBody] FilterContentRequestDto req)
        {
            var searchRequest = BuildSearchRequest(req);
            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                !string.IsNullOrEmpty(req.MixDatabaseName), m => m.MixDatabaseName == req.MixDatabaseName);
            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                req.MetadataQueries.Count > 0, _postService.ParseMetadataQueriesPredicate(req.MetadataQueries));

            if (!string.IsNullOrEmpty(req.MixDatabaseName) && req.Queries.Count > 0)
            {
                _mixRepoDbRepository.InitTableName(req.MixDatabaseName);
                var listData = await _mixRepoDbRepository.GetListByAsync(req.Queries, "id, parentId");
                if (listData != null)
                {
                    List<int> allowIds = new();
                    foreach (var data in listData)
                    {
                        allowIds.Add(ReflectionHelper.ParseObject(data).Value<int>("parentId"));
                    }
                    searchRequest.Predicate = searchRequest.Predicate.AndAlso(m => allowIds.Contains(m.Id));
                }
            }
            var result = await Repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
            return Ok(ParseSearchResult(req, result));
        }

        #endregion

        #region Overrides

        #endregion
    }
}
