using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-file")]
    [ApiController]
    public class MixFileController : MixApiControllerBase
    {
        private readonly MixCmsContext _context;
        public MixFileController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCmsContext context,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, queueService)
        {
            _context = context;
        }

        #region Post

        // Post api/files/id
        [HttpGet]
        [Route("details")]
        public ActionResult<FileModel> Details(string folder, string filename)
        {
            // Request: Key => folder, Keyword => filename
            if (!string.IsNullOrEmpty(folder))
            {
                var result = MixFileHelper.GetFile(filename, folder);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            return NotFound();
        }

        // GET api/files/id
        [HttpGet]
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
                var result = MixFileHelper.SaveFile(file, $"{folder}");
                return Ok(result);
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

        // GET api/files
        [HttpGet]
        [Route("")]
        public ActionResult<MixFileResponseModel> GetList([FromQuery] SearchFileRequestDto request)
        {
            if (!request.Folder.StartsWith(MixFolders.WebRootPath))
            {
                return BadRequest(request.Folder);
            }
            var files = MixFileHelper.GetTopFiles(request.Folder);
            var directories = MixFileHelper.GetTopDirectories(request.Folder);
            var result = new MixFileResponseModel()
            {
                Files = files,
                Directories = directories
            };
            return Ok(result);
        }

        #endregion Post
    }
}
