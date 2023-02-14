using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.RepoDb.Repositories;
using Mix.Services.Databases.Lib.Interfaces;

namespace Mixcore.Controllers
{
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
            MixService mixService, 
            TranslatorService translator, 
            MixIdentityService mixIdentityService, 
            UnitOfWorkInfo<MixCmsContext> uow, 
            IQueueService<MessageQueueModel> queueService, 
            MixcorePostService postService, 
            MixRepoDbRepository mixRepoDbRepository,
            IMixMetadataService metadataService, 
            MixRepoDbRepository repoDbRepository) : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, uow, queueService)
        {
            _postService = postService;
            _mixRepoDbRepository = mixRepoDbRepository;
            _metadataService = metadataService;
            _repoDbRepository = repoDbRepository;
        }

        [HttpPost("filter")]
        public async Task<ActionResult<PagingResponseModel<PostContentViewModel>>> Filter([FromBody] FilterContentRequestDto req)
        {
            var searchRequest = BuildSearchRequest(req);
            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                !string.IsNullOrEmpty(req.MixDatabaseName), m => m.MixDatabaseName == req.MixDatabaseName);
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
            foreach (var item in result.Items)
            {
                await item.LoadAdditionalDataAsync(_repoDbRepository, _metadataService);
            }
            return Ok(ParseSearchResult(req, result));
        }

        protected override async Task<PagingResponseModel<PostContentViewModel>> SearchHandler(SearchRequestDto req, CancellationToken cancellationToken = default)
        {
            var searchPostQuery = new SearchPostQueryModel(Request, req, CurrentTenant.Id);
            
            var result= await _postService.SearchPosts(searchPostQuery, cancellationToken);
            foreach (var item in result.Items)
            {
                await item.LoadAdditionalDataAsync(_mixRepoDbRepository, _metadataService);
            }

            return RestApiService.ParseSearchResult(req, result);
        }

        protected override async Task<PostContentViewModel> GetById(int id)
        {
            var result = await base.GetById(id);
            await result.LoadAdditionalDataAsync(_mixRepoDbRepository, _metadataService);
            return result;
        }
    }
}
