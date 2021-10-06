using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Heart.Model;
using Mix.Heart.Repository;
using Mix.Identity.Services;
using Mix.Lib.Abstracts;
using Mix.Lib.Dtos;
using Mix.Lib.Helpers;
using Mix.Lib.Services;
using Mix.Portal.Domain.ViewModels;
using Mix.Shared.Enums;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-data-content")]
    [ApiController]
    public class MixDataContentPortalController
        : MixRestApiControllerBase<MixDataContentViewModel, MixCmsContext, MixDataContent, Guid>
    {
        private readonly Repository<MixCmsContext, MixDatabaseColumn, int, MixDatabaseColumnViewModel> _colRepository;
        private readonly MixDataService _mixDataService;

        public MixDataContentPortalController(
            ILogger<MixApiControllerBase> logger,
            GlobalConfigService globalConfigService,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixDataService mixDataService,
            MixIdentityService mixIdentityService)
            : base(logger, globalConfigService, mixService, translator, cultureRepository, mixIdentityService)
        {
            _mixDataService = mixDataService;
            _colRepository = MixDatabaseColumnViewModel.GetRepository();
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagingResponseModel<MixDataContentViewModel>>> Search([FromQuery] SearchMixDataDto request)
        {
            var result = await _mixDataService.FilterByKeywordAsync<MixDataContentViewModel>(request, _lang);
            return Ok(result);
        }

        [HttpGet("additional-data")]
        public async Task<ActionResult<MixDataContentViewModel>> GetAdditionalData([FromQuery] string databaseName)
        {
            Guid guidParentId = Guid.Empty;
            bool isParent = int.TryParse(Request.Query["parentId"].ToString(), out int intParentId);
            isParent = isParent || Guid.TryParse(Request.Query["parentId"].ToString(), out guidParentId);
            if (Enum.TryParse(Request.Query["parentType"].ToString(), out MixDatabaseParentType parentType)
                && isParent)
            {
                var getData = await MixDataHelper.GetAdditionalDataAsync(parentType, databaseName, guidParentId, intParentId, _lang);
                return Ok(getData);
            }
            return BadRequest();
        }

        [HttpPost("{lang}/{databaseName}")]
        public async Task<ActionResult> CreateData([FromRoute] string databaseName, [FromBody] JObject data)
        {
            var mixData = new MixDataContentViewModel(_lang, _culture.Id, databaseName, data);
            var result = await mixData.SaveAsync();
            return Ok(result);
        }

        [HttpGet("init/{databaseName}")]
        [HttpGet("{lang}/init/{databaseName}")]
        public async Task<ActionResult> InitData([FromRoute] string databaseName)
        {
            var columns = await _colRepository.GetListAsync(
                    m => m.MixDatabaseName == databaseName);
            var mixData = new MixDataContentViewModel(_lang, _culture.Id, databaseName, new JObject())
            {
                Columns = columns
            };
            return Ok(mixData);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateData(Guid id, [FromBody] JObject data)
        {
            var mixData = await _repository.GetSingleAsync(m => m.Id == id);
            if (mixData != null)
            {
                mixData.Data = data;
                var result = await mixData.SaveAsync();
                return Ok(result);
            }
            return NotFound(id);
        }
    }
}
