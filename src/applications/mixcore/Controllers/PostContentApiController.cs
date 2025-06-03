using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Mixdb.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.RepoDb.Repositories;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Shared.Models;
using Mix.SignalR.Interfaces;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace Mixcore.Controllers
{
    /// <summary>
    /// API controller for managing post content operations
    /// </summary>
    [EnableCors(MixCorsPolicies.PublicApis)]
    [Route("api/v2/rest/mixcore/post-content")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Post Content Management")]
    public sealed class PostContentApiController : MixQueryApiControllerBase<PostContentViewModel, MixCmsContext, MixPostContent, int>
    {
        private readonly IMixDbDataService _mixDbDataService;
        private readonly RepoDbRepository _repoDbRepository;
        private readonly RepoDbRepository _mixRepoDbRepository;
        private readonly IMixMetadataService _metadataService;
        private readonly MixcorePostService _postService;

        /// <summary>
        /// Constructor for PostContentApiController
        /// </summary>
        public PostContentApiController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IMemoryQueueService<MessageQueueModel> queueService,
            MixcorePostService postService,
            RepoDbRepository mixRepoDbRepository,
            IMixMetadataService metadataService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService,
            IMixDbDataServiceFactory mixDbDataServiceFactory,
            DatabaseService databaseService)
            : base(httpContextAccessor, configuration,
                  cacheService, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
            _postService = postService;
            _mixRepoDbRepository = mixRepoDbRepository;
            _metadataService = metadataService;
            _mixDbDataService = mixDbDataServiceFactory.Create(
                databaseService.DatabaseProvider,
                databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION));
        }

        /// <summary>
        /// Searches for post content based on the provided criteria
        /// </summary>
        /// <param name="req">Search request parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paged list of post content items</returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Successfully retrieved post content items", typeof(PagingResponseModel<PostContentViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid request parameters")]
        protected override async Task<PagingResponseModel<PostContentViewModel>> SearchHandler(SearchRequestDto req, CancellationToken cancellationToken = default)
        {
            var searchPostQuery = new SearchPostQueryModel(Request, req, CurrentTenant.Id);

            var result = await _postService.SearchPosts(searchPostQuery, cancellationToken);
            foreach (var item in result.Items)
            {
                await item.LoadAdditionalDataAsync(_mixDbDataService, _metadataService, CacheService, cancellationToken);
            }

            return RestApiService.ParseSearchResult(req, result);
        }

        /// <summary>
        /// Retrieves a specific post content by its identifier
        /// </summary>
        /// <param name="id">The post content identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The post content details</returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Successfully retrieved post content", typeof(PostContentViewModel))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Post content not found")]
        protected override async Task<PostContentViewModel> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await base.GetById(id);
            await result.LoadAdditionalDataAsync(_mixDbDataService, _metadataService, CacheService, cancellationToken);
            return result;
        }

        /// <summary>
        /// Gets a paginated list of post content items
        /// </summary>
        /// <param name="request">Search and pagination parameters</param>
        /// <returns>Paginated list of post content items</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get paginated post content items",
            Description = "Retrieves a paginated list of post content items based on search criteria",
            OperationId = "GetPostContent",
            Tags = new[] { "Post Content" }
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Successfully retrieved post content items", typeof(PagingResponseModel<PostContentViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid request parameters")]
        public override async Task<ActionResult<PagingResponseModel<PostContentViewModel>>> Get([FromQuery] SearchRequestDto request)
        {
            return await base.Get(request);
        }

        /// <summary>
        /// Filters post content items based on criteria
        /// </summary>
        /// <param name="request">Filter and pagination parameters</param>
        /// <returns>Filtered and paginated list of post content items</returns>
        [HttpPost("filter")]
        [SwaggerOperation(
            Summary = "Filter post content items",
            Description = "Filters post content items based on specified criteria",
            OperationId = "FilterPostContent",
            Tags = new[] { "Post Content" }
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Successfully filtered post content items", typeof(PagingResponseModel<PostContentViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid filter parameters")]
        public override async Task<ActionResult<PagingResponseModel<PostContentViewModel>>> Filter([FromBody] SearchRequestDto request)
        {
            return await base.Filter(request);
        }

        /// <summary>
        /// Gets a specific post content by ID
        /// </summary>
        /// <param name="id">The post content identifier</param>
        /// <returns>The post content details</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get post content by ID",
            Description = "Retrieves a specific post content by its unique identifier",
            OperationId = "GetPostContentById",
            Tags = new[] { "Post Content" }
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Successfully retrieved post content", typeof(PostContentViewModel))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Post content not found")]
        public override async Task<ActionResult<PostContentViewModel>> GetSingle(int id)
        {
            return await base.GetSingle(id);
        }

        /// <summary>
        /// Gets a default post content template
        /// </summary>
        /// <returns>A default post content object</returns>
        [HttpGet("default")]
        [SwaggerOperation(
            Summary = "Get default post content template",
            Description = "Retrieves a default post content object with initialized values",
            OperationId = "GetDefaultPostContent",
            Tags = new[] { "Post Content" }
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Successfully retrieved default post content", typeof(PostContentViewModel))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Error creating default post content")]
        public override async Task<ActionResult<PostContentViewModel>> GetDefaultAsync()
        {
            return await base.GetDefaultAsync();
        }
    }
}
