using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.RepoDb.Repositories;
using Mix.Services.Databases.Lib.Services;

namespace Mixcore.Controllers
{
    [Route("api/v2/rest/mixcore/post-content")]
    public sealed class PostContentApiController : MixQueryApiControllerBase<PostContentViewModel, MixCmsContext, MixPostContent, int>
    {
        private readonly MixRepoDbRepository _mixRepoDbRepository;
        private readonly MixMetadataService _metadataService;
        private readonly MixcorePostService _postService;
        public PostContentApiController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixService mixService, TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<MixCmsContext> uow, IQueueService<MessageQueueModel> queueService, MixcorePostService postService, MixRepoDbRepository mixRepoDbRepository, MixMetadataService metadataService) : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, uow, queueService)
        {
            _postService = postService;
            _mixRepoDbRepository = mixRepoDbRepository;
            _metadataService = metadataService;
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
