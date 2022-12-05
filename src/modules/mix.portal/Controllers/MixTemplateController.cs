using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-template")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixTemplateController
        : MixRestfulApiControllerBase<MixTemplateViewModel, MixCmsContext, MixTemplate, int>
    {
        public MixTemplateController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cmsUOW, queueService)
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
            else
            {
                return NotFound();
            }
        }
        #endregion

        #region Overrides

        protected override async Task<MixTemplateViewModel> GetById(int id)
        {
            var result = await base.GetById(id);
            result.FileFolder = $"{MixFolders.TemplatesFolder}/{CurrentTenant.SystemName}/{result.MixThemeName}/{result.FolderType}";
            return result;
        }

        protected override Task<int> CreateHandlerAsync(MixTemplateViewModel data, CancellationToken cancellationToken)
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
