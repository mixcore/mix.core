using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;
using Mix.RepoDb.Repositories;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.SignalR.Interfaces;

namespace Mixcore.Controllers
{
    [EnableCors(MixCorsPolicies.PublicApis)]
    [Route("api/v2/rest/mixcore/post-content")]
    public sealed class PostContentApiController : MixQueryApiControllerBase<PostContentViewModel, MixCmsContext, MixPostContent, int>
    {
        private readonly MixRepoDbRepository _repoDbRepository;
        private readonly MixRepoDbRepository _mixRepoDbRepository;
        private readonly IMixMetadataService _metadataService;
        private readonly MixcorePostService _postService;
        public PostContentApiController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IQueueService<MessageQueueModel> queueService,
            MixcorePostService postService,
            MixRepoDbRepository mixRepoDbRepository,
            IMixMetadataService metadataService,
            MixRepoDbRepository repoDbRepository,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
            _postService = postService;
            _mixRepoDbRepository = mixRepoDbRepository;
            _metadataService = metadataService;
            _repoDbRepository = repoDbRepository;
        }

        [HttpPost("filter")]
        public async Task<ActionResult<PagingResponseModel<PostContentViewModel>>> Filter([FromBody] FilterContentRequestDto req)
        {
            try
            {
                var searchRequest = BuildSearchRequest(req);
                searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                    !string.IsNullOrEmpty(req.MixDatabaseName), m => m.MixDatabaseName == req.MixDatabaseName);
                if (!string.IsNullOrEmpty(req.MixDatabaseName) && req.Queries.Count > 0)
                {
                    _mixRepoDbRepository.InitTableName(req.MixDatabaseName);
                    var listData = await _mixRepoDbRepository.GetListByAsync(req.Queries, "Id, ParentId");
                    if (listData != null)
                    {
                        List<int> allowIds = new();
                        foreach (var data in listData)
                        {
                            // used JObject.FromObject to keep original reponse fieldName
                            allowIds.Add(JObject.FromObject(data).Value<int>("ParentId"));
                        }
                        searchRequest.Predicate = searchRequest.Predicate.AndAlso(m => allowIds.Contains(m.Id));
                    }
                }
                var result = await Repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
                foreach (var item in result.Items)
                {
                    await item.LoadAdditionalDataAsync(_repoDbRepository, _metadataService, CacheService);
                }
                return Ok(ParseSearchResult(req, result));
            }
            catch (MixException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        protected override async Task<PagingResponseModel<PostContentViewModel>> SearchHandler(SearchRequestDto req, CancellationToken cancellationToken = default)
        {
            var searchPostQuery = new SearchPostQueryModel(Request, req, CurrentTenant.Id);

            var result = await _postService.SearchPosts(searchPostQuery, cancellationToken);
            foreach (var item in result.Items)
            {
                await item.LoadAdditionalDataAsync(_mixRepoDbRepository, _metadataService, CacheService);
            }

            return RestApiService.ParseSearchResult(req, result);
        }

        protected override async Task<PostContentViewModel> GetById(int id)
        {
            var result = await base.GetById(id);
            await result.LoadAdditionalDataAsync(_mixRepoDbRepository, _metadataService, CacheService);
            return result;
        }
    }
}
