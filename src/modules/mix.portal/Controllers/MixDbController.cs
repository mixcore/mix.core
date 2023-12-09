using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Azure.Amqp.Framing;
using Mix.Auth.Constants;
using Mix.Database.Constants;
using Mix.Database.Services;
using Mix.Heart.Helpers;
using Mix.Heart.Model;
using Mix.Lib.Interfaces;
using Mix.Lib.ViewModels.ReadOnly;
using Mix.RepoDb.Helpers;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.ViewModels;
using Mix.Service.Commands;
using Mix.Service.Interfaces;
using Mix.Service.Models;
using Mix.Shared.Models;
using Mix.Shared.Services;
using Mix.SignalR.Enums;
using Mix.SignalR.Hubs;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using RepoDb;
using RepoDb.Enumerations;
using RepoDb.Interfaces;
using System.Linq.Expressions;

namespace Mix.Portal.Controllers
{
    [MixDatabaseAuthorize("")]
    [Route("api/v2/rest/mix-portal/mix-db/{name}")]
    [ApiController]
    public class MixDbController : MixTenantApiControllerBase
    {
        private const string CreatedByFieldName = "CreatedBy";
        private const string ModifiedByFieldName = "ModifiedBy";
        private const string LastModifiedFieldName = "LastModified";
        private const string CreatedDateFieldName = "CreatedDateTime";
        private const string PriorityFieldName = "Priority";
        private const string IdFieldName = "Id";
        private const string ParentIdFieldName = "ParentId";
        private const string ChildIdFieldName = "ChildId";
        private const string TenantIdFieldName = "MixTenantId";
        private const string StatusFieldName = "Status";
        private const string IsDeletedFieldName = "IsDeleted";
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        private readonly MixRepoDbRepository _repository;
        private readonly IMixMemoryCacheService _memoryCache;
        private readonly IPortalHubClientService PortalHub;
        private readonly MixRepoDbRepository _associationRepository;
        private string _tableName;
        private MixDatabaseViewModel _database;
        private readonly MixIdentityService _idService;
        private readonly IMixDbService _mixDbService;
        private const string AssociationTableName = nameof(MixDatabaseAssociation);
        private IMixDbCommandHubClientService _mixDbCommandHubClientService;
        private RepoDbMixDatabaseViewModel _mixDb;
        public MixDbController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService,
            MixRepoDbRepository repository,
            IMixMemoryCacheService memoryCache,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            ICache cache,
            DatabaseService databaseService,
            MixIdentityService idService,
            IMixDbService mixDbService,
            IMixDbCommandHubClientService mixDbCommandHubClientService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, queueService, mixTenantService)
        {
            _repository = repository;
            _associationRepository = new(cache, databaseService, cmsUow);
            _associationRepository.InitTableName(AssociationTableName);
            _cmsUow = cmsUow;
            _memoryCache = memoryCache;
            _idService = idService;
            _mixDbService = mixDbService;
            _mixDbCommandHubClientService = mixDbCommandHubClientService;
            PortalHub = portalHub;
        }

