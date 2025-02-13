using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.MixDb;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Extensions;
using Mix.Lib.Interfaces;
using Mix.Mixdb.Interfaces;
using Mix.RepoDb.Repositories;
using Mix.Services.Databases.Lib.Interfaces;

namespace Mixcore.Controllers
{
    [Route("{controller}")]
    public class PostController : MixControllerBase
    {
        protected UnitOfWorkInfo<MixCmsContext> Uow;
        protected readonly MixCmsContext CmsContext;
        private readonly DatabaseService _databaseService;
        private readonly MixCacheService _cacheService;
        private readonly RepoDbRepository _repoDbRepository;
        private readonly IMixMetadataService _metadataService;
        private readonly IMixDbDataService _mixDbDataService;
        public PostController(
            IHttpContextAccessor httpContextAccessor,
            IPSecurityConfigService ipSecurityConfigService,
            IMixCmsService mixCmsService,
            DatabaseService databaseService,
            MixCmsContext cmsContext,
            IMixMetadataService metadataService,
            UnitOfWorkInfo<MixDbDbContext> dbUow,
            MixCacheService cacheService,
            IMixTenantService tenantService,
             IConfiguration configuration,
             IMixDbDataService mixDbDataService)
            : base(httpContextAccessor, mixCmsService, ipSecurityConfigService, tenantService, configuration)
        {
            CmsContext = cmsContext;
            Uow = new(CmsContext);
            _databaseService = databaseService;
            CmsContext = cmsContext;
            _metadataService = metadataService;
            _cacheService = cacheService;
            _mixDbDataService = mixDbDataService;
            _mixDbDataService.SetDbConnection(Uow);
        }

        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (Configuration.IsInit())
            {
                IsValid = false;
                if (string.IsNullOrEmpty(_databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    RedirectUrl = "Init";
                }
                else
                {
                    RedirectUrl = $"/init/step{Configuration.InitStep()}";
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
        protected async Task<IActionResult> Post(int postId, string seoName = null, CancellationToken cancellationToken = default)
        {
            // Home Post
            var postRepo = PostContentViewModel.GetRepository(Uow, _cacheService);
            var post = await postRepo.GetSingleAsync(m => m.Id == postId && m.TenantId == CurrentTenant.Id);
            if (post == null)
            {
                return NotFound();
            }
            await post.LoadAdditionalDataAsync(_mixDbDataService, _metadataService, _cacheService, cancellationToken);
            
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
