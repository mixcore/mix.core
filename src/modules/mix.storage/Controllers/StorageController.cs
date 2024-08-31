using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.SignalR.Interfaces;
using Mix.Storage.Lib.ViewModels;

namespace Mix.Storage.Controllers
{
    [Route("api/v2/rest/mix-storage")]
    [ApiController]
    public class StorageController : MixRestfulApiControllerBase<MixMediaViewModel, MixCmsContext, MixMedia, Guid>
    {
        private readonly MixStorageService _storageService;

        public StorageController(MixStorageService storageService, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixCacheService cacheService, TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<MixCmsContext> uow, IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService) 
            : base(httpContextAccessor, configuration, 
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
            _storageService = storageService;
        }

        #region Routes

        [Route("scale-image")]
        [HttpGet]
        public async Task<ActionResult> Upload([FromQuery] string imageUrl)
        {
            await _storageService.ScaleImage(imageUrl);
            return Ok();
        }

        [Route("upload-file")]
        [HttpPost]
        public async Task<ActionResult> Upload([FromForm] string? folder, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var result = await _storageService.UploadFile(file, folder, User.Identity?.Name);
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
                var result = await _storageService.UploadStream(file, User.Identity?.Name);
                return Ok(result);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpDelete]
        [Route("delete-file")]
        public ActionResult<bool> Delete([FromQuery] string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath) || !fullPath.Contains(MixFolders.StaticFiles, StringComparison.InvariantCultureIgnoreCase))
            {
                return BadRequest();
            }

            var uri = new Uri(fullPath);
            var result = MixFileHelper.DeleteFile(uri.AbsolutePath[1..]);
            return Ok(result);
        }

        #endregion

        #region Overrides

        #endregion
    }
}
