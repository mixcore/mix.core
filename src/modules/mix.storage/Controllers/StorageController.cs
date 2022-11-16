using Microsoft.AspNetCore.Mvc;
using Mix.Storage.Lib.ViewModels;

namespace Mix.Storage.Controllers
{
    [Route("api/v2/rest/mix-storage")]
    [ApiController]
    public class StorageController : MixRestfulApiControllerBase<MixMediaViewModel, MixCmsContext, MixMedia, Guid>
    {
        private readonly MixStorageService _storageService;

        public StorageController(MixStorageService storageService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IQueueService<MessageQueueModel> queueService) : base(httpContextAccessor, configuration, mixService, translator, 
                mixIdentityService, uow, queueService)
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
                var result = await _storageService.UploadFile(file, folder, User?.Identity?.Name);
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
