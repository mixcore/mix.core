using Microsoft.AspNetCore.Mvc;
using Mix.Database.Services;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Shared.Services;

namespace Mixcore.Domain.Bases
{
    public class MvcBaseController : MixControllerBase
    {
        protected UnitOfWorkInfo<MixCmsContext> Uow;
        protected readonly TranslatorService Translator;
        protected readonly DatabaseService DatabaseService;
        public MvcBaseController(
            IHttpContextAccessor httpContextAccessor,
            IPSecurityConfigService ipSecurityConfigService,
            MixService mixService,
            IMixCmsService mixCmsService,
            TranslatorService translator,
            DatabaseService databaseService,
            UnitOfWorkInfo<MixCmsContext> uow) : base(httpContextAccessor, mixService, mixCmsService, ipSecurityConfigService)
        {
            Translator = translator;
            DatabaseService = databaseService;
            Uow = uow;
        }

        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (GlobalConfigService.Instance.AppSettings.IsInit)
            {
                IsValid = false;
                if (string.IsNullOrEmpty(DatabaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
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

        #region Helper
        protected async Task<IActionResult> Page(int pageId, string keyword = null)
        {
            // Home Page
            var pageRepo = PageContentViewModel.GetRepository(Uow);
            var page = await pageRepo.GetSingleAsync(pageId);
            ViewData["Title"] = page.SeoTitle;
            ViewData["Description"] = page.SeoDescription;
            ViewData["Keywords"] = page.SeoKeywords;
            ViewData["Image"] = page.Image;
            ViewData["Layout"] = page.Layout.FilePath;
            ViewData["BodyClass"] = page.ClassName;
            ViewData["ViewMode"] = MixMvcViewMode.Page;
            ViewData["Keyword"] = keyword;

            ViewData["ViewMode"] = MixMvcViewMode.Page;
            return View(page);
        }
        #endregion
    }
}
