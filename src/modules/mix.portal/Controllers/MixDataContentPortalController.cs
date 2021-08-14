using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Heart.Model;
using Mix.Heart.Repository;
using Mix.Identity.Services;
using Mix.Lib.Abstracts;
using Mix.Lib.Dtos;
using Mix.Lib.Services;
using Mix.Portal.Domain.ViewModels;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-data-content")]
    [ApiController]
    public class MixDataContentPortalController : MixApiControllerBase
    {
        private readonly Repository<MixCmsContext, MixDataContent, Guid> _contentRepository;
        private readonly MixDataService _mixDataService;

        public MixDataContentPortalController(
            ILogger<MixApiControllerBase> logger,
            MixAppSettingService appSettingService,
            MixService mixService,
            TranslatorService translator,
            Repository<MixCmsContext, MixCulture, int> cultureRepository,
            Repository<MixCmsContext, MixDataContent, Guid> contentRepository,
            MixDataService mixDataService,
            MixIdentityService mixIdentityService)
            : base(logger, appSettingService, mixService, translator, cultureRepository, mixIdentityService)
        {
            _contentRepository = contentRepository;
            _mixDataService = mixDataService;
        }

        [HttpGet]
        public async Task<ActionResult<PagingResponseModel<MixDataContentViewModel>>> Get([FromQuery] SearchMixDataDto request)
        {
            var result = await _mixDataService.FilterByKeywordAsync<MixDataContentViewModel>(request, _lang);
            return Ok(result);
        }

        [HttpPost("{lang}/{databaseName}")]
        public async Task<ActionResult> CreateData([FromRoute] string databaseName, [FromBody] JObject data)
        {
            var mixData = new MixDataContentViewModel(_lang, _culture.Id, databaseName, data);
            var result = await mixData.SaveAsync();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateData(Guid id, [FromBody] JObject data)
        {
            var mixData = await _contentRepository.GetSingleViewAsync<MixDataContentViewModel>(m => m.Id == id);
            if (mixData != null)
            {
                mixData.Obj = data;
                var result = await mixData.SaveAsync();
                return Ok(result);
            }
            return NotFound(id);
        }
    }
}
