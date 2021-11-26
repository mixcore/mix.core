using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Model;
using Mix.Heart.Repository;
using Mix.Lib.Dtos;
using Mix.Lib.Services;
using Mix.Lib.ViewModels;
using Mix.Heart.Extensions;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-template")]
    [ApiController]
    public class MixTemplateController
        : MixRestApiControllerBase<MixTemplateViewModel, MixCmsContext, MixTemplate, int>
    {
        public MixTemplateController(
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            MixCmsContext context,
            MixCacheService cacheService,
            IQueueService<MessageQueueModel> queueService)
            : base(configuration, mixService, translator, cultureRepository, mixIdentityService, context, cacheService, queueService)
        {

        }

        public override async Task<IActionResult> Update(string id, [FromBody] MixTemplateViewModel data)
        {
            var result = await base.Update(id, data);
            var msg = new MessageQueueModel()
            {
                Action = MixRestAction.Put,
                Status = MixRestStatus.Success
            };
            msg.Package(data);
            _queueService.PushQueue(msg);
            return result;
        }

        public override async Task<ActionResult<PagingResponseModel<MixTemplateViewModel>>> Get([FromQuery] SearchRequestDto req)
        {
            var searchRequest = BuildSearchRequest(req);

            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                Enum.TryParse(Request.Query["folderType"], out MixTemplateFolderType folderType),
                m => m.FolderType == folderType);
            return await _repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
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
    }
}
