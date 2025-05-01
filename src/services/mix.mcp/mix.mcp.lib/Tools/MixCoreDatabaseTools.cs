using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;
using Mix.Constant.Enums;
using Mix.Service.Interfaces;
using Mix.Database.Entities.Cms;
using System.Text.Json.Nodes;
using System.Data;
using Newtonsoft.Json.Linq;
using Mix.Heart.UnitOfWork;
using Mix.Heart.Services;
using Mix.Heart.Extensions;
using Microsoft.EntityFrameworkCore;
using Mix.Mixdb.Interfaces;
using Mix.Lib.ViewModels;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Mixdb.ViewModels;

namespace Mix.MCP.Lib.Tools
{
    [McpServerToolType]
    public class MixCoreDatabaseTools
    {
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        private readonly IMixdbStructureService _mixDbService;
        private readonly IMixMemoryCacheService _memoryCache;
        private readonly MixCacheService _cacheService;
        private readonly DatabaseService _databaseService;

        public MixCoreDatabaseTools(
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMixdbStructureService mixDbService,
            IMixMemoryCacheService memoryCache,
            MixCacheService cacheService,
            DatabaseService databaseService)
        {
            _cmsUow = cmsUow;
            _mixDbService = mixDbService;
            _memoryCache = memoryCache;
            _cacheService = cacheService;
            _databaseService = databaseService;
        }

        [McpServerTool, Description("Create a new database in the system")]
        public async Task<string> CreateDatabase(
            [Description("Display name for the database")] string displayName,
            [Description("System name for the database (if empty, will be generated from display name)")] string systemName = null,
            [Description("Type of database (Default, GuidService, AdditionalData, GuidAdditionalData)")] MixDatabaseType type = MixDatabaseType.Service,
            [Description("Description of the database")] string description = null,
            [Description("ID of the database context (optional)")] int? contextId = null)
        {
            try
            {
                if (string.IsNullOrEmpty(systemName))
                {
                    systemName = displayName.ToColumnName(type == MixDatabaseType.GuidService || type == MixDatabaseType.GuidAdditionalData);
                }

                var existingDb = await _cmsUow.DbContext.MixDatabase
                    .FirstOrDefaultAsync(db => db.SystemName == systemName && !db.IsDeleted);

                if (existingDb != null)
                {
                    return $"A database with the system name '{systemName}' already exists";
                }

                var dbViewModel = new MixDbDatabaseViewModel(_cmsUow)
                {
                    DisplayName = displayName,
                    SystemName = systemName,
                    Type = type,
                    Description = description,
                    MixDatabaseContextId = contextId
                };

                var result = await dbViewModel.SaveAsync();
                if (result > 0)
                {
                    await _mixDbService.Migrate(dbViewModel, _databaseService.DatabaseProvider);

                    return JsonSerializer.Serialize(new
                    {
                        Success = true,
                        Message = $"Database '{displayName}' created successfully",
                        SystemName = systemName
                    });
                }
                else
                {
                    return $"Failed to create database: {systemName}";
                }
            }
            catch (Exception ex)
            {
                return $"Error creating database: {ex.Message}";
            }
        }

