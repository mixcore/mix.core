using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Lib.Base;
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
using Microsoft.Extensions.Configuration;
using Mix.Heart.Services;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-database-column")]
    [ApiController]
    public class MixDatabaseColumnPortalController
        : MixRestApiControllerBase<MixDatabaseColumnViewModel, MixCmsContext, MixDatabaseColumn, int>
    {
        private readonly MixDataService _mixDataService;
        private readonly MixEndpointService _endpointService;

        public MixDatabaseColumnPortalController(
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixDataService mixDataService,
            MixIdentityService mixIdentityService, 
            MixEndpointService endpointService,
            MixCmsContext context,
            MixCacheService cacheService,
            IQueueService<MessageQueueModel> queueService)
            : base(configuration, mixService, translator, cultureRepository, mixIdentityService, context, cacheService, queueService)
        {
            _mixDataService = mixDataService;
            _endpointService = endpointService;
        }

        [HttpGet("init/{mixDatabase}")]
        public async Task<ActionResult<List<MixDatabaseColumnViewModel>>> Init(string mixDatabase)
        {
            int.TryParse(mixDatabase, out int mixDatabaseId);
            var getData = await _repository.GetListAsync(
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
