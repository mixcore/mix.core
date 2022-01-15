using Microsoft.AspNetCore.Mvc;
using Mix.Database.Services;
using Mix.Lib.Services;
using Mix.Shared.Services;

namespace Mixcore.Controllers
{
    [Route("{controller}")]
    public class PostController : MixControllerBase
    {
        protected UnitOfWorkInfo _uow;
        protected readonly MixCmsContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly TranslatorService _translator;
        private readonly MixDatabaseService _databaseService;
        public PostController(
            ILogger<HomeController> logger,
            IPSecurityConfigService ipSecurityConfigService,
            MixService mixService,
            TranslatorService translator,
            MixDatabaseService databaseService,
            MixCmsContext context,
            MixCacheService cacheService)
            : base(mixService, ipSecurityConfigService)
        {
            _context = context;
            _uow = new(_context);
            _logger = logger;
            _translator = translator;
            _databaseService = databaseService;
            _context = context;
        }

        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (GlobalConfigService.Instance.AppSettings.IsInit)
            {
                isValid = false;
                if (string.IsNullOrEmpty(_databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    _redirectUrl = "Init";
                }
                else
                {
                    var status = GlobalConfigService.Instance.AppSettings.InitStatus;
                    _redirectUrl = $"/init/step{status}";
                }
            }
        }

        #region Routes
        [Route("{id}")]
        [Route("{id}/{keyword}")]
        public async Task<IActionResult> Index(int id, string keyword)
        {
            if (isValid)
            {
                return await Post(id, keyword);
            }
            else
            {
                return Redirect(_redirectUrl);
            }
        }

        #endregion Routes

        #region Helper
        protected async Task<IActionResult> Post(int postId, string keyword = null)
        {
            // Home Post
            var postRepo = PostContentViewModel.GetRepository(_uow);
            var post = await postRepo.GetSingleAsync(postId);
            ViewData["Title"] = post.SeoTitle;
            ViewData["Description"] = post.SeoDescription;
            ViewData["Keywords"] = post.SeoKeywords;
            ViewData["Image"] = post.Image;
            ViewData["Layout"] = post.Layout.FilePath;
            ViewData["BodyClass"] = post.ClassName;
            ViewData["ViewMode"] = MixMvcViewMode.Post;
            ViewData["Keyword"] = keyword;

            ViewBag.viewMode = MixMvcViewMode.Post;
            return View(post);
        }
    }


    #endregion
}
