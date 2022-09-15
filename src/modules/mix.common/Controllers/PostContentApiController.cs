using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Entities.Cache;
using Mix.Heart.Models;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.RepoDb.Repositories;
using Mix.Common.Domain.ViewModels;
using Mix.Lib.Models.Common;
using Mix.Heart.Extensions;

namespace Mix.Common.Controllers
{
    [Route("api/v2/rest/post-content")]
    [ApiController]
    public class PostContentApiController : MixQueryApiControllerBase<PostContentViewModel, MixCmsContext, MixPostContent, int>
    {
        private readonly MixRepoDbRepository _mixRepoDbRepository;
        public PostContentApiController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<MixCmsContext> uow,
            IQueueService<MessageQueueModel> queueService,
            MixRepoDbRepository mixRepoDbRepository,
            MixCacheService cacheService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheUOW, uow, queueService)
        {
            _mixRepoDbRepository = mixRepoDbRepository;
        }


        [HttpPost("filter")]
        public async Task<ActionResult<PagingResponseModel<PostContentViewModel>>> Filter([FromBody] SearchRequestDto req)
        {
            var result = await SearchHandler(req);
            return ParseSearchResult(req, result);
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
