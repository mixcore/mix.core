using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Helpers;
using Mix.Heart.Model;
using Mix.Lib.Extensions;
using Mix.Lib.Interfaces;
using Mix.Mixdb.Dtos;
using Mix.Mixdb.Helpers;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.Services;
using Mix.Mixdb.ViewModels;
using Mix.Mq.Lib.Models;
using Mix.Service.Commands;
using Mix.Service.Interfaces;
using Mix.Service.Models;
using Mix.Shared.Models;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using RepoDb.Enumerations;
using RepoDb.Interfaces;

namespace Mix.Portal.Controllers
{
    [MixDatabaseAuthorize("")]
    [Route("api/v2/rest/mix-portal/mix-db/{name}")]
    [ApiController]
    public class MixDbController : MixTenantApiControllerBase
    {
        ILogger<MixDbController> _logger;
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        private readonly IMixMemoryCacheService _memoryCache;
        private readonly MixDbDataServiceFactory _mixDbDataFactory;
        private readonly DatabaseService _databaseService;
        private readonly IPortalHubClientService PortalHub;
        private string _tableName;
        private string _requestedBy;
        private readonly MixIdentityService _idService;
        private IMixDbDataService _mixDbDataService;
        private const string AssociationTableName = nameof(MixDatabaseAssociation);
        private MixDbDatabaseViewModel _mixDb;
        private FieldNameService _fieldNameService;
        public MixDbController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IMemoryQueueService<MessageQueueModel> queueService,
            IMixMemoryCacheService memoryCache,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            ICache cache,
            DatabaseService databaseService,
            MixIdentityService idService,
            MixDbDataServiceFactory mixDbDataFactory,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService,
            ILogger<MixDbController> logger)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, queueService, mixTenantService)
        {
            _cmsUow = cmsUow;
            _databaseService = databaseService;
            _memoryCache = memoryCache;
            _idService = idService;
            PortalHub = portalHub;
            _logger = logger;
            _mixDbDataFactory = mixDbDataFactory;
        }

        #region Overrides
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _tableName = RouteData?.Values["name"].ToString();
            _mixDb = await GetMixDatabase();
            if (!_mixDb.MixDatabaseContextId.HasValue)
            {
                _mixDb.DatabaseProvider = _databaseService.DatabaseProvider;

            }
            _mixDbDataService = _mixDbDataFactory.GetDataService(_mixDb.DatabaseProvider,
                _mixDb.MixDatabaseContext?.ConnectionString.Decrypt(Configuration.AesKey()) ??
                _databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION));
            _fieldNameService = new FieldNameService(_mixDb.NamingConvention);
            _requestedBy = User == null ? default : _idService.GetClaim(User, MixClaims.UserName);
            await base.OnActionExecutionAsync(context, next);
        }

        #endregion

        [HttpPost("by-ids")]
        public async Task<ActionResult<PagingResponseModel<JObject>>> GetByIds([FromBody] GetIdsDto req, CancellationToken cancellationToken = default)
        {
            var searchDto = new SearchMixDbRequestDto
            {
                Queries = new List<MixQueryField>()
                {
                    new MixQueryField(_fieldNameService.Id,req.Ids, MixCompareOperator.InRange)
                }
            };

            var result = await SearchHandler(searchDto, cancellationToken);

            return Ok(result);
        }

        [HttpPost("filter")]
        public async Task<ActionResult<PagingResponseModel<JObject>>> Filter([FromBody] SearchMixDbRequestDto req,
            CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = await SearchHandler(req, cancellationToken);
                return Ok(result);
            }
            throw new TaskCanceledException();
        }

        [HttpPost("nested-data/filter")]
        public async Task<ActionResult<PagingResponseModel<JObject>>> NestedFilter([FromBody] SearchMixDbRequestDto req, CancellationToken cancellationToken = default)
        {
            if (req.Relationship == MixDatabaseRelationshipType.ManyToMany)
            {
                var result = await SearchManyToManyDataHandler(req);
                return Ok(result);
            }
            else
            {
                return Ok(await SearchHandler(req, cancellationToken));
            }
        }

        [HttpPost("export")]
        public async Task<ActionResult<FileModel>> Export([FromBody] SearchMixDbRequestDto req, CancellationToken cancellationToken = default)
        {
            var result = await SearchHandler(req, cancellationToken);
            string filename = $"{_tableName}_{DateTime.UtcNow:dd-MM-yyyy-hh-mm-ss}";
            string exportPath = $"{MixFolders.ExportFolder}/mix-db/{_tableName}/";
            var file = MixCmsHelper.ExportJObjectToExcel(result.Items.ToList(), _tableName, exportPath, filename, null);
            return Ok(file);
        }

        [HttpPost("import")]
        public async Task<ActionResult> Import(IFormFile file, CancellationToken cancellationToken = default)
        {
            var data = MixCmsHelper.LoadExcelFileData(file);
            List<JObject> lstDto = new();
            var username = _idService.GetClaim(User, MixClaims.UserName);
            foreach (var item in data)
            {
                lstDto.Add(item);
            }

            await _mixDbDataService.CreateManyAsync(_tableName, lstDto, _requestedBy, cancellationToken);
            return Ok();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<JObject>> GetSingle(string id, [FromQuery] string selectColumns, CancellationToken cancellationToken = default)
        {
            var result = await _mixDbDataService.GetByIdAsync(_tableName, id, selectColumns, cancellationToken);
            return result != default ? Ok(result) : NotFound(id);
        }

        [HttpGet("get-by-parent/{parentType}/{parentId}")]
        public async Task<ActionResult<JObject>> GetSingleByParent(MixContentType parentType, int parentId, [FromQuery] string selectColumns, CancellationToken cancellationToken = default)
        {
            var data = await _mixDbDataService.GetSingleByParentAsync(_tableName, parentType, parentId, selectColumns, cancellationToken);
            return NotFound();
        }

        [HttpGet("get-by-guid-parent/{parentType}/{parentId}")]
        public async Task<ActionResult<JObject>> GetSingleByGuidParent(MixContentType parentType, Guid parentId, [FromQuery] string selectColumns, CancellationToken cancellationToken = default)
        {
            var obj = await _mixDbDataService.GetSingleByParentAsync(_tableName, parentType, parentId, selectColumns, cancellationToken: cancellationToken);
            return obj != null ? Ok(obj) : NotFound();
        }

        [HttpPost("hub")]
        public ActionResult CreateHub(object dto)
        {
            var obj = new MixDbCommandModel()
            {
                RequestedBy = User?.Identity?.Name,
                Body = JObject.FromObject(dto),
                MixDbName = _tableName
            };
            QueueService.PushMemoryQueue(CurrentTenant.Id, MixQueueTopics.MixDbCommand, MixDbCommandQueueAction.POST.ToString(), obj);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<object>> Create(JObject dto, CancellationToken cancellationToken = default)
        {
            string username = _idService.GetClaim(User, MixClaims.UserName);
            var id = await _mixDbDataService.CreateAsync(_tableName, dto, _requestedBy, cancellationToken);
            var resp = await _mixDbDataService.GetByIdAsync(_tableName, id, string.Empty, cancellationToken);

            //if (dto.ContainsKey(_fieldNameService.ParentId))
            //{
            //await CreateDataRelationship(
            //    dto.Value<string>(_fieldNameService.ParentDatabaseName), 
            //    dto.Value<string>(_fieldNameService.ParentId),
            //    dto.Value<string>(_fieldNameService.ChildDatabaseName), id.ToString(), username);
            //}

            var result = resp != null ? ReflectionHelper.ParseObject(resp) : dto;
            await NotifyResult(MixDbCommandQueueAction.POST, new MixDbAuditLogModel()
            {
                Id = id,
                MixDbName = _tableName,
                After = result,
                Body = dto
            });
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<object>> UpdateFields(string id, [FromBody] JObject dto, CancellationToken cancellationToken = default)
        {
            var data = await _mixDbDataService.UpdateAsync(_tableName, id, dto, _requestedBy, dto.Properties().Select(m => m.Name), cancellationToken);

            if (data != null)
            {
                var result = await _mixDbDataService.GetSingleByAsync(_tableName, new MixQueryField(_fieldNameService.Id, id), string.Empty, cancellationToken);
                var resp = result != null ? ReflectionHelper.ParseObject(result) : dto;
                await NotifyResult(MixDbCommandQueueAction.PUT, new MixDbAuditLogModel()
                {
                    Id = id,
                    MixDbName = _tableName,
                    Before = dto,
                    After = resp,
                    Body = dto
                });
                return Ok(ReflectionHelper.ParseObject(resp));
            }
            return BadRequest();
        }

        private async Task NotifyResult(MixDbCommandQueueAction action, MixDbAuditLogModel log)
        {
            try
            {
                string username = _idService.GetClaim(User, MixClaims.UserName);
                QueueService.PushMemoryQueue(
                    CurrentTenant.Id,
                    MixQueueTopics.MixBackgroundTasks,
                    MixQueueActions.MixDbEvent,
                    new MixDbEventCommand(username, action.ToString(), _tableName, log));

                var modifiedEntities = new List<ModifiedEntityModel>()
                {
                    new()
                    {
                        Id = log.Id,
                        CacheFolder = $"{MixFolders.MixDbCacheFolder}/{_tableName}",
                        Action = ViewModelAction.Update
                    }
                };
                var modifiedData = new JObject() {
                    new JProperty("modifiedEntities", JArray.FromObject(modifiedEntities))
                };
                await PortalHub.SendMessageAsync(new SignalRMessageModel()
                {
                    Action = MessageAction.NewQueueMessage,
                    Title = MixQueueTopics.MixViewModelChanged,
                    Message = ViewModelAction.Delete.ToString(),
                    Data = modifiedData.ToString(),
                    Type = MessageType.Success,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MixDbController Cannot Notify");
            }
        }

        [HttpPatch]
        public async Task<ActionResult<JObject>> Patch([FromBody] JObject obj, CancellationToken cancellationToken = default)
        {
            return Ok(await PatchHandler(obj, cancellationToken));
        }

        [HttpPost("data-relationship")]
        public async Task<IActionResult> CreateDataRelationship([FromBody] CreateDataRelationshipDto obj, CancellationToken cancellationToken = default)
        {
            await _mixDbDataService.CreateDataRelationshipAsync(_tableName, obj, _requestedBy, cancellationToken);
            return Ok();
        }

        [HttpDelete("data-relationship/{id}")]
        public async Task<IActionResult> DeleteDataRelationship(int id, CancellationToken cancellationToken = default)
        {
            var relDbName = GetRelationshipDbName();
            await _mixDbDataService.DeleteDataRelationshipAsync(_tableName, id, _requestedBy, cancellationToken);
            return Ok();
        }

        [HttpPatch("patch-many")]
        public async Task<IActionResult> PatchMany([FromBody] IEnumerable<JObject> lstObj,
                CancellationToken cancellationToken = default)
        {
            await PatchManyHandler(lstObj, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> Delete(string id, CancellationToken cancellationToken = default)
        {
            var result = await _mixDbDataService.GetByIdAsync(_tableName, id, string.Empty, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            var data = await _mixDbDataService.DeleteAsync(_tableName, id, _requestedBy, cancellationToken);
            await NotifyResult(MixDbCommandQueueAction.DELETE, new MixDbAuditLogModel()
            {
                Id = id,
                MixDbName = _tableName,
                Before = result
            });
            return data > 0 ? Ok() : NotFound();
        }


        #region Handler
        private async Task CreateDataRelationship(string parentName, string parentId, string childName, string childId, string username, CancellationToken cancellationToken)
        {
            var rel = new DataRelationshipModel()
            {
                ParentDatabaseName = parentName,
                ChildDatabaseName = childName
            };
            if (Guid.TryParse(parentId, out Guid guidParentId))
            {
                rel.GuidParentId = guidParentId;
            }
            if (int.TryParse(parentId, out int intParentId))
            {
                rel.ParentId = intParentId;
            }

            if (Guid.TryParse(childId, out Guid guidChildId))
            {
                rel.GuidChildId = guidChildId;
            }
            if (int.TryParse(childId, out int intChildId))
            {
                rel.ChildId = intChildId;
            }
            string relDbName = GetRelationshipDbName();
            await _mixDbDataService.CreateAsync(relDbName, JObject.FromObject(rel), _requestedBy, cancellationToken);
        }

        private string GetRelationshipDbName()
        {
            string relName = _mixDb.NamingConvention == MixDatabaseNamingConvention.SnakeCase
                            ? MixDatabaseNames.DATA_RELATIONSHIP_SNAKE_CASE
                            : MixDatabaseNames.DATA_RELATIONSHIP_TITLE_CASE;
            return _mixDb.MixDatabaseContextId.HasValue
                        ? $"{_mixDb.MixDatabaseContext.SystemName}_{relName}"
                        : MixDatabaseNames.SYSTEM_DATA_RELATIONSHIP;
        }

        private async Task PatchManyHandler(IEnumerable<JObject> lstObj, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (var obj in lstObj)
            {
                await PatchHandler(obj, cancellationToken);
            }
        }
        private async Task<JObject> PatchHandler(JObject objDto, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var id = await _mixDbDataService.ExtractIdAsync(_tableName, objDto);
                var obj = await _mixDbDataService.GetByIdAsync(_tableName, id, string.Empty, cancellationToken);
                if (obj == null)
                {
                    throw new MixException(MixErrorStatus.NotFound);
                }
                var log = new MixDbAuditLogModel()
                {
                    Id = id,
                    MixDbName = _tableName,
                    Before = obj.DeepClone() as JObject,
                    Body = objDto
                };

                string username = _idService.GetClaim(User, MixClaims.UserName);

                // Not use Reflection to keep title case 

                foreach (var prop in objDto.Properties())
                {
                    var propName = prop.Name;
                    if (obj.ContainsKey(propName))
                    {
                        obj[propName] = prop.Value;
                    }
                }
                await _mixDbDataService.UpdateAsync(_tableName, id, obj, _requestedBy, objDto.Properties().Select(m => m.Name), cancellationToken);

                log.After = obj;
                await NotifyResult(MixDbCommandQueueAction.PATCH, log);
                return obj;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        private async Task<PagingResponseModel<JObject>> SearchHandler(SearchMixDbRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await GetResult(await ParseSearchDto(request), cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        private async Task<SearchMixDbRequestModel> ParseSearchDto(SearchMixDbRequestDto request)
        {
            var result = new SearchMixDbRequestModel(request);
            result.Queries = await BuildSearchQueryAsync(request);

            if (request.RelatedDataRequests != null)
            {
                result.RelatedDataRequests = new List<SearchMixDbRequestModel>();
                foreach (var item in request.RelatedDataRequests)
                {
                    result.RelatedDataRequests.Add(await ParseSearchDto(item));
                }
            }
            return result;
        }

        private async Task<PagingResponseModel<JObject>> SearchManyToManyDataHandler(SearchMixDbRequestDto request)
        {
            try
            {
                if (request.ObjParentId == null)
                {
                    throw new MixException(MixErrorStatus.Badrequest, "Bad Request");
                }

                var relQuery = new List<MixQueryField>() {
                        new MixQueryField(_fieldNameService.ParentDatabaseName, request.ParentName),
                        new MixQueryField(_fieldNameService.ChildDatabaseName, _tableName)
                };
                var parentDb = await GetMixDatabase(request.ParentName);
                var childDb = await GetMixDatabase(_tableName);

                if (parentDb.Type == MixDatabaseType.GuidService)
                {
                    relQuery.Add(new(_fieldNameService.GuidParentId, request.ObjParentId));
                }
                else
                {
                    relQuery.Add(new(_fieldNameService.ParentId, request.ObjParentId));
                }

                var allowsRels = await _mixDbDataService.GetListByAsync(new SearchMixDbRequestModel()
                {
                    TableName = GetRelationshipDbName(),
                    Queries = relQuery
                });

                var queries = new List<MixQueryField>();

                if (request.Queries != null)
                {
                    foreach (var query in request.Queries)
                    {
                        queries.Add(new(query.FieldName, ParseSearchKeyword(query.CompareOperator, query.Value), query.CompareOperator));
                    }
                }
                queries.Add(
                    new(
                        _fieldNameService.Id,
                        childDb.Type == MixDatabaseType.GuidService
                        ? allowsRels.Select(m => m.Value<int>(_fieldNameService.GuidChildId)).ToList()
                        : allowsRels.Select(m => m.Value<int>(_fieldNameService.ChildId)).ToList(),
                        MixCompareOperator.InRange
                    ));

                var paging = new PagingRequestModel(request);
                var nestedData = await GetResult(new(_tableName, queries, paging));
                var items = new List<JObject>();

                foreach (var item in nestedData.Items)
                {
                    var relId = childDb.Type == MixDatabaseType.GuidService
                        ? allowsRels.FirstOrDefault(m => m.Value<Guid>(_fieldNameService.GuidChildId) == item.Value<Guid>(_fieldNameService.Id))
                        : allowsRels.FirstOrDefault(m => m.Value<int>(_fieldNameService.ChildId) == item.Value<int>(_fieldNameService.Id));
                    if (relId != null)
                        items.Add(
                            new JObject(
                                new JProperty(_fieldNameService.Id, relId.Value<int>(_fieldNameService.Id)),
                                new JProperty("data", item)
                            ));
                }

                return new PagingResponseModel<JObject>()
                {
                    Items = items,
                    PagingData = nestedData.PagingData
                }; ;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }


        private async Task<PagingResponseModel<JObject>> GetResult(
            SearchMixDbRequestModel request,
            CancellationToken cancellationToken = default)
        {
            request.TableName ??= _tableName;
            return await _mixDbDataService.GetPagingAsync(request, cancellationToken);
        }

        private async Task<List<MixQueryField>> BuildSearchQueryAsync(SearchMixDbRequestDto request)
        {
            var mixDb = await GetMixDatabase(request.MixDatabaseName);
            var queries = new List<MixQueryField>();
            if (request.ObjParentId != null)
            {
                queries.Add(new MixQueryField(_fieldNameService.GetParentId(request.ParentName), request.ObjParentId));
            }
            if (request.Queries != null)
            {
                foreach (var query in request.Queries)
                {
                    var col = mixDb.Columns.FirstOrDefault(m => m.SystemName == query.FieldName);
                    if (col != null || _fieldNameService.GetAllFieldName().Contains(query.FieldName))
                    {
                        queries.Add(new(query.FieldName,

                            ParseSearchKeyword(
                                query.CompareOperator,
                                query.Value,
                                col?.DataType
                                    ),
                            query.CompareOperator));
                    }
                }
            }

            AddCursorQuery(request, queries);
            return queries;
        }

        private void AddCursorQuery(SearchMixDbRequestDto request, List<MixQueryField> queries)
        {
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                var orderByCol = _mixDb.Columns.FirstOrDefault(m => m.SystemName == request.SortBy);
                object? val = request.After ?? request.Before;

                if (orderByCol != null)
                {
                    val = MixDbHelper.ParseObjectValue(orderByCol.DataType, val);
                }
                if (val != null)
                {
                    queries.Add(new(request.SortBy, val, request.After != null ? MixCompareOperator.GreaterThan : MixCompareOperator.LessThan));
                }
            }
        }

        private object ParseSearchKeyword(MixCompareOperator? searchMethod, object keyword, MixDataType? dataType = MixDataType.String)
        {
            if (keyword == null)
            {
                return keyword;
            }
            switch (searchMethod)
            {
                case MixCompareOperator.Like:
                case MixCompareOperator.ILike:
                    return $"%{keyword}%";
                case MixCompareOperator.InRange:
                case MixCompareOperator.NotInRange:
                    var arr = keyword.ToString().Split(',', StringSplitOptions.TrimEntries);
                    if (dataType != MixDataType.String)
                    {
                        List<object> result = [];
                        foreach (var item in arr)
                        {
                            result.Add(MixDbHelper.ParseObjectValue(dataType, item));
                        }
                        return result;
                    }
                    return arr;
                default:
                    return MixDbHelper.ParseObjectValue(dataType, keyword);
            }
        }

        private Operation ParseSearchOperation(MixCompareOperator? searchMethod)
        {
            return searchMethod switch
            {
                MixCompareOperator.Like => Operation.Like,
                MixCompareOperator.Equal => Operation.Equal,
                MixCompareOperator.NotEqual => Operation.NotEqual,
                MixCompareOperator.LessThanOrEqual => Operation.LessThanOrEqual,
                MixCompareOperator.LessThan => Operation.LessThan,
                MixCompareOperator.GreaterThan => Operation.GreaterThan,
                MixCompareOperator.GreaterThanOrEqual => Operation.GreaterThanOrEqual,
                MixCompareOperator.InRange => Operation.In,
                MixCompareOperator.NotInRange => Operation.NotIn,
                _ => Operation.Equal
            };
        }

        #endregion

        #region Private

        private List<MixQueryField> GetAssociationQueries(string parentDatabaseName = null, string childDatabaseName = null, object? parentId = null, object? childId = null)
        {
            var queries = new List<MixQueryField>();
            if (!string.IsNullOrEmpty(parentDatabaseName))
            {
                queries.Add(new MixQueryField(_fieldNameService.ParentDatabaseName, parentDatabaseName));
            }
            if (!string.IsNullOrEmpty(childDatabaseName))
            {
                queries.Add(new MixQueryField(_fieldNameService.ChildDatabaseName, childDatabaseName));
            }
            if (parentId != null)
            {
                if (_mixDb.Type == MixDatabaseType.GuidService)
                {
                    queries.Add(new MixQueryField(_fieldNameService.GuidParentId, Guid.Parse(parentId.ToString())));
                }
                else
                {
                    queries.Add(new MixQueryField(_fieldNameService.ParentId, parentId));
                }
            }
            if (childId != null)
            {
                if (_mixDb.Type == MixDatabaseType.GuidService)
                {
                    queries.Add(new MixQueryField(_fieldNameService.GuidChildId, Guid.Parse(childId.ToString())));
                }
                else
                {
                    queries.Add(new MixQueryField(_fieldNameService.ChildId, childId));
                }
            }
            return queries;
        }

        private async Task<MixDbDatabaseViewModel> GetMixDatabase(string tableName = null)
        {
            tableName ??= _tableName;
            string name = $"{typeof(MixDbDatabaseViewModel).FullName}_{tableName}";
            return await _memoryCache.TryGetValueAsync(
                name,
                cache =>
                {
                    cache.SlidingExpiration = TimeSpan.FromSeconds(20);
                    return MixDbDatabaseViewModel.GetRepository(_cmsUow, CacheService).GetSingleAsync(m => m.SystemName == tableName);
                }
                );
        }
        #endregion
    }
}