using Microsoft.AspNetCore.Mvc;
using Mix.Storage.Lib.ViewModels;

namespace Mix.Storage.Controllers
{
    [Route("api/v2/rest/mix-storage")]
    [ApiController]
    public class StorageController : MixRestApiControllerBase<MixMediaViewModel, MixCmsContext, MixMedia, Guid>
    {
        private MixStorageService _storageService;

        public StorageController(MixStorageService storageService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<MixCmsContext> uow,
            IQueueService<MessageQueueModel> queueService) : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheUOW, uow, queueService)
        {
            _storageService = storageService;
        }

        #region Routes

        [Route("upload-file")]
        [HttpPost]
        public async Task<ActionResult> Upload([FromForm] string? folder, [FromForm] IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var result = await _storageService.UploadFile(file, folder,  User?.Identity?.Name);
                return Ok(result);
            }
            return BadRequest();
        }

        [Authorize]
        [Route("upload-file-stream")]
        [HttpPost]
        public async Task<ActionResult> UploadFileStream(FileModel file)
        {
            if (ModelState.IsValid)
            {
                var result = await _storageService.UploadStream(file, User?.Identity?.Name);
                return Ok(result);
            }
            return BadRequest();
        }


        #endregion

        #region Overrides

        #endregion
    }
}
