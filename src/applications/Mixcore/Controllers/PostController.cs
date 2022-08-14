using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Runtime;
using Mix.Database.Services;
using Mix.Heart.Helpers;
using Mix.Lib.Services;
using Mix.RepoDb.Repositories;
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
        private readonly DatabaseService _databaseService;
        private readonly RuntimeDbContextService _runtimeDbContextService;
        private readonly MixRepoDbRepository _repoDbRepository;
        public PostController(
            IHttpContextAccessor httpContextAccessor,
            ILogger<HomeController> logger,
            IPSecurityConfigService ipSecurityConfigService,
            MixService mixService,
            TranslatorService translator,
            DatabaseService databaseService,
            MixCmsContext context,
            MixCacheService cacheService,
            RuntimeDbContextService runtimeDbContextService,
            MixRepoDbRepository repoDbRepository)
            : base(httpContextAccessor, mixService, ipSecurityConfigService)
        {
            _context = context;
            _uow = new(_context);
            _logger = logger;
            _translator = translator;
            _databaseService = databaseService;
            _context = context;
            _runtimeDbContextService = runtimeDbContextService;
            _repoDbRepository = repoDbRepository;
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
            var post = await postRepo.GetSingleAsync(m => m.Id == postId && m.MixTenantId == MixTenantId);
            if (post == null)
            {
                return NotFound();
            }
            if (post.AdditionalData == null)
            {
                _repoDbRepository.Init(post.MixDatabaseName);
                post.AdditionalData = ReflectionHelper.ParseObject(await _repoDbRepository.GetSingleByParentAsync(post.Id));
                if (post.AdditionalData!=null)
                {
                    await postRepo.CacheService.SetAsync($"{post.Id}/{typeof(PostContentViewModel).FullName}", post, typeof(MixPostContent), postRepo.CacheFilename);
                }
            }
            ViewData["Title"] = post.SeoTitle;
            ViewData["Description"] = post.SeoDescription;
            ViewData["Keywords"] = post.SeoKeywords;
            ViewData["Image"] = post.Image;
            ViewData["Layout"] = post.Layout?.FilePath;
            ViewData["BodyClass"] = post.ClassName;
            ViewData["ViewMode"] = MixMvcViewMode.Post;
            ViewData["Keyword"] = keyword;

            ViewBag.viewMode = MixMvcViewMode.Post;
            return View(post);
        }
    }


    #endregion
}
