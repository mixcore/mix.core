using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Constant.Constants;
using Mix.Database.Entities.QueueLog;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Interfaces;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Log.Lib.Models;
using Mix.Log.Lib.ViewModels;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Shared.Dtos;
using Mix.SignalR.Interfaces;

namespace Mix.Log.Controllers
{
    [Route("api/v2/rest/mix-log/queue-log")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixQueueLogController : MixQueryApiControllerBase<MixQueueMessageLogViewModel, QueueLogDbContext, QueueLog, Guid>
    {
        private readonly DatabaseService _databaseService;
        public MixQueueLogController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<QueueLogDbContext> uow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService,
            DatabaseService databaseService)
            : base(
                httpContextAccessor,
                configuration,
                cacheService,
                mixIdentityService,
                uow,
                queueService,
                portalHub,
                mixTenantService)
        {
            _databaseService = databaseService;
        }

        #region Routes

        [HttpGet("search")]
        public async Task<ActionResult> Search([FromQuery] SearchRequestDto req, CancellationToken cancellationToken = default)
        {
            SearchLogResponseModel<MixQueueMessageLogViewModel> result = new();
            if (req.FromDate.HasValue)
            {
                var searchRequest = BuildSearchRequest(req);
                var searchDate = req.FromDate.Value;
                var searchResult = await SearchByDate(searchRequest, searchDate, cancellationToken);
                result.Results.Add(searchResult);
                if (result.PagingData.TotalPage < searchResult.Data?.PagingData.TotalPage)
                {
                    result.PagingData = searchResult.Data.PagingData;
                }

                if (req.ToDate.HasValue)
                {
                    while (searchDate < req.ToDate.Value)
                    {
                        searchDate = searchDate.AddDays(1);
                        searchResult = await SearchByDate(searchRequest, searchDate, cancellationToken);
                        result.Results.Add(searchResult);
                        if (result.PagingData.TotalPage < searchResult.Data?.PagingData.TotalPage)
                        {
                            result.PagingData = searchResult.Data.PagingData;
                        }
                    }
                }
                return Ok(result);
            }

            var data = await base.SearchHandler(req, cancellationToken);
            result.Results.Add(new()
            {
                SearchDate = DateTime.UtcNow,
                Data = data,
            });
            result.PagingData = data.PagingData;

            return Ok(result);
        }

        #endregion

        #region Overrides

        private async Task<SearchLogResult<MixQueueMessageLogViewModel>> SearchByDate(SearchQueryModel<QueueLog, Guid> searchRequest, DateTime searchDate, CancellationToken cancellationToken)
        {
            try
            {

                string fileName = $"{MixFolders.MixQueueLogFolder}/{searchDate.ToString("MM_yyyy")}/queuelog_{searchDate.ToString("dd_MM_yyyy")}.sqlite";

                if (!System.IO.File.Exists(fileName))
                {
                    return new()
                    {
                        SearchDate = searchDate
                    };
                }

                using (var context = _databaseService.GetQueueLogDbContext())
                {
                    using (var MixQueueMessageLogUow = new UnitOfWorkInfo(context))
                    {
                        return new()
                        {
                            SearchDate = searchDate,
                            Data = await MixQueueMessageLogViewModel.GetRepository(MixQueueMessageLogUow, CacheService)
                                            .GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData, cancellationToken)
                        };
                    }
                }
            }
            catch (MixException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        #endregion
    }
}
