using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Identity.Services;
using Mix.Lib.Abstracts;
using Mix.Lib.Dtos;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Portal.Domain.ViewModels;
using Mix.Shared.Services;
using System;
using System.Linq.Expressions;
using Mix.Heart.Extensions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-database-column")]
    [ApiController]
    public class MixDatabaseColumnPortalController
        : MixRestApiControllerBase<MixDatabaseColumnViewModel, MixCmsContext, MixDatabaseColumn, int>
    {
        private readonly Repository<MixCmsContext, MixDatabaseColumn, int> _columnRepository;
        private readonly MixDataService _mixDataService;

        public MixDatabaseColumnPortalController(
            ILogger<MixApiControllerBase> logger,
            GlobalConfigService globalConfigService,
            MixService mixService,
            TranslatorService translator,
            Repository<MixCmsContext, MixCulture, int> cultureRepository,
            Repository<MixCmsContext, MixDatabaseColumn, int> columnRepository,
            MixDataService mixDataService,
            MixIdentityService mixIdentityService)
            : base(logger, globalConfigService, mixService, translator, cultureRepository, columnRepository, mixIdentityService)
        {
            _columnRepository = columnRepository;
            _mixDataService = mixDataService;
        }

        [HttpGet("init/{mixDatabase}")]
        public async Task<ActionResult<List<MixDatabaseColumnViewModel>>> Init(string mixDatabase)
        {
            int.TryParse(mixDatabase, out int mixDatabaseId);
            var getData = await _columnRepository.GetListViewAsync<MixDatabaseColumnViewModel>(
                f => f.MixDatabaseName == mixDatabase || f.MixDatabaseId == mixDatabaseId);
            return Ok(getData);
        }

        protected override SearchQueryModel<MixDatabaseColumn, int> BuildSearchRequest(SearchRequestDto req)
        {
            var searchReq = base.BuildSearchRequest(req);
            if (!string.IsNullOrEmpty(req.Keyword))
            {
                Expression<Func<MixDatabaseColumn, bool>> keywordPred = model =>
                 model.MixDatabaseName.Contains(req.Keyword)
                 || model.SystemName.Contains(req.Keyword)
                 || model.DisplayName.Contains(req.Keyword)
                 || model.DefaultValue.Contains(req.Keyword);
                searchReq.Predicate = searchReq.Predicate.AndAlso(keywordPred);
            }
            return searchReq;
        }
    }
}
