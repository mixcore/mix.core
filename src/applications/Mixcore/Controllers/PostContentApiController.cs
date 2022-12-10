using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;

namespace Mixcore.Controllers
{
    [Route("api/v2/rest/mixcore/post-content")]
    public sealed class PostContentApiController : MixQueryApiControllerBase<PostContentViewModel, MixCmsContext, MixPostContent, int>
    {
        private readonly PostService _postService;
        public PostContentApiController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixService mixService, TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<MixCmsContext> uow, IQueueService<MessageQueueModel> queueService, PostService postService) : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, uow, queueService)
        {
            _postService = postService;
        }
        protected override async Task<PagingResponseModel<PostContentViewModel>> SearchHandler(SearchRequestDto req, CancellationToken cancellationToken = default)
        {
            var searchPostQuery = new SearchPostQueryModel(Request, req, CurrentTenant.Id);
            return await _postService.SearchPosts(searchPostQuery, cancellationToken);
        }
    }
}
