using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-data-content")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixDataContentPortalController
        : MixRestApiControllerBase<MixDataContentViewModel, MixCmsContext, MixDataContent, Guid>
    {
        private readonly Repository<MixCmsContext, MixDatabaseColumn, int, MixDatabaseColumnViewModel> _colRepository;
        private readonly MixDataService _mixDataService;

        public MixDataContentPortalController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixDataService mixDataService,
            MixIdentityService mixIdentityService,
            MixCmsContext context,
            MixCacheService cacheService,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, context, queueService)
        {
            _mixDataService = mixDataService;
            _mixDataService.SetUnitOfWork(_uow);
            _colRepository = MixDatabaseColumnViewModel.GetRootRepository(context);
        }

        public override async Task<ActionResult<PagingResponseModel<MixDataContentViewModel>>> Get([FromQuery] SearchRequestDto req)
        {
            SearchMixDataDto searchReq = new SearchMixDataDto(req, Request);
            var result = await _mixDataService.FilterByKeywordAsync<MixDataContentViewModel>(searchReq, _lang);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagingResponseModel<MixDataContentViewModel>>> Search([FromQuery] SearchMixDataDto request)
        {
            var result = await _mixDataService.FilterByKeywordAsync<MixDataContentViewModel>(request, _lang);
            return Ok(result);
        }

        [HttpGet("additional-data")]
        public async Task<ActionResult<MixDataContentViewModel>> GetAdditionalData([FromQuery] GetAdditionalDataDto dto)
        {
            if (dto.IsValid())
            {
                var getData = await MixDataHelper.GetAdditionalDataAsync(
                    _uow,
                    dto.ParentType.Value,
                    dto.DatabaseName,
                    dto.GuidParentId,
                    dto.IntParentId,
                    _lang);
                return Ok(getData);
            }
            return BadRequest();
        }

        [HttpPost("{lang}/{databaseName}")]
        public async Task<ActionResult> CreateData([FromRoute] string databaseName, [FromBody] JObject data)
        {
            var mixData = new MixDataContentViewModel(_lang, _culture.Id, databaseName, data);
            mixData.SetUowInfo(_uow);
            var result = await mixData.SaveAsync();
            return Ok(result);
        }

        [HttpGet("init/{databaseName}")]
        [HttpGet("{lang}/init/{databaseName}")]
        public async Task<ActionResult> InitData([FromRoute] string databaseName)
        {
            int.TryParse(databaseName, out int id);
            var dbRepo = MixDatabaseViewModel.GetRepository(_uow);
            var mixdb = await dbRepo.GetSingleAsync(m => m.Id == id || m.SystemName == databaseName);
            var mixData = new MixDataContentViewModel(_lang, _culture.Id, databaseName, new JObject())
            {
                Columns = mixdb.Columns,
                MixDatabaseId = mixdb.Id,
                MixDatabaseName = mixdb.SystemName
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
                mixData.SetUowInfo(_uow);
                var result = await mixData.SaveAsync();
                return Ok(result);
            }
            return NotFound(id);
        }
    }
}
