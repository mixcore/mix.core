﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Heart.Model;
using Mix.Heart.Repository;
using Mix.Lib.Abstracts;
using Mix.Lib.Dtos;
using Mix.Lib.Models;
using Mix.Lib.Services;
using Mix.Lib.ViewModels;
using Mix.Portal.Domain.ViewModels;
using Mix.Queue.Interfaces;
using Mix.Shared.Services;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-theme")]
    [ApiController]
    public class MixThemeController
        : MixRestApiControllerBase<MixThemeViewModel, MixCmsContext, MixTheme, int>
    {
        private readonly MixThemeExportService _exportService;
        private readonly MixThemeImportService _importService;
        private IQueueService<QueueMessageModel> _queueService;
        public MixThemeController(
            ILogger<MixApiControllerBase> logger,
            GlobalConfigService globalConfigService,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            MixThemeExportService exportService,
            MixCmsContext context, MixThemeImportService importService,
            IQueueService<QueueMessageModel> queueService)
            : base(logger, globalConfigService, mixService, translator, cultureRepository, mixIdentityService, context)
        {

            _exportService = exportService;
            _importService = importService;
            _queueService = queueService;
        }

        public override System.Threading.Tasks.Task<ActionResult<PagingResponseModel<MixThemeViewModel>>> Get([FromQuery] SearchRequestDto req)
        {
            var post = new MixThemeViewModel()
            {
                DisplayName = " test queue"
            };
            var msg = new QueueMessageModel();
            msg.Package(post);
            _queueService.PushQueue(msg);
            return base.Get(req);
        }

        [HttpPost("export")]
        public ActionResult<SiteDataViewModel> ExportTheme(ExportThemeDto dto)
        {
            var siteData = _exportService.ExportTheme(dto);
            return Ok(siteData);
        }
    }
}