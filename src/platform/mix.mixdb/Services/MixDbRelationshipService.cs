using Microsoft.Extensions.Logging;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Heart.Helpers;
using Mix.Heart.Models;
using Mix.Mixdb.Dtos;
using Mix.Mixdb.Helpers;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.ViewModels;
using Mix.RepoDb.Models;
using Mix.RepoDb.ViewModels;
using Mix.Service.Services;
using Mix.Shared.Dtos;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;
using RepoDb;
using RepoDb.Enumerations;

namespace Mix.Mixdb.Services
{
    /// <summary>
    /// Service xử lý các mối quan hệ dữ liệu trong MixDb
    /// </summary>
    public class MixDbRelationshipService : IMixDbRelationshipService
    {
        private readonly IMixDbDataService _dataService;
        private readonly FieldNameService _fieldNameService;
        private readonly ILogger _logger;

        public MixDbRelationshipService(
            IMixDbDataService dataService,
            FieldNameService fieldNameService,
            ILogger logger)
        {
            _dataService = dataService;
            _fieldNameService = fieldNameService;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task LoadNestedDataAsync(string tableName, JObject item, List<SearchMixDbRequestModel> relatedDataRequests, CancellationToken cancellationToken)
        {
            try
            {
                if (relatedDataRequests != null)
                {

                    var db = await _dataService.GetMixDb(tableName);
                    foreach (var req in relatedDataRequests)
                    {
                        var rel = db.Relationships.FirstOrDefault(m => m.DestinateTableName == req.TableName);
                        if (rel == null)
                        {
                            _logger.LogWarning($"Relationship not found for table {req.TableName}");
                            continue;
                        }

                        if (rel.Type == MixDbTableRelationshipType.OneToMany)
                        {
                            await LoadOneToMany(tableName, item, rel, req.Clone(), cancellationToken);
                        }
                        if (rel.Type == MixDbTableRelationshipType.ManyToMany)
                        {
                            await LoadManyToMany(tableName, item, rel, req.Clone(), cancellationToken);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when loading related data for table {tableName}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task LoadOneToMany(string tableName, JObject item, MixDbTableRelationshipViewModel rel, SearchMixDbRequestModel req, CancellationToken cancellationToken)
        {
            try
            {
                req.Queries ??= new();
                req.Queries.Add(new MixQueryField(rel.SourceColumnName, await _dataService.ExtractIdAsync(tableName, item)));
                var data = await _dataService.GetPagingAsync(req, cancellationToken: cancellationToken);
                item.Add(new JProperty(rel.DisplayName, ReflectionHelper.ParseObject(data)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when loading one-to-many relationship data for table {tableName}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task LoadManyToMany(string tableName, JObject item, MixDbTableRelationshipViewModel rel, SearchMixDbRequestModel req, CancellationToken cancellationToken)
        {
            try
            {
                var relQuery = new List<QueryField>() {
                    new QueryField(_fieldNameService.ParentDatabaseName, rel.SourceTableName),
                    new QueryField(_fieldNameService.ChildDatabaseName, rel.DestinateTableName)
                };
                var parentDb = await _dataService.GetMixDb(rel.SourceTableName);
                var childDb = await _dataService.GetMixDb(rel.DestinateTableName);

                if (parentDb.Type == MixDbTableType.GuidService)
                {
                    relQuery.Add(new(_fieldNameService.GuidParentId, await _dataService.ExtractIdAsync(rel.SourceTableName, item)));
                }
                else
                {
                    relQuery.Add(new(_fieldNameService.ParentId, await _dataService.ExtractIdAsync(rel.DestinateTableName, item)));
                }

                var allowsRels = await _dataService.GetListByAsync(GetRelationshipDbName(), relQuery);
                if (allowsRels != null)
                {
                    req.Queries ??= new List<MixQueryField>();
                    req.Queries.Add(
                        new(
                            _fieldNameService.Id,
                            childDb.Type == MixDbTableType.GuidService
                            ? allowsRels.Select(m => m.Value<int>(_fieldNameService.GuidChildId)).ToList()
                            : allowsRels.Select(m => m.Value<int>(_fieldNameService.ChildId)).ToList(),
                            MixCompareOperator.InRange
                    ));
                    var data = await _dataService.GetPagingAsync(req, cancellationToken: cancellationToken);
                    item.Add(new JProperty(rel.DisplayName, data));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when loading many-to-many relationship data for table {tableName}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<SearchMixDbRequestModel>?> ParseRelatedDataRequests(string? selectFieldNames, string tableName)
        {
            if (string.IsNullOrEmpty(selectFieldNames))
            {
                return default;
            }
            var fields = selectFieldNames.Split(',');
            var mixDb = await _dataService.GetMixDb(tableName);
            return mixDb.Relationships.Where(m => fields.Contains(m.DisplayName)).Select(m => new SearchMixDbRequestModel()
            {
                TableName = m.DestinateTableName,
                Paging = new PagingRequestModel()
            }).ToList();
        }

        private string GetRelationshipDbName()
        {
            var mixDb = _dataService.GetMixDb(_fieldNameService.NamingConvention == MixDatabaseNamingConvention.SnakeCase
                            ? MixDbTableNames.DATA_RELATIONSHIP_SNAKE_CASE
                            : MixDbTableNames.DATA_RELATIONSHIP_TITLE_CASE);
            return $"{mixDb.Result?.MixDbDatabase.SystemName}_{MixDbTableNames.SYSTEM_DATA_RELATIONSHIP}";
        }
    }
}