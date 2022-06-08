using Microsoft.AspNetCore.Mvc;
using Mix.Shared.Services;

namespace Mix.Storage.Controllers
{
    [Route("api/v2/rest/mix-storage/media")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private MixStorageService _storageService;

        public StorageController(MixStorageService storageService)
        {
            _storageService = storageService;
        }

        #region Routes

        [Route("upload-file")]
        [HttpPost]
        public async Task<ActionResult> Upload([FromForm] string folder, [FromForm] IFormFile file)
        {
            if (ModelState.IsValid)
            {
                await _storageService.UploadFile(folder, file);
                return Ok();
            }
            return BadRequest();
        }
        #endregion

        #region Overrides

        #endregion


    }
}
