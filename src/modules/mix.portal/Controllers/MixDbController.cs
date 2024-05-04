using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Database.Services;
using Mix.Heart.Helpers;
using Mix.Heart.Model;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.RepoDb.Dtos;
using Mix.RepoDb.Helpers;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.ViewModels;
using Mix.Service.Commands;
using Mix.Service.Interfaces;
using Mix.Service.Models;
using Mix.Shared.Models;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using RepoDb;
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
        private readonly MixRepoDbRepository _repository;
        private readonly IMixMemoryCacheService _memoryCache;
        private readonly IPortalHubClientService PortalHub;
        private readonly MixRepoDbRepository _associationRepository;
        private string _tableName;
        private MixDatabaseViewModel _database;
        private readonly MixIdentityService _idService;
        private readonly IMixDbDataService _mixDbDataService;
        private const string AssociationTableName = nameof(MixDatabaseAssociation);
        private IMixDbCommandHubClientService _mixDbCommandHubClientService;
        private RepoDbMixDatabaseViewModel _mixDb;
        private FieldNameService _fieldNameService;
        public MixDbController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IMemoryQueueService<MessageQueueModel> queueService,
            MixRepoDbRepository repository,
            IMixMemoryCacheService memoryCache,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            ICache cache,
            DatabaseService databaseService,
            MixIdentityService idService,
            IMixDbDataService mixDbService,
            IMixDbCommandHubClientService mixDbCommandHubClientService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService,
            ILogger<MixDbController> logger)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, queueService, mixTenantService)
        {
            _repository = repository;
            _associationRepository = new(cache, databaseService);
            _associationRepository.InitTableName(AssociationTableName);
            _cmsUow = cmsUow;
            _memoryCache = memoryCache;
            _idService = idService;
            _mixDbDataService = mixDbService;
            _mixDbCommandHubClientService = mixDbCommandHubClientService;
            PortalHub = portalHub;
            _logger = logger;
        }

        #region Overrides
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _tableName = RouteData?.Values["name"].ToString();
            _mixDb = await GetMixDatabase();
            _fieldNameService = new FieldNameService(_mixDb.NamingConvention);
            if (_mixDb.MixDatabaseContextId.HasValue)
            {
                _repository.Init(_tableName, _mixDb.MixDatabaseContext.DatabaseProvider, _mixDb.MixDatabaseContext.ConnectionString);
            }
            else
            {
                _repository.InitTableName(_tableName);
            }
            await base.OnActionExecutionAsync(context, next);
        }

        #endregion

        [HttpGet("my-data")]
        public async Task<ActionResult<PagingResponseModel<JObject>>> MyData([FromQuery] SearchMixDbRequestDto req)
        {
            string username = _idService.GetClaim(User, MixClaims.Username);
            PagingResponseModel<JObject> result = await _mixDbDataService.GetMyData(_tableName, req, username);
            return Ok(result);
        }

        [HttpGet("my-data/{id}")]
        public async Task<ActionResult<JObject>> GetMyDataById(string id, [FromQuery] bool loadNestedData)
        {
            string username = _idService.GetClaim(User, MixClaims.Username);
            JObject result = await _mixDbDataService.GetMyDataById(_tableName, username, GetId(id), loadNestedData);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<PagingResponseModel<JObject>>> Get([FromQuery] SearchMixDbRequestDto req)
        {
            var result = await SearchHandler(req);

            return Ok(result);
        }

        [HttpPost("by-ids")]
        public async Task<ActionResult<PagingResponseModel<JObject>>> GetByIds([FromBody] GetIdsDto req)
        {
            var searchDto = new SearchMixDbRequestDto
            {
                SearchColumns = "id",
                SearchMethod = MixCompareOperator.InRange,
                Keyword = string.Join(',', req.Ids)
            };

            var result = await SearchHandler(searchDto);

            return Ok(result);
        }

        [HttpPost("filter")]
        public async Task<ActionResult<PagingResponseModel<JObject>>> Filter([FromBody] SearchMixDbRequestDto req)
        {
            var result = await SearchHandler(req);

            return Ok(result);
        }

        [HttpPost("nested-data/filter")]
        public async Task<ActionResult<PagingResponseModel<JObject>>> NestedFilter([FromBody] SearchMixDbRequestDto req)
        {
            if (req.Relationship == MixDatabaseRelationshipType.ManyToMany)
            {
                var result = await SearchManyToManyDataHandler(req);
                return Ok(result);
            }
            else
            {
                return Ok(await SearchHandler(req));
            }
        }

        [HttpPost("export")]
        public async Task<ActionResult<FileModel>> Export([FromBody] SearchMixDbRequestDto req)
        {
            var result = await SearchHandler(req);
            string filename = $"{_tableName}_{DateTime.UtcNow:dd-MM-yyyy-hh-mm-ss}";
            string exportPath = $"{MixFolders.ExportFolder}/mix-db/{_tableName}/";
            var file = MixCmsHelper.ExportJObjectToExcel(result.Items.ToList(), _tableName, exportPath, filename, null);
            return Ok(file);
        }

        [HttpPost("import")]
        public async Task<ActionResult> Import([FromForm] IFormFile file)
        {
            var data = MixCmsHelper.LoadExcelFileData(file);
            List<JObject> lstDto = new();
            var username = _idService.GetClaim(User, MixClaims.Username);
            foreach (var item in data)
            {
                lstDto.Add(await MixDbHelper.ParseDtoToEntityAsync(item, _mixDb.Type, _mixDb.Columns, _fieldNameService, username: username));
            }

            var result = await _repository.InsertManyAsync(lstDto, _mixDb);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<JObject>> GetSingle(string id, [FromQuery] bool loadNestedData)
        {
            object objId = GetId(id);
            var result = await _mixDbDataService.GetById(_tableName, objId, loadNestedData);
            return result != default ? Ok(result) : NotFound(id);
        }

        [HttpPut("update-priority/{dbName}/{id}")]
        public async Task<ActionResult<JObject>> UpdatePriority(string dbName, string id, [FromBody] UpdatePriorityDto<int> dto)
        {
            object objId = GetId(id);
            var data = await _mixDbDataService.GetById(dbName, objId, false);
            if (data == null)
            {
                return NotFound();
            }

            var min = Math.Min((int)data[_fieldNameService.Priority], dto.Priority);
            var max = Math.Max((int)data[_fieldNameService.Priority], dto.Priority);

            var queryFields = new List<SearchQueryField>
            {
                new SearchQueryField(_fieldNameService.Id, id, MixCompareOperator.NotEqual),
                new SearchQueryField(_fieldNameService.Priority, max, MixCompareOperator.LessThanOrEqual),
                new SearchQueryField(_fieldNameService.Priority, max, MixCompareOperator.LessThanOrEqual)
            };

            var query = await _repository.GetListByAsync(queryFields);
            int start = min;
            if (dto.Priority == min)
            {
                data[_fieldNameService.Priority] = dto.Priority;
                start++;
            }

            foreach (var item in query.OrderBy(m => m[_fieldNameService.Priority]))
            {
                item.Priority = start;
                await _repository.UpdateAsync(objId, item, _mixDb);
                start++;
            }

            if (dto.Priority == max)
            {
                data[_fieldNameService.Priority] = start;
            }

            await _repository.UpdateAsync(id, data, _mixDb);

            return Ok();
        }

        [HttpGet("get-by-parent/{parentType}/{parentId}")]
        public async Task<ActionResult<JObject>> GetSingleByParent(MixContentType parentType, int parentId, [FromQuery] bool loadNestedData)
        {
            dynamic obj = await _repository.GetSingleByParentAsync(parentType, parentId, _fieldNameService);
            if (obj != null)
            {
                try
                {
                    var data = ReflectionHelper.ParseObject(obj);
                    foreach (var item in _mixDb.Relationships)
                    {
                        if (loadNestedData)
                        {

                            List<QueryField> queries = GetAssociationQueries(item.SourceDatabaseName, item.DestinateDatabaseName, data.id);
                            var associations = await _associationRepository.GetListByAsync(queries);
                            if (associations is { Count: > 0 })
                            {
                                var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>(_fieldNameService.ChildId)).ToList();
                                _repository.InitTableName(item.DestinateDatabaseName);
                                List<QueryField> query = new() { new(_fieldNameService.Id, Operation.In, nestedIds) };
                                var nestedData = await _repository.GetListByAsync(query);
                                if (nestedData != null)
                                {
                                    data.Add(new JProperty(item.DisplayName, JArray.FromObject(nestedData)));
                                }
                            }
                        }
                        else
                        {
                            data.Add(new JProperty(
                                    $"{item.DisplayName}Url",
                                    $"{CurrentTenant.Configurations.Domain}/api/v2/rest/mix-portal/mix-db/{item.DestinateDatabaseName}?ParentId={data.id}&ParentName={item.SourceDatabaseName}"));
                        }
                    }
                    return Ok(data);
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
            return NotFound();
            //throw new MixException(MixErrorStatus.NotFound, id);
        }

        [HttpGet("get-by-guid-parent/{parentType}/{parentId}")]
        public async Task<ActionResult<JObject>> GetSingleByGuidParent(MixContentType parentType, Guid parentId, [FromQuery] bool loadNestedData)
        {
            dynamic obj = await _repository.GetSingleByParentAsync(parentType, parentId, _fieldNameService);
            if (obj != null)
            {
                try
                {
                    var data = ReflectionHelper.ParseObject(obj);
                    foreach (var item in _mixDb.Relationships)
                    {
                        if (loadNestedData)
                        {

                            List<QueryField> queries = GetAssociationQueries(item.SourceDatabaseName, item.DestinateDatabaseName, data.id);
                            var associations = await _associationRepository.GetListByAsync(queries);
                            if (associations is { Count: > 0 })
                            {
                                var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>(_fieldNameService.ChildId)).ToList();
                                _repository.InitTableName(item.DestinateDatabaseName);
                                List<QueryField> query = new() { new(_fieldNameService.Id, Operation.In, nestedIds) };
                                var nestedData = await _repository.GetListByAsync(query);
                                if (nestedData != null)
                                {
                                    data.Add(new JProperty(item.DisplayName, JArray.FromObject(nestedData)));
                                }
                            }
                        }
                        else
                        {
                            data.Add(new JProperty(
                                    $"{item.DisplayName}Url",
                                    $"{CurrentTenant.Configurations.Domain}/api/v2/rest/mix-portal/mix-db/{item.DestinateDatabaseName}?ParentId={data.id}&ParentName={item.SourceDatabaseName}"));
                        }
                    }
                    return Ok(data);
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
            return NotFound();
            //throw new MixException(MixErrorStatus.NotFound, id);
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
            QueueService.PushMemoryQueue(CurrentTenant.Id, MixQueueTopics.MixDbCommand, MixDbCommandQueueAction.Create.ToString(), obj);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<object>> Create(JObject dto)
        {
            string username = _idService.GetClaim(User, MixClaims.Username);
            JObject obj = await MixDbHelper.ParseDtoToEntityAsync(dto, _mixDb.Type, _mixDb.Columns, _fieldNameService, CurrentTenant.Id, username);
            var id = await _repository.InsertAsync(obj, _mixDb);
            var resp = await _repository.GetSingleAsync(new QueryField(_fieldNameService.Id, id));

            if (obj.ContainsKey(_fieldNameService.ParentId))
            {
                await CreateDataRelationship(obj.Value<string>(_fieldNameService.ParentDatabaseName), obj.Value<string>(_fieldNameService.ParentId), obj.Value<string>(_fieldNameService.ChildDatabaseName), id.ToString(), username);
            }

            var result = resp != null ? ReflectionHelper.ParseObject(resp) : obj;
            await NotifyResult(id, result);
            return Ok(result);
        }


        [PreventDuplicateFormSubmission]
        [HttpPut("{id}")]
        public async Task<ActionResult<object>> Update(string id, [FromBody] JObject dto)
        {
            JObject obj = await MixDbHelper.ParseDtoToEntityAsync(dto, _mixDb.Type, _mixDb.Columns, _fieldNameService, CurrentTenant.Id, _idService.GetClaim(User, MixClaims.Username));

            var data = await _repository.UpdateAsync(GetId(id), obj, _mixDb);

            if (data != null)
            {
                var result = await _repository.GetSingleAsync(new QueryField(_fieldNameService.Id, GetId(id)));
                var resp = result != null ? ReflectionHelper.ParseObject(result) : obj;
                await NotifyResult(GetId(id), resp);
                return Ok(ReflectionHelper.ParseObject(resp));
            }
            return BadRequest();
        }

        private async Task NotifyResult(object id, JObject obj)
        {
            try
            {
                string username = _idService.GetClaim(User, MixClaims.Username);
                QueueService.PushMemoryQueue(
                    CurrentTenant.Id,
                    MixQueueTopics.MixBackgroundTasks,
                    MixQueueActions.MixDbEvent,
                    new MixDbEventCommand(username, MixDbCommandQueueAction.Update.ToString(), _tableName, obj));

                var modifiedEntities = new List<ModifiedEntityModel>()
                {
                    new()
                    {
                        Id = id,
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
        public async Task<IActionResult> Patch([FromBody] JObject obj, CancellationToken cancellationToken = default)
        {
            await PatchHandler(obj, cancellationToken);
            return Ok();
        }

        [HttpPost("data-relationship")]
        public async Task<IActionResult> CreateDataRelationship([FromBody] CreateDataRelationshipDto obj, CancellationToken cancellationToken = default)
        {
            await _mixDbDataService.CreateDataRelationship(obj, cancellationToken);
            return Ok();
        }

        [HttpDelete("data-relationship/{id}")]
        public async Task<IActionResult> DeleteDataRelationship(int id, CancellationToken cancellationToken = default)
        {
            var relDbName = GetRelationshipDbName();
            await _mixDbDataService.DeleteDataRelationship(relDbName, id, cancellationToken);
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
        public async Task<ActionResult<object>> Delete(string id)
        {
            var objId = GetId(id);
            var data = await _repository.DeleteAsync(objId, _fieldNameService);
            var childAssociationsQueries = GetAssociationQueries(parentDatabaseName: _tableName, parentId: objId);
            var parentAssociationsQueries = GetAssociationQueries(childDatabaseName: _tableName, childId: objId);
            _repository.InitTableName(GetRelationshipDbName());
            await _repository.DeleteAsync(childAssociationsQueries);
            await _repository.DeleteAsync(parentAssociationsQueries);
            string username = _idService.GetClaim(User, MixClaims.Username);
            QueueService.PushMemoryQueue(CurrentTenant.Id, MixQueueTopics.MixBackgroundTasks, MixQueueActions.MixDbEvent,
                new MixDbEventCommand(username, MixDbCommandQueueAction.Delete.ToString(), _tableName, new(new JProperty("data", objId))));
            return data > 0 ? Ok() : NotFound();
        }


        #region Handler
        private async Task CreateDataRelationship(string parentName, string parentId, string childName, string childId, string username)
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
            _repository.InitTableName(relDbName);
            var relDb = await GetMixDatabase(relDbName);
            var fieldNameService = new FieldNameService(relDb.NamingConvention);
            var obj = await MixDbHelper.ParseDtoToEntityAsync(JObject.FromObject(rel), relDb.Type, relDb.Columns, fieldNameService, CurrentTenant.Id, username);
            await _repository.InsertAsync(obj, relDb);
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
            try
            {
                foreach (var obj in lstObj)
                {
                    await PatchHandler(obj, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                if (ex is not MixException)
                {
                    throw new MixException(MixErrorStatus.ServerError, ex);
                }
            }
        }
        private async Task PatchHandler(JObject objDto, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var id = objDto.Value<int>("id");
                var data = await _repository.GetSingleAsync(new QueryField(_fieldNameService.Id, id));
                if (data == null)
                {
                    throw new MixException(MixErrorStatus.NotFound);
                }


                string username = _idService.GetClaim(User, MixClaims.Username);

                // Not use Reflection to keep title case 
                JObject obj = JObject.FromObject(data);

                foreach (var prop in objDto.Properties())
                {
                    var propName = prop.Name;
                    if (obj.ContainsKey(propName))
                    {
                        obj[propName] = prop.Value;
                    }
                }
                var parsedObj = await MixDbHelper.ParseDtoToEntityAsync(obj, _mixDb.Type, _mixDb.Columns, _fieldNameService, CurrentTenant.Id, username);
                await _repository.UpdateAsync(id, parsedObj, _mixDb);
                QueueService.PushMemoryQueue(CurrentTenant.Id, MixQueueTopics.MixBackgroundTasks, MixQueueActions.MixDbEvent,
                    new MixDbEventCommand(username, MixDbCommandQueueAction.Patch.ToString(), _tableName, objDto));
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        private async Task<PagingResponseModel<JObject>> SearchHandler(SearchMixDbRequestDto request)
        {
            try
            {
                IEnumerable<QueryField> queries = await BuildSearchQueryAsync(request);

                var paging = new PagingRequestModel()
                {
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    SortBy = request.OrderBy,
                    SortDirection = request.Direction
                };

                var result = await GetResult(queries, paging, request.LoadNestedData);
                if (request.LoadNestedData)
                {
                    foreach (var item in result.Items)
                    {
                        item.Add(await _mixDbDataService.LoadNestedData(_mixDb, _fieldNameService, item.Value<int>(_fieldNameService.Id)));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        private async Task<PagingResponseModel<JObject>> SearchManyToManyDataHandler(SearchMixDbRequestDto request)
        {
            try
            {
                if (request.ObjParentId == null)
                {
                    throw new MixException(MixErrorStatus.Badrequest, "Bad Request");
                }

                var relDbName = GetRelationshipDbName();
                _repository.InitTableName(relDbName);
                var relQuery = new List<QueryField>() {
                        new QueryField(_fieldNameService.ParentDatabaseName, request.ParentName),
                        new QueryField(_fieldNameService.ChildDatabaseName, _tableName)
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

                var allowsRels = await _mixDbDataService.ParseListDataAsync(relDbName, await _repository.GetListByAsync(relQuery));

                _repository.InitTableName(_tableName);
                var queries = BuildSearchPredicate(request).ToList();

                if (request.Queries != null)
                {
                    foreach (var query in request.Queries)
                    {
                        Operation op = ParseOperator(query.CompareOperator);
                        queries.Add(new(query.FieldName, op, ParseSearchKeyword(query.CompareOperator, query.Value)));
                    }
                }
                queries.Add(
                    new(
                        _fieldNameService.Id,
                        Operation.In,
                        childDb.Type == MixDatabaseType.GuidService
                        ? allowsRels.Select(m => m.Value<int>(_fieldNameService.GuidChildId)).ToList()
                        : allowsRels.Select(m => m.Value<int>(_fieldNameService.ChildId)).ToList()
                    ));

                var paging = new PagingRequestModel()
                {
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    SortBy = request.OrderBy,
                    SortDirection = request.Direction
                };
                var nestedData = await GetResult(queries, paging, request.LoadNestedData);
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


        private async Task<PagingResponseModel<JObject>> GetResult(IEnumerable<QueryField> queries, PagingRequestModel paging, bool loadNestedData)
        {
            var result = await _repository.GetPagingAsync(queries, paging);

            var items = new List<JObject>();

            foreach (var item in result.Items)
            {
                var data = await _mixDbDataService.ParseDataAsync(_tableName, item);
                if (loadNestedData)
                {
                    foreach (var rel in _mixDb.Relationships)
                    {
                        var id = data.Value<int>(_fieldNameService.Id);

                        List<QueryField> nestedQueries = GetAssociationQueries(rel.SourceDatabaseName, rel.DestinateDatabaseName, id);
                        var orderFields = new List<OrderField>
                        {
                            new(paging.SortBy, paging.SortDirection == SortDirection.Asc? Order.Ascending: Order.Descending)
                        };

                        var associations = await _associationRepository.GetListByAsync(nestedQueries, orderFields: orderFields);
                        if (associations is { Count: > 0 })
                        {
                            JArray nestedDataList = new();
                            var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>(_fieldNameService.ChildId)).ToList();

                            _repository.InitTableName(rel.DestinateDatabaseName);

                            List<QueryField> query = new() { new(_fieldNameService.Id, Operation.In, nestedIds) };
                            var nestedData = await _repository.GetListByAsync(query);
                            foreach (var nestedId in nestedIds)
                            {
                                var nd = nestedData.FirstOrDefault(m => m.Id == nestedId);
                                if (nd is not null)
                                {
                                    nestedDataList.Add(await _mixDbDataService.ParseDataAsync(rel.DestinateDatabaseName, nd));
                                }
                            }

                            data.Add(new JProperty(rel.DisplayName, ReflectionHelper.ParseArray(nestedDataList)));
                        }
                        else
                        {
                            data.Add(new JProperty(rel.DisplayName, new JArray()));
                        }
                    }
                }
                items.Add(data);
            }
            return new PagingResponseModel<JObject> { Items = items, PagingData = result.PagingData };
        }

        private Task<List<QueryField>> BuildSearchQueryAsync(SearchMixDbRequestDto request)
        {
            var queries = BuildSearchPredicate(request).ToList();
            if (request.ObjParentId != null)
            {
                queries.Add(new QueryField(_fieldNameService.GetParentId(request.ParentName), request.ObjParentId));
            }
            if (request.Queries != null)
            {
                foreach (var query in request.Queries)
                {
                    Operation op = ParseOperator(query.CompareOperator);
                    queries.Add(new(query.FieldName, op, ParseSearchKeyword(query.CompareOperator, query.Value)));
                }
            }
            return Task.FromResult(queries);
        }

        private Operation ParseOperator(MixCompareOperator compareOperator)
        {
            switch (compareOperator)
            {
                case MixCompareOperator.Equal:
                    return Operation.Equal;
                case MixCompareOperator.Like:
                    return Operation.Like;
                case MixCompareOperator.NotEqual:
                    return Operation.NotEqual;
                case MixCompareOperator.Contain:
                    return Operation.In;
                case MixCompareOperator.NotContain:
                    return Operation.NotIn;
                case MixCompareOperator.InRange:
                    return Operation.In;
                case MixCompareOperator.NotInRange:
                    return Operation.NotIn;
                case MixCompareOperator.GreaterThanOrEqual:
                    return Operation.GreaterThanOrEqual;
                case MixCompareOperator.GreaterThan:
                    return Operation.GreaterThan;
                case MixCompareOperator.LessThanOrEqual:
                    return Operation.LessThanOrEqual;
                case MixCompareOperator.LessThan:
                    return Operation.LessThan;
                default:
                    return Operation.Equal;
            }
        }

        private IEnumerable<QueryField> BuildSearchPredicate(SearchMixDbRequestDto req)
        {
            req.OrderBy ??= _fieldNameService.Priority;
            var operation = ParseSearchOperation(req.SearchMethod);
            var queries = new List<QueryField>()
            {
                //new QueryField(_fieldNameService.TenantId, CurrentTenant.Id)
            };
            if (!string.IsNullOrEmpty(req.SearchColumns) && !string.IsNullOrEmpty(req.Keyword))
            {
                var searchColumns = req.SearchColumns.Replace(" ", string.Empty).Split(',');

                var keyword = ParseSearchKeyword(req.SearchMethod, req.Keyword);

                foreach (var item in searchColumns)
                {
                    QueryField field = new QueryField(item, operation, keyword);
                    queries.Add(field);
                }
            }
            return queries;
        }

        private object ParseSearchKeyword(MixCompareOperator? searchMethod, object keyword)
        {
            if (keyword == null)
            {
                return keyword;
            }
            return searchMethod switch
            {
                MixCompareOperator.Like => $"%{keyword}%",
                MixCompareOperator.InRange => keyword.ToString().Split(',', StringSplitOptions.TrimEntries),
                MixCompareOperator.NotInRange => keyword.ToString().Split(',', StringSplitOptions.TrimEntries),
                _ => keyword
            };
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

        private List<QueryField> GetAssociationQueries(string parentDatabaseName = null, string childDatabaseName = null, object? parentId = null, object? childId = null)
        {
            var queries = new List<QueryField>();
            if (!string.IsNullOrEmpty(parentDatabaseName))
            {
                queries.Add(new QueryField(_fieldNameService.ParentDatabaseName, parentDatabaseName));
            }
            if (!string.IsNullOrEmpty(childDatabaseName))
            {
                queries.Add(new QueryField(_fieldNameService.ChildDatabaseName, childDatabaseName));
            }
            if (parentId != null)
            {
                if (parentId.GetType() == typeof(Guid))
                {
                    queries.Add(new QueryField(_fieldNameService.GuidParentId, parentId));
                }
                else
                {
                    queries.Add(new QueryField(_fieldNameService.ParentId, parentId));
                }
            }
            if (childId != null)
            {
                if (childId.GetType() == typeof(Guid))
                {
                    queries.Add(new QueryField(_fieldNameService.GuidChildId, childId));
                }
                else
                {
                    queries.Add(new QueryField(_fieldNameService.ChildId, childId));
                }
            }
            return queries;
        }

        private async Task<RepoDbMixDatabaseViewModel> GetMixDatabase(string tableName = null)
        {
            tableName ??= _tableName;
            string name = $"{typeof(RepoDbMixDatabaseViewModel).FullName}_{tableName}";
            return await _memoryCache.TryGetValueAsync(
                name,
                cache =>
                {
                    cache.SlidingExpiration = TimeSpan.FromSeconds(20);
                    return RepoDbMixDatabaseViewModel.GetRepository(_cmsUow, CacheService).GetSingleAsync(m => m.SystemName == tableName);
                }
                );
        }
        private object GetId(string id)
        {
            try
            {
                if (_mixDb.Type == MixDatabaseType.GuidService)
                {
                    return Guid.Parse(id);
                }
                else
                {
                    return int.Parse(id);
                }
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }
        #endregion
    }
}