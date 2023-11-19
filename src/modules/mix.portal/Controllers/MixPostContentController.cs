using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Lib.Interfaces;
using Mix.Portal.Domain.Services;
using Mix.RepoDb.Repositories;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-post-content")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixPostContentController
        : MixBaseContentController<MixPortalPostContentViewModel, MixPostContent, int>
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
            PortalPostService postService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(MixContentType.Post, identityService, userManager, httpContextAccessor, configuration, cacheService, translator,
                  mixIdentityService, cmsUow, queueService, portalHub, mixTenantService)
        {
            _mixRepoDbRepository = mixRepoDbRepository;
            _postService = postService;
        }

        #region Routes

        [HttpPost("filter")]
        public async Task<ActionResult<PagingResponseModel<MixPortalPostContentViewModel>>> Filter([FromBody] FilterContentRequestDto req)
        {
            var searchRequest = BuildSearchRequest(req);
            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                !string.IsNullOrEmpty(req.MixDatabaseName), m => m.MixDatabaseName == req.MixDatabaseName);
            var metadataPostIds = _postService.ParseMetadataQueriesPredicate(req.MetadataQueries)?.ToList();
            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(metadataPostIds != null,
                                        m => metadataPostIds.Contains(m.Id));

            if (!string.IsNullOrEmpty(req.MixDatabaseName) && req.Queries.Count > 0)
            {
                _mixRepoDbRepository.InitTableName(req.MixDatabaseName);
                var listData = await _mixRepoDbRepository.GetListByAsync(req.Queries, "ParentId");
                if (listData != null)
                {
                    List<int> allowIds = listData.Select(m => (int)m.ParentId).ToList();
                    searchRequest.Predicate = searchRequest.Predicate.AndAlso(m => allowIds.Contains(m.Id));
                }
            }
            var result = await Repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
            if (req.LoadNestedData)
            {
                foreach (var item in result.Items)
                {
                    await item.LoadAdditionalDataAsync(_mixRepoDbRepository, CacheService);
                }
            }
            return Ok(ParseSearchResult(req, result));
        }

        #endregion

        #region Overrides
        #endregion
    }
}
