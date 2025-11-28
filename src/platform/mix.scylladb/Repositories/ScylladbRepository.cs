using Cassandra;
using Cassandra.Mapping;
using Mix.Constant.Enums;
using Mix.Heart.Model;
using Mix.Heart.Models;
using Mix.Scylladb.Builders;
using Mix.Shared.Dtos;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mix.Scylladb.Repositories
{
    public class ScylladbRepository
    {
        #region Properties
        private Cluster _cluster;
        private Cassandra.ISession _session;
        private IMapper _mapper;
        public string _connectionString { get; set; }
        private string _tableName;
        private string _keyspace;
        private bool _isRoot = true;
        #endregion

        #region Methods
        public void Connect(string connectionString)
        {
            if (_connectionString != connectionString)
            {
                _keyspace = Regex.Match(connectionString, "(Keyspace=)(\\w+)").Groups[2].Value;
                _cluster = Cluster.Builder()
                            .WithConnectionString(connectionString)
                            .Build();
                _session = _cluster.Connect(_keyspace);
                _mapper = new Mapper(_session);
                _connectionString = connectionString;
            }
        }
        public Task Disconnect()
        {
            //_cluster.Dispose();
            return Task.CompletedTask;
        }

        public async Task<JObject?> GetSingleByAsync(string tableName, MixQueryField query, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var commandText = StatementBuilder.CreateSelect(tableName, query);
            var data = await ExecuteCommand(commandText);
            var row = data.FirstOrDefault();
            return row.ParseJObject(data.Columns);
        }

        public async Task<JObject?> GetSingleByAsync(string tableName, List<MixQueryField> queries, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var commandText = StatementBuilder.CreateSelect(tableName, queries, MixConjunction.Or);
            var data = await ExecuteCommand(commandText);
            var row = data.FirstOrDefault();
            return row.ParseJObject(data.Columns);
        }

        public async Task<JObject?> GetByIdAsync(string tableName, Guid id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var statement =  new SimpleStatement($"SELECT * FROM {tableName} WHERE id = ?", id);
            var data = await ExecuteCommand(statement);
            var row = data.FirstOrDefault();
            return row == null ? default : row.ParseJObject(data.Columns);
        }

        public async Task InsertAsync(string tableName, Dictionary<string, object?> dic, CancellationToken cancellationToken = default)
        {
            if (dic is null)
            {
                throw new NullReferenceException();
            }
            var insertStatement = StatementBuilder.CreateInsert(tableName, dic);
            await _session.ExecuteAsync(insertStatement);
        }
        
        public async Task InsertManyAsync(string tableName, List<Dictionary<string, object>> dics, CancellationToken cancellationToken = default)
        {
            if (dics is null)
            {
                throw new NullReferenceException();
            }
            var insertStatement = StatementBuilder.CreateInsertMany(tableName, dics);
            await _session.ExecuteAsync(insertStatement);
        }

        public async Task UpdateAsync(string tableName, Guid id, Dictionary<string, object> dic)
        {
            var fields = dic.Keys.Where(m => string.Compare(m, "id", StringComparison.OrdinalIgnoreCase) != 0).ToArray();
            var values = dic.Where(m => string.Compare(m.Key, "id", StringComparison.OrdinalIgnoreCase) != 0).Select(m => m.Value);

            var updateStatement = new SimpleStatement(
                        $"""
                        UPDATE {tableName} SET {string.Join(" = ?,", fields).TrimEnd(',')}  = ? WHERE id = ?
                        """
                        , values.Append(id).ToArray());
            await _session.ExecuteAsync(updateStatement);
        }

        public async Task DeleteAsync(string tableName, Guid id)
        {
            var deleteStatement = StatementBuilder.CreateDelete(tableName, id);
            _session.Execute(deleteStatement);
            var result = await _session.ExecuteAsync(deleteStatement);
        }

        public async Task DeleteManyAsync(string tableName, List<MixQueryField> queries, CancellationToken cancellationToken = default)
        {
            var deleteStatement = StatementBuilder.CreateDelete(tableName, queries);
            _session.Execute(deleteStatement);
            var result = await _session.ExecuteAsync(deleteStatement);
        }

        public async Task<PagingResponseModel<JObject>> GetPagingAsync(
            string tableName,
            IEnumerable<MixQueryField> queryFields,
            PagingRequestModel pagingRequest,
            MixConjunction conjunction,
            IEnumerable<string>? selectFields = null,
            CancellationToken cancellationToken = default)
        {
            int pageSize = pagingRequest.PageSize ?? 100;
            var count = await CountAsync(tableName, queryFields, conjunction);
            var statement = StatementBuilder.CreateSelect(tableName, queryFields, conjunction, selectFields);
            statement
                .SetAutoPage(false)
                .SetPageSize(pageSize);
            if (pagingRequest.PagingState != null)
            {
                statement.SetPagingState(Convert.FromBase64String(pagingRequest.PagingState));
            }
            var data = await ExecuteCommand(statement);
            var pagingState = Convert.ToBase64String(data.PagingState);
            return new PagingResponseModel<JObject>()
            {
                Items = data.ParseListJObject(),
                PagingData = new()
                {
                    PagingState = pagingState,
                    Page = pagingRequest.PageIndex + 1,
                    PageIndex = pagingRequest.PageIndex,
                    PageSize = pagingRequest.PageSize,
                    Total = count,
                    TotalPage = (int)Math.Ceiling((double)count / pageSize)
                }
            };
        }

        public async Task<List<JObject>> GetAllAsync(
            string tableName,
            IEnumerable<string>? selectFields = null,
            IEnumerable<MixSortByColumn>? sortByFields = null,
            CancellationToken cancellationToken = default)
        {
            var commandText = StatementBuilder.CreateSelect(tableName, selectFieldNames: selectFields);
            var data = await ExecuteCommand(commandText);
            return data.ParseListJObject() ?? new();
        }

        public async Task<List<JObject>> GetListAsync(
            string tableName,
            IEnumerable<MixQueryField> queryFields,
            MixConjunction conjunction,
            IEnumerable<MixSortByColumn>? sortByFields = null,
            IEnumerable<string>? selectFields = null,
            CancellationToken cancellationToken = default)
        {
            var commandText = StatementBuilder.CreateSelect(tableName, queryFields, conjunction, selectFields);
            var data = await ExecuteCommand(commandText);
            return data.ParseListJObject() ?? new();
        }

        private async Task<long> CountAsync(string tableName, IEnumerable<MixQueryField> queryFields, MixConjunction conjunction)
        {
            var countCommandText = StatementBuilder.CreateCount(tableName, queryFields, conjunction);
            var resultSet = await ExecuteCommand(countCommandText);
            var count = resultSet.FirstOrDefault()?.GetValue<long>(0);
            return count ?? 0;
        }

        #endregion

        #region Helpers

        public async Task<RowSet> ExecuteCommand(SimpleStatement command)
        {
            try
            {
                return await _session.ExecuteAsync(command);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
