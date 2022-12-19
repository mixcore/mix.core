using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Lib.Dtos;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.RepoDb.Repositories;
using Mix.Services.Databases.Lib.Services;
using System.Threading;

namespace Mixcore.Controllers
{
    [Route("api/v2/rest/mixcore/module-data")]
    public sealed class ModuleDataController : MixRestHandlerApiControllerBase<ModuleDataViewModel, MixCmsContext, MixModuleData, int>
    {
        public ModuleDataController(IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, uow, queueService)
        {
        }

        [HttpGet("get-module-data")]
        public async Task<ActionResult<PagingResponseModel<ModuleDataViewModel>>> GetByModule([FromQuery] SearchModuleDataDto req, CancellationToken cancellationToken = default)
        {
            var searchRequest = BuildSearchRequest(req);
            var result = await RestApiService.SearchHandler(req, searchRequest, cancellationToken);
            return Ok(ParseSearchResult(req, result));
        }

        protected override SearchQueryModel<MixModuleData, int> BuildSearchRequest(SearchRequestDto req)
        {
            var request = new SearchModuleDataDto(req, Request);
            var searchRequest = base.BuildSearchRequest(request);

            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                request.ModuleId.HasValue,
                m => m.ParentId == request.ModuleId);

            return searchRequest;
        }

        private PagingResponseModel<JObject> ParseSearchResult(SearchRequestDto req, PagingResponseModel<ModuleDataViewModel> result, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string[] selectedColumns = { };
            if (!string.IsNullOrEmpty(req.Columns))
            {
                selectedColumns = req.Columns.TrimEnd(',').Split(',', StringSplitOptions.TrimEntries);
            }

            List<JObject> objects = result.Items.Select(m => m.Data).ToList();
            if (!string.IsNullOrEmpty(req.Columns))
            {
                objects = objects.Select(item => new JObject()
                    {
                        item.Properties()
                            .Where(p => selectedColumns.Any(m => m.ToLower() == p.Name.ToLower()))
                    }).ToList();
            }
            return new PagingResponseModel<JObject>()
            {
                Items = objects,
                PagingData = result.PagingData
            };
        }
    }
}