        [McpServerTool, Description("Add a column to an existing database")]
        public async Task<string> AddColumn(
            [Description("Name of the database to add the column to")] string databaseName,
            [Description("Display name for the column")] string displayName,
            [Description("System name for the column (if empty, will be generated from display name)")] string systemName = null,
            [Description("Data type for the column (String, Integer, Double, Boolean, DateTime, etc.)")] MixDataType dataType = MixDataType.String,
            [Description("Default value for the column (optional)")] string defaultValue = null,
            [Description("Whether the column is required")] bool isRequired = false,
            [Description("Whether the column value should be encrypted")] bool isEncrypt = false,
            [Description("Whether the column value should be unique")] bool isUnique = false,
            [Description("Maximum length for string columns (optional)")] int? maxLength = null)
        {
            try
            {
                var database = await _memoryCache.TryGetValueAsync(
                    databaseName,
                    cache =>
                    {
                        cache.SlidingExpiration = TimeSpan.FromSeconds(20);
                        return MixDbDatabaseViewModel.GetRepository(_cmsUow, _cacheService)
                            .GetSingleAsync(m => m.SystemName == databaseName);
                    });

                if (database == null)
                {
                    return $"Database '{databaseName}' not found";
                }

                if (string.IsNullOrEmpty(systemName))
                {
                    systemName = displayName.ToColumnName(database.NamingConvention == MixDatabaseNamingConvention.TitleCase);
                }

                if (database.Columns.Any(c => c.SystemName == systemName))
                {
                    return $"A column with the system name '{systemName}' already exists in this database";
                }

                var columnViewModel = new MixdbDatabaseColumnViewModel(_cmsUow)
                {
                    MixDatabaseId = database.Id,
                    MixDatabaseName = databaseName,
                    DisplayName = displayName,
                    SystemName = systemName,
                    DataType = dataType,
                    DefaultValue = defaultValue,
                    ColumnConfigurations = new Shared.Models.ColumnConfigurations()
                    {
                        IsRequire = isRequired,
                        IsEncrypt = isEncrypt,
                        IsUnique = isUnique
                    }
                };

                if (maxLength.HasValue)
                {
                    columnViewModel.ColumnConfigurations.MaxLength = maxLength.Value;
                }

                var result = await columnViewModel.SaveAsync();
                if (result > 0)
                {
                    await _mixDbService.AddColumn(database, columnViewModel);

                    return JsonSerializer.Serialize(new
                    {
                        Success = true,
                        Message = $"Column '{displayName}' added to database '{databaseName}' successfully",
                        SystemName = systemName
                    });
                }
                else
                {
                    return $"Failed to add column: {systemName}";
                }
            }
            catch (Exception ex)
            {
                return $"Error adding column: {ex.Message}";
            }
        }

        //[McpServerTool, Description("Create or update data in a database")]
        //public async Task<string> SaveData(
        //    [Description("Name of the database to save data to")] string databaseName,
        //    [Description("JSON data to save (fields must match column names)")] string data,
        //    [Description("ID of the record to update (if creating new, leave as null)")] string id = null)
        //{
        //    try
        //    {
        //        var database = await _memoryCache.TryGetValueAsync(
        //            databaseName,
        //            cache =>
        //            {
        //                cache.SlidingExpiration = TimeSpan.FromSeconds(20);
        //                return MixDbDatabaseViewModel.GetRepository(_cmsUow, _cacheService)
        //                    .GetSingleAsync(m => m.SystemName == databaseName);
        //            });

        //        if (database == null)
        //        {
        //            return $"Database '{databaseName}' not found";
        //        }

        //        JsonNode dataObj;
        //        try
        //        {
        //            dataObj = JsonNode.Parse(data);
        //            if (dataObj == null)
        //            {
        //                return "Invalid JSON data";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return $"Invalid JSON data: {ex.Message}";
        //        }

        //        var jObject = JObject.Parse(data);

        //        if (!string.IsNullOrEmpty(id))
        //        {
        //            if (database.Type == MixDatabaseType.GuidService || database.Type == MixDatabaseType.GuidAdditionalData)
        //            {
        //                if (Guid.TryParse(id, out Guid guidId))
        //                {
        //                    jObject["id"] = guidId;
        //                }
        //                else
        //                {
        //                    return "Invalid GUID format for ID";
        //                }
        //            }
        //            else
        //            {
        //                if (int.TryParse(id, out int intId))
        //                {
        //                    jObject["id"] = intId;
        //                }
        //                else
        //                {
        //                    return "Invalid integer format for ID";
        //                }
        //            }

        //            var updateResult = await _mixDbService.Migrate(
        //                databaseName,
        //                id,
        //                jObject,
        //                "system",
        //                jObject.Properties().Select(p => p.Name),
        //                CancellationToken.None);

        //            return JsonSerializer.Serialize(new
        //            {
        //                Success = true,
        //                Message = "Data updated successfully",
        //                Id = id
        //            });
        //        }
        //        else
        //        {
        //            var newId = await _mixDbService.CreateAsync(
        //                databaseName,
        //                jObject,
        //                "system",
        //                CancellationToken.None);

        //            return JsonSerializer.Serialize(new
        //            {
        //                Success = true,
        //                Message = "Data created successfully",
        //                Id = newId
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return $"Error saving data: {ex.Message}";
        //    }
        //}
    }
}