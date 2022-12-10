using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-data-content")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixDataContentPortalController
        : MixRestfulApiControllerBase<MixDataContentViewModel, MixCmsContext, MixDataContent, Guid>
    {
        private readonly Repository<MixCmsContext, MixDatabaseColumn, int, MixDatabaseColumnViewModel> _colRepository;
        private readonly MixDataService _mixDataService;

        public MixDataContentPortalController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixDataService mixDataService,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cmsUOW, queueService)
        {
            _mixDataService = mixDataService;
            _mixDataService.SetUnitOfWork(Uow);
            _colRepository = MixDatabaseColumnViewModel.GetRootRepository(cmsUOW.DbContext);
        }
        protected override async Task<PagingResponseModel<MixDataContentViewModel>> SearchHandler(
            [FromQuery] SearchRequestDto req, CancellationToken cancellationToken = default)
        {
            SearchDataContentModel searchReq = new SearchDataContentModel(req, Request);
            return await _mixDataService.Search<MixDataContentViewModel>(searchReq, Lang, cancellationToken);
        }

        [HttpGet("additional-data")]
        public async Task<ActionResult<MixDataContentViewModel>> GetAdditionalData([FromQuery] GetAdditionalDataDto dto, CancellationToken cancellationToken = default)
        {
            if (dto.IsValid())
            {
                var getData = await MixDataHelper.GetAdditionalDataAsync<AdditionalDataContentViewModel>(
                    Uow,
                    dto.ParentType.Value,
                    dto.DatabaseName,
                    dto.GuidParentId,
                    dto.IntParentId,
                    dto.Specificulture,
                    cancellationToken);
                return Ok(getData);
            }
            return BadRequest();
        }

        [HttpPost("{lang}/{databaseName}")]
        public async Task<ActionResult> CreateData([FromRoute] string databaseName, [FromBody] JObject data, CancellationToken cancellationToken = default)
        {
            var mixData = new MixDataContentViewModel(Lang, Culture.Id, databaseName, data);
            mixData.SetUowInfo(Uow);
            var result = await mixData.SaveAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("init/{databaseName}")]
        [HttpGet("{lang}/init/{databaseName}")]
        public async Task<ActionResult> InitData([FromRoute] string databaseName, CancellationToken cancellationToken = default)
        {
            int.TryParse(databaseName, out int id);
            var dbRepo = MixDatabaseViewModel.GetRepository(Uow);
            var mixdb = await dbRepo.GetSingleAsync(m => m.Id == id || m.SystemName == databaseName, cancellationToken);
            var mixData = new MixDataContentViewModel(Lang, Culture.Id, databaseName, new JObject())
            {
                Columns = mixdb.Columns,
                MixDatabaseId = mixdb.Id,
                MixDatabaseName = mixdb.SystemName
            };
            return Ok(mixData);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateData(Guid id, [FromBody] JObject data, CancellationToken cancellationToken = default)
        {
            var mixData = await Repository.GetSingleAsync(m => m.Id == id, cancellationToken);
            if (mixData != null)
            {
                mixData.Data = data;
                mixData.SetUowInfo(Uow);
                var result = await mixData.SaveAsync(cancellationToken);
                return Ok(result);
            }
            return NotFound(id);
        }
    }
}
