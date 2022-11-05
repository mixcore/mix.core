using Microsoft.AspNetCore.Mvc;
using Mix.Storage.Lib.Models;
using Mix.Storage.Lib.ViewModels;

namespace Mix.Storage.Controllers
{
    [Route("api/v2/rest/mix-storage/file-system")]
    [ApiController]
    [MixAuthorize(roles: MixRoles.Owner)]
    public class FileSystemController : MixTenantApiControllerBase
    {
        private MixStorageService _storageService;

        public FileSystemController(
            IHttpContextAccessor httpContextAccessor, 
            IConfiguration configuration, 
            MixService mixService, 
            TranslatorService translator, 
            MixIdentityService mixIdentityService, 
            IQueueService<MessageQueueModel> queueService, 
            MixStorageService storageService) 
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, queueService)
        {
            _storageService = storageService;
        }

        #region Routes

        [HttpGet]
        public ActionResult<MixFileResponseModel> GetList([FromQuery] SearchFileRequestDto request)
        {
            var files = MixFileHelper.GetTopFiles(request.Folder);
            var directories = MixFileHelper.GetTopDirectories(request.Folder);
            var result = new MixFileResponseModel()
            {
                Files = files,
                Directories = directories
            };
            return Ok(result);
        }


        // Post api/files/id
        [HttpGet]
        [Route("details")]
        public ActionResult<FileModel> Details(string folder, string filename)
        {
            // Request: Key => folder, Keyword => filename
            if (!string.IsNullOrEmpty(folder))
            {
                var result = MixFileHelper.GetFileByFullName($"{folder}/{filename}");
                if (result != null)
                {
                    return Ok(result);
                }
            }
            return NotFound();
        }

        // GET api/files/id
        [HttpDelete]
        [Route("delete")]
        public ActionResult<bool> Delete()
        {
            string fullPath = Request.Query["fullPath"].ToString();
            var result = MixFileHelper.DeleteFile(fullPath);
            return result ? Ok() : BadRequest();
        }

        // POST api/values
        /// <summary>
        /// Uploads the image.
        /// </summary>
        /// <param name="image">The img information.</param>
        /// <param name="file"></param> Ex: { "base64": "", "fileFolder":"" }
        /// <returns></returns>
        [Route("upload-file")]
        [HttpPost]
        public IActionResult Upload([FromForm] string folder, [FromForm] IFormFile file)
        {
            if (ModelState.IsValid)
            {
                folder ??= DateTime.Now.ToString("yyyy-MMM");
                folder = $"{MixFolders.UploadsFolder}/{folder.TrimStart('/').TrimEnd('/')}";
                string webPath = $"{MixFolders.WebRootPath}/{folder}";
                var result = MixFileHelper.SaveFile(file, webPath);
                if (!string.IsNullOrEmpty(result))
                {
                    return Ok($"{CurrentTenant.Configurations.Domain}/{folder}/{result}");
                }
            }
            return BadRequest();
        }

        // POST api/files
        [HttpPost]
        [Route("save")]
        public ActionResult<FileModel> Save([FromBody] FileModel model)
        {
            if (model != null)
            {
                var result = MixFileHelper.SaveFile(model);
                return Ok(result);
            }
            return BadRequest(model);
        }

        #endregion

        #region Overrides

        #endregion
    }
}
