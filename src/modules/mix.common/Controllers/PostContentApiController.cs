using MessagePack.Resolvers;
using Microsoft.AspNetCore.Mvc;
using Mix.Common.Domain.ViewModels;
using Mix.Heart.Extensions;
using Mix.Heart.Models;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Lib.ViewModels;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.Repositories;
using Mix.SignalR.Interfaces;

namespace Mix.Common.Controllers
{
    [Route("api/v2/rest/common/post-content")]
    [ApiController]
    public class PostContentApiController : MixQueryApiControllerBase<PostContentViewModel, MixCmsContext, MixPostContent, int>
    {
        private readonly IMixDbDataService _mixDbDataService;
        private readonly MixRepoDbRepository _mixRepoDbRepository;
        public PostContentApiController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMemoryQueueService<MessageQueueModel> queueService,
            MixRepoDbRepository mixRepoDbRepository,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService,
            IMixDbDataService mixDbDataService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, cmsUow, queueService, portalHub, mixTenantService)
        {
            _mixRepoDbRepository = mixRepoDbRepository;
            _mixDbDataService = mixDbDataService;
        }


        [HttpPost("filter")]
        public async Task<ActionResult<PagingResponseModel<MixPostContentViewModel>>> Filter([FromBody] FilterContentRequestDto req)
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
            return Ok(ParseSearchResult(req, result));
        }

        protected override async Task<PostContentViewModel> GetById(int id)
        {
            var result = await base.GetById(id);
            if (result.AdditionalData == null)
            {
                await result.LoadAdditionalDataAsync(_mixDbDataService);
                await CacheService.SetAsync($"{id}/{typeof(PostContentViewModel).Name}", result, $"{typeof(MixPostContent).Assembly.GetName().Name}_{typeof(MixPostContent).Name}", "full");
            }
            return result;
        }


        //protected override async Task<PagingResponseModel<PostContentViewModel>> SearchHandler(SearchRequestDto req)
        //{
        //    var searchRequest = BuildSearchRequest(req);
        //    var result = await _repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);

        //    List<Task> tasks = new();
        //    foreach (var post in result.Items)
        //    {
        //        if (post.AdditionalData == null)
        //        {
        //            tasks.Add(
        //                post.LoadAdditionalDataAsync(_mixRepoDbRepository)
        //                .ContinueWith(r => _cacheService.SetAsync($"{post.Id}/{typeof(PostContentViewModel).FullName}", post, typeof(MixPostContent), "full")));
        //        }
        //    }
        //    await Task.WhenAll(tasks);
        //    return result;
        //}

    }
}
