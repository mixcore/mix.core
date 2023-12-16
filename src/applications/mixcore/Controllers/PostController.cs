using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.MixDb;
using Mix.Database.Services;
using Mix.Lib.Interfaces;

using Mix.RepoDb.Repositories;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Shared.Services;

namespace Mixcore.Controllers
{
    [Route("{controller}")]
    public class PostController : MixControllerBase
    {
        protected UnitOfWorkInfo Uow;
        protected readonly MixCmsContext CmsContext;
        private readonly DatabaseService _databaseService;
        private readonly MixCacheService _cacheService;
        private readonly MixRepoDbRepository _repoDbRepository;
        private readonly IMixMetadataService _metadataService;
        public PostController(
            IHttpContextAccessor httpContextAccessor,
            IPSecurityConfigService ipSecurityConfigService,
            IMixCmsService mixCmsService,
            DatabaseService databaseService,
            MixCmsContext cmsContext,
            MixRepoDbRepository repoDbRepository,
            IMixMetadataService metadataService,
            UnitOfWorkInfo<MixDbDbContext> dbUow,
            MixCacheService cacheService,
            IMixTenantService tenantService,
             IConfiguration configuration)
            : base(httpContextAccessor, mixCmsService, ipSecurityConfigService, tenantService, configuration)
        {
            CmsContext = cmsContext;
            Uow = new(CmsContext);
            _databaseService = databaseService;
            CmsContext = cmsContext;
            _repoDbRepository = repoDbRepository;
            _metadataService = metadataService;
            _repoDbRepository.SetDbConnection(dbUow);
            _cacheService = cacheService;
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
        [Route("{id}/{seoName}")]
        public async Task<IActionResult> Index(int id, string seoName)
        {
            if (IsValid)
            {
                return await Post(id, seoName);
            }
            else
            {
                return Redirect(RedirectUrl);
            }
        }

        #endregion Routes

        #region Helper
        protected async Task<IActionResult> Post(int postId, string? seoName = null)
        {
            // Home Post
            var postRepo = PostContentViewModel.GetRepository(Uow, _cacheService);
            var post = await postRepo.GetSingleAsync(m => m.Id == postId && m.MixTenantId == CurrentTenant.Id);
            if (post == null)
            {
                return NotFound();
            }
            await post.LoadAdditionalDataAsync(_repoDbRepository, _metadataService, _cacheService);
            
            ViewData["Title"] = post.SeoTitle;
            ViewData["Description"] = post.SeoDescription;
            ViewData["Keywords"] = post.SeoKeywords;
            ViewData["Image"] = post.Image;
            ViewData["Layout"] = post.Layout?.FilePath;
            ViewData["BodyClass"] = post.ClassName;
            ViewData["ViewMode"] = MixMvcViewMode.Post;
            ViewData["Keyword"] = seoName;

            ViewData["ViewMode"] = MixMvcViewMode.Post;
            return View(post);
        }
    }


    #endregion
}
