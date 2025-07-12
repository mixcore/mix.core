using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Heart.UnitOfWork;
using Mix.MCP.Lib.Helpers;
using Mix.Mixdb.Interfaces;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using ModelContextProtocol.Server;
using RepoDb;
using Mix.Heart.Model;
using Mix.Shared.Models;
using Mix.Shared.Dtos;
using Mix.Constant.Constants;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Helpers;

namespace Mix.MCP.Lib.Tools
{
    /// <summary>
    /// Tool for performing CRUD operations on Mix Database data
    /// </summary>
    [McpServerToolType]
    public class MixDbDataTool : BaseMcpTool
    {
        private readonly IMixDbDataService _mixDbService;
        private readonly MixDbHelper _databaseHelper;

        /// <summary>
        /// Initializes a new instance of the MixDbDataTool class
        /// </summary>
        public MixDbDataTool(
            DatabaseService databaseService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMixDbDataServiceFactory mixDbDataSrvFactory,
            ILogger<MixDbDataTool> logger)
            : base(cmsUow, logger)
        {
            _mixDbService = mixDbDataSrvFactory.Create(databaseService.DatabaseProvider, databaseService.GetConnectionString(MixConstants.CONST_ACCOUNT_CONNECTION))!;
            _databaseHelper = new MixDbHelper(cmsUow, null, logger);
        }

