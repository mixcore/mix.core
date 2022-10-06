using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Helpers;
using Mix.RepoDb.Repositories;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-post-content")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin},{MixRoles.Owner}")]
    public class MixPostContentController
        : MixBaseContentController<MixPostContentViewModel, MixPostContent, int>
    {
        private readonly MixRepoDbRepository _mixRepoDbRepository;
        public MixPostContentController(
            MixIdentityService identityService,
            TenantUserManager userManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService,
            MixRepoDbRepository mixRepoDbRepository)
            : base(MixContentType.Post, identityService, userManager, httpContextAccessor, configuration, mixService, translator, mixIdentityService, cacheUOW, cmsUOW, queueService)
        {
            _mixRepoDbRepository = mixRepoDbRepository;
        }

        #region Routes

        [HttpPost("filter")]
        public async Task<ActionResult<PagingResponseModel<MixPostContentViewModel>>> Filter([FromBody] FilterContentRequestDto req)
        {
            var searchRequest = BuildSearchRequest(req);
            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                !string.IsNullOrEmpty(req.MixDatabaseName), m => m.MixDatabaseName == req.MixDatabaseName);
            if (!string.IsNullOrEmpty(req.MixDatabaseName) && req.Queries.Count > 0)
            {
                _mixRepoDbRepository.Init(req.MixDatabaseName);
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
            var result = await _repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
            return ParseSearchResult(req, result);
        }

        #endregion

        #region Overrides

        #endregion

    }
}
