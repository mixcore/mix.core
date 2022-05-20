using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-template")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixTemplateController
        : MixRestApiControllerBase<MixTemplateViewModel, MixCmsContext, MixTemplate, int>
    {
        public MixTemplateController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            MixCacheDbContext cacheDbContext,
            MixCmsContext context,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheDbContext, context, queueService)
        {

        }


        [HttpGet("copy/{id}")]
        public async Task<ActionResult<MixTemplateViewModel>> Copy(int id)
        {
            var getData = await _repository.GetSingleAsync(id);
            if (getData != null)
            {
                var copyResult = await getData.CopyAsync();
                if (copyResult != null)
                {
                    return Ok(copyResult);
                }
                else
                {
                    return BadRequest(copyResult.Errors);
                }
            }
            else
            {
                return NotFound();
            }
        }

        #region Overrides


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
