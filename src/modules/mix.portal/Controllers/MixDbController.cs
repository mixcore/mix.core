using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Azure;
using Mix.Database.Services;
using Mix.Heart.Helpers;
using Mix.Heart.Repository;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.Services;
using Mix.Shared.Models;
using Org.BouncyCastle.Ocsp;
using RepoDb;
using RepoDb.Enumerations;
using RepoDb.Interfaces;
using System.Reflection;

namespace Mix.Portal.Controllers
{
    [MixDatabaseAuthorize("")]
    [Route("api/v2/rest/mix-portal/mix-db/{name}")]
    [ApiController]
    public class MixDbController : MixTenantApiControllerBase
    {
        private const string createdByFieldName = "CreatedBy";
        private const string createdDateFieldName = "CreatedDateTime";
        private const string priorityFieldName = "Priority";
        private const string idFieldName = "Id";
        private const string parentIdFieldName = "ParentId";
        private const string childIdFieldName = "ChildId";
        private const string tenantIdFieldName = "MixTenantId";
        private const string statusFieldName = "Status";
        private const string isDeletedFieldName = "IsDeleted";
        private UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        private readonly MixRepoDbRepository _repository;
        private readonly MixMemoryCacheService _memoryCache;
        private readonly MixRepoDbRepository _associationRepository;
        private readonly MixCmsContext _context;
        private string _tableName;
        private MixDatabaseViewModel _database;
        private readonly MixIdentityService _idService;
        private readonly MixDbService _mixDbService;
        private static string _associationTableName = nameof(MixDatabaseAssociation);
        public MixDbController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCmsContext context,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService,
            MixRepoDbRepository repository,
            MixMemoryCacheService memoryCache,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            ICache cache,
            DatabaseService databaseService,
            MixIdentityService idService,
            MixDbService mixDbService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, queueService)
        {
            _context = context;
            _repository = repository;
            _associationRepository = new(cache, databaseService, cmsUOW);
            _associationRepository.InitTableName(_associationTableName);
            _cmsUOW = cmsUOW;
            _memoryCache = memoryCache;
            _idService = idService;
            _mixDbService = mixDbService;
        }

        #region Overrides

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _tableName = RouteData?.Values["name"].ToString();
            _repository.InitTableName(_tableName);
            base.OnActionExecuting(context);
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
            string filename = $"{_tableName}_{DateTime.UtcNow.ToString("dd-MM-yyyy-hh-mm-ss")}";
            string exportPath = $"{MixFolders.ExportFolder}/mix-db/{_tableName}/";
            var file = MixCmsHelper.ExportJObjectToExcel(result.Items.ToList(), _tableName, exportPath, filename, null);
            return Ok(file);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<JObject>> GetSingle(int id, [FromQuery] bool loadNestedData)
        {
            var result = await _mixDbService.GetById(_tableName, id, loadNestedData);
            return result != default ? Ok(result) : NotFound(id);
        }

