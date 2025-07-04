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
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Mixcore.Controllers
{
    /// <summary>
    /// API controller for managing page content operations
    /// </summary>
    [Route("api/v2/rest/mixcore/page-content")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Page Content Management")]
    public sealed class PageContentApiController : MixQueryApiControllerBase<PageContentViewModel, MixCmsContext, MixPageContent, int>
    {
        private readonly IMixMetadataService _metadataService;
        private readonly IMixDbDataService _mixDbDataService;

        /// <summary>
        /// Constructor for PageContentApiController
        /// </summary>
        public PageContentApiController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IMixMetadataService metadataService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService,
            IMixDbDataServiceFactory mixDbDataServiceFactory,
            DatabaseService databaseService)
            : base(httpContextAccessor, configuration,
                  cacheService, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
            _metadataService = metadataService;
            _mixDbDataService = mixDbDataServiceFactory.Create(
            databaseService.DatabaseProvider,
            databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION));
        }

        [HttpGet("get-by-seo-name/{seoName}")]
        public async Task<ActionResult<MixPageContentViewModel>> GetBySeoName(string seoName, CancellationToken cancellationToken = default)
        {
            var data = await RestApiService.Repository.GetFirstAsync(m => m.SeoName == seoName);
            if (data != null)
            {
                return Ok(data);
            }
            throw new MixException(MixErrorStatus.NotFound, seoName);
        }

        /// <summary>
        /// Retrieves a specific page content by its identifier
        /// </summary>
        /// <param name="id">The page content identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The page content details</returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Successfully retrieved page content", typeof(PageContentViewModel))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Page content not found")]
        protected override async Task<PageContentViewModel> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await base.GetById(id, cancellationToken);
            await result.LoadDataAsync(_mixDbDataService, _metadataService, new(), CacheService);
            return result;
        }

        /// <summary>
        /// Gets a paginated list of page content items
        /// </summary>
        /// <param name="request">Search and pagination parameters</param>
        /// <returns>Paginated list of page content items</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get paginated page content items",
            Description = "Retrieves a paginated list of page content items based on search criteria",
            OperationId = "GetPageContent",
            Tags = new[] { "Page Content" }
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Successfully retrieved page content items", typeof(PagingResponseModel<PageContentViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid request parameters")]
        public override async Task<ActionResult<PagingResponseModel<PageContentViewModel>>> Get([FromQuery] SearchRequestDto request)
        {
            return await base.Get(request);
        }

        /// <summary>
        /// Filters page content items based on criteria
        /// </summary>
        /// <param name="request">Filter and pagination parameters</param>
        /// <returns>Filtered and paginated list of page content items</returns>
        [HttpPost("filter")]
        [SwaggerOperation(
            Summary = "Filter page content items",
            Description = "Filters page content items based on specified criteria",
            OperationId = "FilterPageContent",
            Tags = new[] { "Page Content" }
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Successfully filtered page content items", typeof(PagingResponseModel<PageContentViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid filter parameters")]
        public override async Task<ActionResult<PagingResponseModel<PageContentViewModel>>> Filter([FromBody] SearchRequestDto request)
        {
            return await base.Filter(request);
        }

        /// <summary>
        /// Gets a specific page content by ID
        /// </summary>
        /// <param name="id">The page content identifier</param>
        /// <returns>The page content details</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get page content by ID",
            Description = "Retrieves a specific page content by its unique identifier",
            OperationId = "GetPageContentById",
            Tags = new[] { "Page Content" }
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Successfully retrieved page content", typeof(PageContentViewModel))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Page content not found")]
        public override async Task<ActionResult<PageContentViewModel>> GetSingle(int id)
        {
            return await base.GetSingle(id);
        }

        /// <summary>
        /// Gets a default page content template
        /// </summary>
        /// <returns>A default page content object</returns>
        [HttpGet("default")]
        [SwaggerOperation(
            Summary = "Get default page content template",
            Description = "Retrieves a default page content object with initialized values",
            OperationId = "GetDefaultPageContent",
            Tags = new[] { "Page Content" }
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Successfully retrieved default page content", typeof(PageContentViewModel))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Error creating default page content")]
        public override async Task<ActionResult<PageContentViewModel>> GetDefaultAsync()
        {
            return await base.GetDefaultAsync();
        }
    }
}