        #region Overrides
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _tableName = RouteData?.Values["name"].ToString();
            _mixDb = await GetMixDatabase();
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
            PagingResponseModel<JObject> result = await _mixDbService.GetMyData(_tableName, req, username);
            return Ok(result);
        }

        [HttpGet("my-data/{id}")]
        public async Task<ActionResult<JObject>> GetMyDataById(int id, [FromQuery] bool loadNestedData)
        {
            string username = _idService.GetClaim(User, MixClaims.Username);
            JObject result = await _mixDbService.GetMyDataById(_tableName, username, id, loadNestedData);
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
            foreach (var item in data)
            {
                lstDto.Add(await MixDbHelper.ParseImportDtoToEntityAsync(item, _mixDb.Columns, CurrentTenant.Id, _idService.GetClaim(User, MixClaims.Username)));
            }

            var result = await _repository.InsertManyAsync(lstDto, _mixDb);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<JObject>> GetSingle(int id, [FromQuery] bool loadNestedData)
        {
            var result = await _mixDbService.GetById(_tableName, id, loadNestedData);
            string username = _idService.GetClaim(User, MixClaims.Username);
            QueueService.PushQueue(CurrentTenant.Id,
                                    MixQueueTopics.MixBackgroundTasks,
                                    MixQueueActions.MixDbEvent
                                    , new MixDbEventCommand(username, "GET", _tableName, result));
            return result != default ? Ok(result) : NotFound(id);
        }


        [HttpPut("update-priority/{dbName}/{id}")]
        public async Task<ActionResult<JObject>> UpdatePriority(string dbName, int id, [FromBody] UpdatePriorityDto<int> dto)
        {
            var data = await _mixDbService.GetById(dbName, id, false);
            if (data == null)
            {
                return NotFound();
            }

            var min = Math.Min((int)data[PriorityFieldName], dto.Priority);
            var max = Math.Max((int)data[PriorityFieldName], dto.Priority);

            var queryFields = new List<SearchQueryField>
            {
                new SearchQueryField(IdFieldName, id, MixCompareOperator.NotEqual),
                new SearchQueryField(PriorityFieldName, max, MixCompareOperator.LessThanOrEqual),
                new SearchQueryField(PriorityFieldName, max, MixCompareOperator.LessThanOrEqual)
            };

            var query = await _repository.GetListByAsync(queryFields);
            int start = min;
            if (dto.Priority == min)
            {
                data[PriorityFieldName] = dto.Priority;
                start++;
            }

            foreach (var item in query.OrderBy(m => m[PriorityFieldName]))
            {
                item.Priority = start;
                await _repository.UpdateAsync(item, _mixDb);
                start++;
            }

            if (dto.Priority == max)
            {
                data[PriorityFieldName] = start;
            }

            await _repository.UpdateAsync(data, _mixDb);

            return Ok();
        }

        [HttpGet("get-by-parent/{parentType}/{parentId}")]
        public async Task<ActionResult<JObject>> GetSingleByParent(MixContentType parentType, int parentId, [FromQuery] bool loadNestedData)
        {
            dynamic obj = await _repository.GetSingleByParentAsync(parentType, parentId);
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
                                var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>(ChildIdFieldName)).ToList();
                                _repository.InitTableName(item.DestinateDatabaseName);
                                List<QueryField> query = new() { new(IdFieldName, Operation.In, nestedIds) };
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
            dynamic obj = await _repository.GetSingleByParentAsync(parentType, parentId);
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
                                var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>(ChildIdFieldName)).ToList();
                                _repository.InitTableName(item.DestinateDatabaseName);
                                List<QueryField> query = new() { new(IdFieldName, Operation.In, nestedIds) };
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
            QueueService.PushQueue(CurrentTenant.Id, MixQueueTopics.MixDbCommand, MixDbCommandQueueActions.Create, obj);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<object>> Create(JObject dto)
        {
            JObject obj = await MixDbHelper.ParseDtoToEntityAsync(dto, _mixDb.Columns, CurrentTenant.Id, _idService.GetClaim(User, MixClaims.Username));
            string username = _idService.GetClaim(User, MixClaims.Username);
            var id = await _repository.InsertAsync(obj, _mixDb);
            var resp = await _repository.GetSingleAsync(id);
            var result = resp != null ? ReflectionHelper.ParseObject(resp) : obj;
            QueueService.PushQueue(CurrentTenant.Id, MixQueueTopics.MixBackgroundTasks, MixQueueActions.MixDbEvent,
                new MixDbEventCommand(username, "POST", _tableName, result));
            return Ok(result);
        }

        [PreventDuplicateFormSubmission]
        [HttpPut("{id}")]
        public async Task<ActionResult<object>> Update(int id, [FromBody] JObject dto)
        {
            JObject obj = await MixDbHelper.ParseDtoToEntityAsync(dto, _mixDb.Columns, CurrentTenant.Id, _idService.GetClaim(User, MixClaims.Username));
            string username = _idService.GetClaim(User, MixClaims.Username);
            var data = await _repository.UpdateAsync(obj, _mixDb);
            if (data != null)
            {
                var result = await _mixDbService.GetById(_tableName, id, true);

                QueueService.PushQueue(CurrentTenant.Id, MixQueueTopics.MixBackgroundTasks, MixQueueActions.MixDbEvent,
                                            new MixDbEventCommand(username, "PUT", _tableName, obj));
                var modifiedEnties = new List<ModifiedEntityModel>()
                {
                    new ModifiedEntityModel()
                    {
                        Id = id,
                        CacheFolder = $"{MixFolders.MixDbCacheFolder}/{_tableName}",
                        Action = ViewModelAction.Update
                    }
                };
                var modifiedData = new JObject() {
                    new JProperty("modifiedEntiies", modifiedEnties)
                };
                await PortalHub.SendMessageAsync(new SignalRMessageModel()
                {
                    Action = MessageAction.NewQueueMessage,
                    Title = MixQueueTopics.MixViewModelChanged,
                    Message = ViewModelAction.Delete.ToString(),
                    Data = modifiedData.ToString(),
                    Type = MessageType.Success,
                });
                return Ok(ReflectionHelper.ParseObject(result));
            }
            return BadRequest();
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] JObject obj, CancellationToken cancellationToken = default)
        {
            await PatchHandler(obj, cancellationToken);
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
        public async Task<ActionResult<object>> Delete(int id)
        {
            var data = await _repository.DeleteAsync(id);
            var childAssociationsQueries = GetAssociationQueries(parentDatabaseName: _tableName, parentId: id);
            var parentAssociationsQueries = GetAssociationQueries(childDatabaseName: _tableName, childId: id);
            _repository.InitTableName(AssociationTableName);
            await _repository.DeleteAsync(childAssociationsQueries);
            await _repository.DeleteAsync(parentAssociationsQueries);
            string username = _idService.GetClaim(User, MixClaims.Username);
            QueueService.PushQueue(CurrentTenant.Id, MixQueueTopics.MixBackgroundTasks, MixQueueActions.MixDbEvent,
                new MixDbEventCommand(username, "DELETE", _tableName, new(new JProperty("data", id))));
            return data > 0 ? Ok() : NotFound();
        }


        #region Handler

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
                var data = await _repository.GetSingleAsync(id);
                if (data == null)
                {
                    throw new MixException(MixErrorStatus.NotFound);
                }


                string username = _idService.GetClaim(User, MixClaims.Username);

                // Not use Reflection to keep title case 
                JObject obj = JObject.FromObject(data);

                foreach (var prop in objDto.Properties())
                {
                    var propName = prop.Name.ToTitleCase();
                    if (obj.ContainsKey(propName))
                    {
                        obj[propName] = prop.Value;
                    }
                }

                await _repository.UpdateAsync(objDto, _mixDb);
                QueueService.PushQueue(CurrentTenant.Id, MixQueueTopics.MixBackgroundTasks, MixQueueActions.MixDbEvent,
                    new MixDbEventCommand(username, "PATCH", _tableName, objDto));
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

                return await GetResult(queries, paging, request.LoadNestedData);
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
                var data = await _mixDbService.ParseDataAsync(_tableName, item);
                if (loadNestedData)
                {
                    foreach (var rel in _mixDb.Relationships)
                    {
                        var id = data.Value<int>("id");

                        List<QueryField> nestedQueries = GetAssociationQueries(rel.SourceDatabaseName, rel.DestinateDatabaseName, id);
                        var orderFields = new List<OrderField>
                        {
                            new("Priority", Order.Ascending)
                        };

                        var associations = await _associationRepository.GetListByAsync(nestedQueries, orderFields: orderFields);
                        if (associations is { Count: > 0 })
                        {
                            JArray nestedDataList = new();
                            var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>(ChildIdFieldName)).ToList();

                            _repository.InitTableName(rel.DestinateDatabaseName);

                            List<QueryField> query = new() { new(IdFieldName, Operation.In, nestedIds) };
                            var nestedData = await _repository.GetListByAsync(query);
                            foreach (var nestedId in nestedIds)
                            {
                                var nd = nestedData.FirstOrDefault(m => m.Id == nestedId);
                                if (nd is not null)
                                {
                                    nestedDataList.Add(await _mixDbService.ParseDataAsync(rel.DestinateDatabaseName, nd));
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

        private async Task<List<QueryField>> BuildSearchQueryAsync(SearchMixDbRequestDto request)
        {
            var queries = BuildSearchPredicate(request).ToList();
            if (request.ParentId.HasValue || request.GuidParentId.HasValue)
            {
                _database = await MixDatabaseViewModel.GetRepository(_cmsUow, CacheService).GetSingleAsync(m => m.SystemName == _tableName);

                if (_database.Type == MixDatabaseType.AdditionalData || _database.Type == MixDatabaseType.GuidAdditionalData)
                {
                    queries.Add(new(ParentIdFieldName, _database.Type == MixDatabaseType.AdditionalData ? request.ParentId : request.GuidParentId));
                }
                else
                {
                    Expression<Func<MixDatabaseAssociation, bool>> predicate = m =>
                        m.ParentDatabaseName == request.ParentName
                                        && m.ChildDatabaseName == _tableName;
                    predicate = predicate.AndAlsoIf(request.ParentId.HasValue, m => m.ParentId == request.ParentId.Value);
                    predicate = predicate.AndAlsoIf(request.GuidParentId.HasValue, m => m.GuidParentId == request.GuidParentId.Value);
                    var allowsIds = _cmsUow.DbContext.MixDatabaseAssociation
                            .Where(predicate)
                            .Select(m => m.ChildId).ToList();
                    queries.Add(new(IdFieldName, Operation.In, allowsIds));
                }
            }

            if (request.Queries != null)
            {
                foreach (var query in request.Queries)
                {
                    Operation op = ParseOperator(query.CompareOperator);
                    queries.Add(new(query.FieldName, op, ParseSearchKeyword(query.CompareOperator, query.Value)));
                }
            }
            return queries;
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
            var operation = ParseSearchOperation(req.SearchMethod);
            var queries = new List<QueryField>()
            {
                new QueryField(TenantIdFieldName, CurrentTenant.Id)
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

        private List<QueryField> GetAssociationQueries(string parentDatabaseName = null, string childDatabaseName = null, int? parentId = null, int? childId = null)
        {
            var queries = new List<QueryField>();
            if (!string.IsNullOrEmpty(parentDatabaseName))
            {
                queries.Add(new QueryField("ParentDatabaseName", parentDatabaseName));
            }
            if (!string.IsNullOrEmpty(childDatabaseName))
            {
                queries.Add(new QueryField("ChildDatabaseName", childDatabaseName));
            }
            if (parentId.HasValue)
            {
                queries.Add(new QueryField(ParentIdFieldName, parentId));
            }
            if (childId.HasValue)
            {
                queries.Add(new QueryField(ChildIdFieldName, childId));
            }
            return queries;
        }

        private async Task<RepoDbMixDatabaseViewModel> GetMixDatabase()
        {
            string name = $"{typeof(RepoDbMixDatabaseViewModel).FullName}_{_tableName}";
            return await _memoryCache.TryGetValueAsync(
                name,
                cache =>
                {
                    cache.SlidingExpiration = TimeSpan.FromSeconds(20);
                    return RepoDbMixDatabaseViewModel.GetRepository(_cmsUow, CacheService).GetSingleAsync(m => m.SystemName == _tableName);
                }
                );
        }

        #endregion
    }
}