        /// <summary>
        /// Get a single record by ID
        /// </summary>
        [McpServerTool, Description("Get a single record by ID from a Mix Database")]
        public async Task<string> GetMixDbDataById(
            [Description("System name of the database (e.g., mix_products)")] string databaseSystemName,
            [Description("ID of the record to retrieve")] int id,
            [Description("Comma-separated list of columns to select (e.g., 'objId,name,price')")] string selectColumns = null)
        {
            if (string.IsNullOrWhiteSpace(databaseSystemName))
                return "Database system name cannot be empty.";

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                var database = await _databaseHelper.GetDatabaseBySystemName(databaseSystemName);
                if (database == null)
                    return $"Database with system name '{databaseSystemName}' not found.";

                var record = await _mixDbService.GetByIdAsync(databaseSystemName, id, selectColumns, ct);
                if (record == null)
                    return $"Record with ID {id} not found in database '{databaseSystemName}'.";

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Data = record
                }).ToString();
            }, "GetMixDbDataById");
        }

        /// <summary>
        /// Get records by query conditions
        /// </summary>
        [McpServerTool, Description("Get records by query conditions from a Mix Database")]
        public async Task<string> GetListMidxDbData(
            [Description("System name of the database (e.g., mix_products)")] string databaseSystemName,
            [Description("Query conditions in JSON format (e.g., '[{\"Field\":\"name\",\"Value\":\"Product 1\",\"Method\":\"Equal\"}]')")] string queryJson,
            [Description("Sort conditions in JSON format (e.g., '[{\"Field\":\"name\",\"Direction\":\"Ascending\"}]')")] string sortJson = null,
            [Description("Comma-separated list of columns to select (e.g., 'objId,name,price')")] string selectColumns = null,
            [Description("Whether to load nested data")] bool loadNestedData = false)
        {
            if (string.IsNullOrWhiteSpace(databaseSystemName))
                return "Database system name cannot be empty.";
            if (string.IsNullOrWhiteSpace(queryJson))
                return "Query conditions cannot be empty.";

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                var database = await _databaseHelper.GetDatabaseBySystemName(databaseSystemName);
                if (database == null)
                    return $"Database with system name '{databaseSystemName}' not found.";

                var queryFields = System.Text.Json.JsonSerializer.Deserialize<List<QueryField>>(queryJson);
                var sortFields = !string.IsNullOrEmpty(sortJson)
                    ? System.Text.Json.JsonSerializer.Deserialize<List<MixSortByColumn>>(sortJson)
                    : null;

                var records = await _mixDbService.GetListByAsync(
                    databaseSystemName,
                    queryFields,
                    sortFields,
                    selectColumns,
                    loadNestedData,
                    ct);

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Data = records
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "GetMidxDbDataListBy");
        }

        /// <summary>
        /// CreateMixDbData a new record
        /// </summary>
        [McpServerTool, Description("Create a new record in a Mix Database")]
        public async Task<string> CreateMixDbData(
            [Description("System name of the database (e.g., mix_products)")] string databaseSystemName,
            [Description("Record data in JSON format (e.g., '{\"name\":\"Product 1\",\"price\":99.99}')")] string dataJson,
            [Description("Username of the creator")] string createdBy = null)
        {
            if (string.IsNullOrWhiteSpace(databaseSystemName))
                return "Database system name cannot be empty.";
            if (string.IsNullOrWhiteSpace(dataJson))
                return "Record data cannot be empty.";

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                var database = await _databaseHelper.GetDatabaseBySystemName(databaseSystemName);
                if (database == null)
                    return $"Database with system name '{databaseSystemName}' not found.";

                var data = JObject.Parse(dataJson);
                var id = await _mixDbService.CreateAsync(databaseSystemName, data, createdBy, ct);

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Message = "Record created successfully",
                    Id = id
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "CreateMixDbData");
        }

        /// <summary>
        /// CreateMixDbData multiple records
        /// </summary>
        [McpServerTool, Description("Create multiple records in a Mix Database")]
        public async Task<string> CreateManyMixDbData(
            [Description("System name of the database (e.g., mix_products)")] string databaseSystemName,
            [Description("Array of record data in JSON format (e.g., '[{\"name\":\"Product 1\",\"price\":99.99},{\"name\":\"Product 2\",\"price\":149.99}]')")] string dataJson,
            [Description("Username of the creator")] string createdBy = null)
        {
            if (string.IsNullOrWhiteSpace(databaseSystemName))
                return "Database system name cannot be empty.";
            if (string.IsNullOrWhiteSpace(dataJson))
                return "Record data cannot be empty.";

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                var database = await _databaseHelper.GetDatabaseBySystemName(databaseSystemName);
                if (database == null)
                    return $"Database with system name '{databaseSystemName}' not found.";

                var data = JArray.Parse(dataJson).ToObject<List<JObject>>();
                await _mixDbService.CreateManyAsync(databaseSystemName, data, createdBy, ct);

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Message = $"Successfully created {data.Count} records"
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "CreateManyMixDbData");
        }
        
        /// <summary>
        /// CreateMixDbData multiple records
        /// </summary>
        [McpServerTool, Description("Update multiple records in a Mix Database")]
        public async Task<string> UpdateManyMixDbData(
            [Description("System name of the database (e.g., mix_products)")] string databaseSystemName,
            [Description("Array of record data in JSON format (e.g., '[{\"name\":\"Product 1\",\"price\":99.99},{\"name\":\"Product 2\",\"price\":149.99}]')")] string dataJson,
            [Description("Username of the creator")] string modifiedBy = null)
        {
            if (string.IsNullOrWhiteSpace(databaseSystemName))
                return "Database system name cannot be empty.";
            if (string.IsNullOrWhiteSpace(dataJson))
                return "Record data cannot be empty.";

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                var database = await _databaseHelper.GetDatabaseBySystemName(databaseSystemName);
                if (database == null)
                    return $"Database with system name '{databaseSystemName}' not found.";

                var data = JArray.Parse(dataJson).ToObject<List<JObject>>();
                await _mixDbService.UpdateManyAsync(databaseSystemName, data, modifiedBy, ct);

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Message = $"Successfully updated {data.Count} records"
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "CreateManyMixDbData");
        }

        /// <summary>
        /// UpdateMixDbData a record
        /// </summary>
        [McpServerTool, Description("Update a record in a Mix Database")]
        public async Task<string> UpdateMixDbData(
            [Description("System name of the database (e.g., mix_products)")] string databaseSystemName,
            [Description("ID of the record to update")] string strId,
            [Description("Record data in JSON format (e.g., '{\"name\":\"Updated Product\",\"price\":199.99}')")] string dataJson,
            [Description("Username of the modifier")] string modifiedBy = null)
        {
            if (string.IsNullOrWhiteSpace(databaseSystemName))
                return "Database system name cannot be empty.";
            if (string.IsNullOrWhiteSpace(dataJson))
                return "Record data cannot be empty.";

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                var database = await _databaseHelper.GetDatabaseBySystemName(databaseSystemName);
                if (database == null)
                    return $"Database with system name '{databaseSystemName}' not found.";

                var data = JObject.Parse(dataJson);
                var fields = data.Properties().Where(m => m.Name.ToLower() != "id").Select(m => m.Name).ToArray();
                object? id = default;
                if (int.TryParse(strId, out int integerId))
                {
                    id = integerId;
                }
                else if (Guid.TryParse(strId, out Guid guidId))
                {
                    id = guidId;
                }
                if (!data.ContainsKey("id"))
                {
                    data.Add(new JProperty("id", id));
                }
                var result = await _mixDbService.UpdateAsync(databaseSystemName, id, data, modifiedBy, fields, ct);

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Message = "Record updated successfully",
                    Result = result
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "UpdateMixDbData");
        }

        /// <summary>
        /// DeleteMixDbData a record
        /// </summary>
        [McpServerTool, Description("Delete a record from a Mix Database")]
        public async Task<string> DeleteMixDbData(
            [Description("System name of the database (e.g., mix_products)")] string databaseSystemName,
            [Description("ID of the record to delete")] object id,
            [Description("Username of the deleter")] string modifiedBy = null)
        {
            if (string.IsNullOrWhiteSpace(databaseSystemName))
                return "Database system name cannot be empty.";

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                var database = await _databaseHelper.GetDatabaseBySystemName(databaseSystemName);
                if (database == null)
                    return $"Database with system name '{databaseSystemName}' not found.";

                var result = await _mixDbService.DeleteAsync(databaseSystemName, id, modifiedBy, ct);

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Message = "Record deleted successfully",
                    AffectedRows = result
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "DeleteMixDbData");
        }

        /// <summary>
        /// Get paged results
        /// </summary>
        [McpServerTool, Description("Get paged results from a Mix Database")]
        public async Task<string> GetPagingMixDbData(
            [Description("System name of the database (e.g., mix_products)")] string databaseSystemName,
            [Description("Page number (1-based)")] int page = 1,
            [Description("Page size")] int pageSize = 10,
            [Description("Query conditions in JSON format (e.g., '[{\"Field\":\"name\",\"Value\":\"Product\",\"Method\":\"Contains\"}]')")] string queryJson = null,
            [Description("Sort conditions in JSON format (e.g., '[{\"Field\":\"name\",\"Direction\":\"Ascending\"}]')")] string sortJson = null,
            [Description("Comma-separated list of columns to select (e.g., 'objId,name,price')")] string selectColumns = null)
        {
            if (string.IsNullOrWhiteSpace(databaseSystemName))
                return "Database system name cannot be empty.";
            if (page < 1)
                return "Page number must be greater than 0.";
            if (pageSize < 1)
                return "Page size must be greater than 0.";

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                var database = await _databaseHelper.GetDatabaseBySystemName(databaseSystemName);
                if (database == null)
                    return $"Database with system name '{databaseSystemName}' not found.";

                var request = new SearchMixDbRequestModel
                {
                    Paging = new PagingRequestModel
                    {
                        Page = page,
                        PageSize = pageSize
                    },
                    SelectColumns = selectColumns
                };

                if (!string.IsNullOrEmpty(queryJson))
                    request.Queries = System.Text.Json.JsonSerializer.Deserialize<List<MixQueryField>>(queryJson);
                if (!string.IsNullOrEmpty(sortJson))
                    request.Paging.SortByColumns = System.Text.Json.JsonSerializer.Deserialize<List<MixSortByColumn>>(sortJson);

                var result = await _mixDbService.GetPagingAsync(request, ct);

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Data = result
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "GetPagingMixDbData");
        }
    }
}