        [HttpGet("get-by-parent/{parentType}/{parentId}")]
        public async Task<ActionResult<JObject>> GetSingleByParent(MixContentType parentType, string parentId, [FromQuery] bool loadNestedData)
        {
            dynamic obj = await _repository.GetSingleByParentAsync(parentType, parentId);
            if (obj != null)
            {
                var data = ReflectionHelper.ParseObject(obj);
                var database = await GetMixDatabase();
                foreach (var item in database.Relationships)
                {
                    if (loadNestedData)
                    {

                        List<QueryField> queries = GetAssociatoinQueries(item.SourceDatabaseName, item.DestinateDatabaseName, data.id);
                        var associations = await _associationRepository.GetListByAsync(queries);
                        if (associations.Count > 0)
                        {
                            var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>(childIdFieldName)).ToList();
                            _repository.InitTableName(item.DestinateDatabaseName);
                            List<QueryField> query = new() { new(idFieldName, Operation.In, nestedIds) };
                            var nestedData = await _repository.GetListByAsync(query);
                            data.Add(new JProperty(item.DisplayName, JArray.FromObject(nestedData)));
                        }
                    }
                    else
                    {
                        data.Add(new JProperty($"{item.DisplayName}Url", $"{CurrentTenant.Configurations.Domain}/api/v2/rest/mix-portal/mix-db/{item.DestinateDatabaseName}?ParentId={data.id}&ParentName={item.SourceDatabaseName}"));
                    }
                }
                return Ok(data);
            }
            return NotFound();
            //throw new MixException(MixErrorStatus.NotFound, id);
        }

        [PreventDuplicateFormSubmission]
        [HttpPost]
        public async Task<ActionResult<object>> Create(JObject dto)
        {
            JObject obj = new JObject();
            foreach (var pr in dto.Properties())
            {
                obj.Add(new JProperty(pr.Name.ToTitleCase(), pr.Value));
            }
            if (!obj.ContainsKey(createdDateFieldName))
            {
                obj.Add(new JProperty(createdDateFieldName, DateTime.UtcNow));
            }
            if (!obj.ContainsKey(priorityFieldName))
            {
                obj.Add(new JProperty(priorityFieldName, 0));
            }
            if (!obj.ContainsKey(tenantIdFieldName))
            {
                obj.Add(new JProperty(tenantIdFieldName, CurrentTenant.Id));
            }

            if (!obj.ContainsKey(statusFieldName))
            {
                obj.Add(new JProperty(statusFieldName, MixContentStatus.Published.ToString()));
            }

            if (!obj.ContainsKey(isDeletedFieldName))
            {
                obj.Add(new JProperty(isDeletedFieldName, false));
            }
            var data = await _repository.InsertAsync(obj);

            if (data > 0)
            {
                var result = await _repository.GetSingleAsync(data);
                return Ok(ReflectionHelper.ParseObject(result));
            }
            return BadRequest();
        }

        [PreventDuplicateFormSubmission]
        [HttpPut("{id}")]
        public async Task<ActionResult<object>> Update(int id, [FromBody] JObject dto)
        {
            JObject obj = new JObject();
            foreach (var pr in dto.Properties())
            {
                obj.Add(new JProperty(pr.Name.ToTitleCase(), pr.Value));
            }
            if (!obj.ContainsKey(tenantIdFieldName))
            {
                obj.Add(new JProperty(tenantIdFieldName, CurrentTenant.Id));
            }
            var data = await _repository.UpdateAsync(obj);
            if (data != null)
            {
                return Ok(ReflectionHelper.ParseObject(await _repository.GetSingleAsync(id)));
            }
            return BadRequest();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JObject fields)
        {
            var data = await _repository.GetSingleAsync(id);
            if (data == null)
            {
                return NotFound();
            }

            JObject obj = ReflectionHelper.ParseObject(data);
            foreach (var prop in fields.Properties())
            {
                if (obj.ContainsKey(prop.Name))
                {
                    obj[prop.Name] = prop.Value;
                }
            }
            await _repository.UpdateAsync(obj);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> Delete(int id)
        {
            var data = await _repository.DeleteAsync(id);
            //Expression<Func<MixDatabaseAssociation, bool>> associationPredicate = m => m.ParentDatabaseName == _tableName && m.ParentId == id;
            //associationPredicate = associationPredicate.Or(m => m.ChildDatabaseName == _tableName && m.ChildId == id);
            //await _associationRepository.DeleteManyAsync(associationPredicate);
            var childAssociationsQueries = GetAssociatoinQueries(parentDatabaseName: _tableName, parentId: id);
            var parentAssociationsQueries = GetAssociatoinQueries(childDatabaseName: _tableName, childId: id);
            _repository.InitTableName(_associationTableName);
            await _repository.DeleteAsync(childAssociationsQueries);
            await _repository.DeleteAsync(parentAssociationsQueries);
            return data > 0 ? Ok() : NotFound();
        }


        #region Handler

        private async Task<PagingResponseModel<JObject>> SearchHandler(SearchMixDbRequestDto request)
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


        private async Task<PagingResponseModel<JObject>> GetResult(IEnumerable<QueryField> queries, PagingRequestModel paging, bool loadNestedData)
        {
            var result = await _repository.GetPagingAsync(queries, paging);

            var items = new List<JObject>();
            var database = await GetMixDatabase();

            foreach (var item in result.Items)
            {
                var data = ReflectionHelper.ParseObject(item);
                if (loadNestedData)
                {
                    foreach (var rel in database.Relationships)
                    {
                        var id = data.Value<int>("id");

                        List<QueryField> nestedQueries = GetAssociatoinQueries(rel.SourceDatabaseName, rel.DestinateDatabaseName, id);
                        var associations = await _associationRepository.GetListByAsync(nestedQueries);
                        if (associations.Count > 0)
                        {
                            var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>(childIdFieldName)).ToList();
                            _repository.InitTableName(rel.DestinateDatabaseName);
                            List<QueryField> query = new() { new(idFieldName, Operation.In, nestedIds) };
                            var nestedData = await _repository.GetListByAsync(query);
                            data.Add(new JProperty(rel.DisplayName, ReflectionHelper.ParseArray(nestedData)));
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
            if (request.ParentId.HasValue)
            {
                _database = await MixDatabaseViewModel.GetRepository(_cmsUOW).GetSingleAsync(m => m.SystemName == _tableName);
                if (_database.Type == MixDatabaseType.AdditionalData || _database.Type == MixDatabaseType.GuidAdditionalData)
                {
                    queries.Add(new(parentIdFieldName, request.ParentId));
                }
                else
                {
                    var allowsIds = _cmsUOW.DbContext.MixDatabaseAssociation
                            .Where(m => m.ParentDatabaseName == request.ParentName && m.ParentId == request.ParentId.Value && m.ChildDatabaseName == _tableName)
                            .Select(m => m.ChildId).ToList();
                    queries.Add(new(idFieldName, Operation.In, allowsIds));
                }
            }

            if (request.Queries != null)
            {
                foreach (var query in request.Queries)
                {
                    Operation op = ParseOperator(query.CompareOperator);
                    queries.Add(new(query.FieldName, op, query.Value));
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
            var queries = new List<QueryField>()
            {
                new QueryField(tenantIdFieldName, CurrentTenant.Id)
            };
            if (!string.IsNullOrEmpty(req.SearchColumns) && !string.IsNullOrEmpty(req.Keyword))
            {
                var searchColumns = req.SearchColumns.Replace(" ", string.Empty).Split(',');
                var operation = ParseSearchOperation(req.SearchMethod);
                var keyword = ParseSearchKeyword(req.SearchMethod, req.Keyword);

                foreach (var item in searchColumns)
                {
                    QueryField field = new QueryField(item, operation, keyword);
                    queries.Add(field);
                }
            }
            return queries;
        }

        private object ParseSearchKeyword(ExpressionMethod? searchMethod, string keyword)
        {
            return searchMethod switch
            {
                ExpressionMethod.Like => $"%{keyword}%",
                ExpressionMethod.In => keyword.Split(',', StringSplitOptions.TrimEntries),
                _ => keyword
            };
        }

        private Operation ParseSearchOperation(ExpressionMethod? searchMethod)
        {
            return searchMethod switch
            {
                ExpressionMethod.Like => Operation.Like,
                ExpressionMethod.Equal => Operation.Equal,
                ExpressionMethod.NotEqual => Operation.NotEqual,
                ExpressionMethod.LessThanOrEqual => Operation.LessThanOrEqual,
                ExpressionMethod.LessThan => Operation.LessThan,
                ExpressionMethod.GreaterThan => Operation.GreaterThan,
                ExpressionMethod.GreaterThanOrEqual => Operation.GreaterThanOrEqual,
                ExpressionMethod.In => Operation.In,
                _ => Operation.Equal
            };
        }

        #endregion

        #region Private

        private List<QueryField> GetAssociatoinQueries(string parentDatabaseName = null, string childDatabaseName = null, int? parentId = null, int? childId = null)
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
                queries.Add(new QueryField(parentIdFieldName, parentId));
            }
            if (childId.HasValue)
            {
                queries.Add(new QueryField(childIdFieldName, parentId));
            }
            return queries;
        }

        private async Task<MixDatabaseViewModel> GetMixDatabase()
        {
            string name = $"{typeof(MixDatabaseViewModel).FullName}_{_tableName}";
            return await _memoryCache.TryGetValueAsync(
                name,
                cache =>
                {
                    cache.SlidingExpiration = TimeSpan.FromSeconds(20);
                    return MixDatabaseViewModel.GetRepository(_cmsUOW).GetSingleAsync(m => m.SystemName == _tableName);
                }
                );
        }


        #endregion
    }
}
