using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-template")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixTemplateController
        : MixRestfulApiControllerBase<MixTemplateViewModel, MixCmsContext, MixTemplate, int>
    {
        public MixTemplateController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration, 
                  cacheService, translator, mixIdentityService, cmsUow, queueService, portalHub, mixTenantService)
        {
        }

        #region Routes


        [HttpGet("copy/{id}")]
        public async Task<ActionResult<MixTemplateViewModel>> Copy(int id)
        {
            var getData = await Repository.GetSingleAsync(id);
            if (getData != null)
            {
                var copyResult = await getData.CopyAsync();
                if (copyResult != null)
                {
                    return Ok(copyResult);
                }
                return BadRequest();
            }

            return NotFound();
        }
        #endregion

        #region Overrides

        protected override async Task<MixTemplateViewModel> GetById(int id)
        {
            var result = await base.GetById(id);
            result.FileFolder = $"{MixFolders.TemplatesFolder}/{CurrentTenant.SystemName}/{result.MixThemeName}/{result.FolderType}";
            return result;
        }

        protected override Task<int> CreateHandlerAsync(MixTemplateViewModel data, CancellationToken cancellationToken = default)
        {
            data.FileFolder = $"{MixFolders.TemplatesFolder}/{CurrentTenant.SystemName}/{data.MixThemeName}/{data.FolderType}";
            return base.CreateHandlerAsync(data, cancellationToken);
        }

        protected override Task UpdateHandler(int id, MixTemplateViewModel data, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            data.FileFolder = $"{MixFolders.TemplatesFolder}/{CurrentTenant.SystemName}/{data.MixThemeName}/{data.FolderType}";
            return base.UpdateHandler(id, data, cancellationToken);
        }
        protected override SearchQueryModel<MixTemplate, int> BuildSearchRequest(SearchRequestDto req)
        {
            var request = new SearchTemplateDto(req, Request);
            var searchRequest = base.BuildSearchRequest(request);

            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                request.Folder.HasValue,
                m => m.FolderType == request.Folder.Value);

            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                request.ThemeId.HasValue,
                m => m.MixThemeId == request.ThemeId);

            return searchRequest;
        }
        #endregion
    }
}
