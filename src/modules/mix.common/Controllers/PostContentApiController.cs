using Microsoft.AspNetCore.Mvc;
using Mix.Common.Domain.ViewModels;
using Mix.Heart.Extensions;
using Mix.Heart.Models;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.RepoDb.Repositories;

namespace Mix.Common.Controllers
{
    [Route("api/v2/rest/post-content")]
    [ApiController]
    public class PostContentApiController : MixQueryApiControllerBase<PostContentViewModel, MixCmsContext, MixPostContent, int>
    {
        private readonly MixRepoDbRepository _mixRepoDbRepository;
        protected MixCacheService _cacheService;
        public PostContentApiController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IQueueService<MessageQueueModel> queueService,
            MixRepoDbRepository mixRepoDbRepository)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, uow, queueService)
        {
            _mixRepoDbRepository = mixRepoDbRepository;
            _cacheService = new();
        }


        [HttpPost("filter")]
        public async Task<ActionResult<PagingResponseModel<PostContentViewModel>>> Filter([FromBody] FilterContentRequestDto req)
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
            List<Task> tasks = new();
            foreach (var post in result.Items)
            {
                if (post.AdditionalData == null)
                {
                    tasks.Add(
                        post.LoadAdditionalDataAsync(_mixRepoDbRepository)
                        .ContinueWith(r => _cacheService.SetAsync($"{post.Id}/{typeof(PostContentViewModel).FullName}", post, typeof(MixPostContent), "full")));
                }
            }
            await Task.WhenAll(tasks);
            return Ok(ParseSearchResult(req, result));
        }

        protected override async Task<PostContentViewModel> GetById(int id)
        {
            var result = await base.GetById(id);
            if (result.AdditionalData == null)
            {
                await result.LoadAdditionalDataAsync(_mixRepoDbRepository);
                await _cacheService.SetAsync($"{id}/{typeof(PostContentViewModel).FullName}", result, typeof(MixPostContent), "full");
            }
            return result;
        }


        protected override async Task<PagingResponseModel<PostContentViewModel>> SearchHandler(SearchRequestDto req)
        {
            var searchRequest = BuildSearchRequest(req);
            var result = await _repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);

            List<Task> tasks = new();
            foreach (var post in result.Items)
            {
                if (post.AdditionalData == null)
                {
                    tasks.Add(
                        post.LoadAdditionalDataAsync(_mixRepoDbRepository)
                        .ContinueWith(r => _cacheService.SetAsync($"{post.Id}/{typeof(PostContentViewModel).FullName}", post, typeof(MixPostContent), "full")));
                }
            }
            await Task.WhenAll(tasks);
            return result;
        }

    }
}
