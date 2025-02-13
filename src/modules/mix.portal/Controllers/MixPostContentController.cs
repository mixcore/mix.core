using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Auth.Constants;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mixdb.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.Portal.Domain.Services;
using Mix.RepoDb.Repositories;
using Mix.SignalR.Interfaces;
using NuGet.Protocol;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-post-content")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixPostContentController
        : MixBaseContentController<MixPortalPostContentViewModel, MixPostContent, int>
    {
        private string _requestedBy;
        private readonly PortalPostService _postService;
        private readonly IMixDbDataService _mixDbDataSrv;
        public MixPostContentController(
            MixIdentityService identityService,
            TenantUserManager userManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IMixDbDataService mixDbDataSrv,
            PortalPostService postService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(MixContentType.Post, identityService, userManager, httpContextAccessor, configuration, cacheService, translator,
                  mixIdentityService, cmsUow, queueService, portalHub, mixTenantService)
        {
            _mixDbDataSrv = mixDbDataSrv;
            _postService = postService;
        }
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _requestedBy = User == null ? default : MixIdentityService.GetClaim(User, MixClaims.UserName);
            return base.OnActionExecutionAsync(context, next);
        }
        #region Routes

        //[HttpPost("filter")]
        //public async Task<ActionResult<PagingResponseModel<MixPortalPostContentViewModel>>> Filter([FromBody] FilterContentRequestDto req, CancellationToken cancellationToken = default)
        //{
        //    var searchRequest = BuildSearchRequest(req);
        //    searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
        //        !string.IsNullOrEmpty(req.MixDatabaseName), m => m.MixDatabaseName == req.MixDatabaseName);
        //    var metadataPostIds = _postService.ParseMetadataQueriesPredicate(req.MetadataQueries)?.ToList();
        //    searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(metadataPostIds != null,
        //                                m => metadataPostIds.Contains(m.Id));

        //    if (!string.IsNullOrEmpty(req.MixDatabaseName) && req.Queries.Count > 0)
        //    {
        //        var listData = await _mixDbDataSrv.GetListByAsync(
        //            new Shared.Models.SearchMixDbRequestModel() {
        //                TableName = req.MixDatabaseName, 
        //                Queries = req.Queries, 
        //            },
        //            cancellationToken: cancellationToken);
        //        if (listData != null)
        //        {
        //            List<int> allowIds = listData.Select(m => m.GetJObjectProperty<int>("ParentId")).ToList();
        //            searchRequest.Predicate = searchRequest.Predicate.AndAlso(m => allowIds.Contains(m.Id));
        //        }
        //    }
        //    var result = await Repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
        //    if (req.LoadNestedData)
        //    {
        //        foreach (var item in result.Items)
        //        {
        //            await item.LoadAdditionalDataAsync(_mixDbDataSrv, CacheService, cancellationToken: cancellationToken);
        //        }
        //    }
        //    return Ok(ParseSearchResult(req, result));
        //}

        #endregion

        #region Overrides
        #endregion
    }
}
