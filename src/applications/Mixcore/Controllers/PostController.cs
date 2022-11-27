using Microsoft.AspNetCore.Mvc;
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
        private readonly MixRepoDbRepository _repoDbRepository;
        public PostController(
            IHttpContextAccessor httpContextAccessor,
            ILogger<HomeController> logger,
            IPSecurityConfigService ipSecurityConfigService,
            MixService mixService,
            MixCmsService mixCmsService,
            TranslatorService translator,
            DatabaseService databaseService,
            MixCmsContext context,
            MixRepoDbRepository repoDbRepository)
            : base(httpContextAccessor, mixService, mixCmsService, ipSecurityConfigService)
        {
            _context = context;
            _uow = new(_context);
            _logger = logger;
            _translator = translator;
            _databaseService = databaseService;
            _context = context;
            _repoDbRepository = repoDbRepository;
        }

        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (GlobalConfigService.Instance.AppSettings.IsInit)
            {
                IsValid = false;
                if (string.IsNullOrEmpty(_databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    RedirectUrl = "Init";
                }
                else
                {
                    var status = GlobalConfigService.Instance.AppSettings.InitStatus;
                    RedirectUrl = $"/init/step{status}";
                }
            }
        }

        #region Routes
        [Route("{id}")]
        [Route("{id}/{keyword}")]
        public async Task<IActionResult> Index(int id, string keyword)
        {
            if (IsValid)
            {
                return await Post(id, keyword);
            }
            else
            {
                return Redirect(RedirectUrl);
            }
        }

        #endregion Routes

        #region Helper
        protected async Task<IActionResult> Post(int postId, string keyword = null)
        {
            // Home Post
            var postRepo = PostContentViewModel.GetRepository(_uow);
            var post = await postRepo.GetSingleAsync(m => m.Id == postId && m.MixTenantId == CurrentTenant.Id);
            if (post == null)
            {
                return NotFound();
            }
            if (post.AdditionalData == null)
            {
                await post.LoadAdditionalDataAsync(_repoDbRepository);
            }
            ViewData["Title"] = post.SeoTitle;
            ViewData["Description"] = post.SeoDescription;
            ViewData["Keywords"] = post.SeoKeywords;
            ViewData["Image"] = post.Image;
            ViewData["Layout"] = post.Layout?.FilePath;
            ViewData["BodyClass"] = post.ClassName;
            ViewData["ViewMode"] = MixMvcViewMode.Post;
            ViewData["Keyword"] = keyword;

            ViewData["ViewMode"] = MixMvcViewMode.Post;
            return View(post);
        }
    }


    #endregion
}
