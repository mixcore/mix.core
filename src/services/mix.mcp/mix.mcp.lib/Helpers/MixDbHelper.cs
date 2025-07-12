using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Heart.Extensions;
using Mix.Heart.UnitOfWork;
using Mix.MCP.Lib.Models;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.ViewModels;
using Mix.RepoDb.ViewModels;
using System.Text.RegularExpressions;

namespace Mix.MCP.Lib.Helpers
{
    /// <summary>
    /// Helper class for Mix Database operations
    /// </summary>
    public class MixDbHelper
    {
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        private readonly IMixdbStructure _mixDbStructureService;
        private readonly ILogger _logger;

        /// <summary>
        /// Initialize a new instance of MixDbHelper
        /// </summary>
        public MixDbHelper(
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMixdbStructure mixDbStructureService,
            ILogger logger)
        {
            _cmsUow = cmsUow;
            _mixDbStructureService = mixDbStructureService;
            _logger = logger;
        }

        /// <summary>
        /// Get database view model by system name
        /// </summary>
        /// <param name="databaseSystemName">System name of the database</param>
        /// <param name="cacheService">Cache service if caching is needed</param>
        /// <returns>The database view model or null if not found</returns>
        public async Task<MixDbTableViewModel> GetDatabaseBySystemName(
            string databaseSystemName,
            Mix.Heart.Services.MixCacheService cacheService = null)
        {
            // First try to get the database entity
            return await MixDbTableViewModel.GetRepository(_cmsUow, cacheService)
                    .GetSingleAsync(db => db.SystemName == databaseSystemName && !db.IsDeleted);
        }

        /// <summary>
        /// Check if a database exists by system name
        /// </summary>
        public async Task<bool> DatabaseExists(string databaseSystemName)
        {
            return await _cmsUow.DbContext.MixDbTable
                .AnyAsync(db => db.SystemName == databaseSystemName && !db.IsDeleted);
        }

        /// <summary>
        /// CreateMixDbData a new database with the specified columns
        /// </summary>
        public async Task<MixDbTableViewModel> CreateDatabase(
            string displayName,
            string systemName,
            List<ColumnInfo> columns,
            string description = null,
            MixDatabaseNamingConvention namingConvention = MixDatabaseNamingConvention.SnakeCase,
            MixDbTableType type = MixDbTableType.Service,
            int mixDatabaseContextId = 1)
        {
            // CreateMixDbData database
            var dbViewModel = new MixDbTableViewModel(_cmsUow)
            {
                TenantId = 1,
                DisplayName = displayName,
                SystemName = systemName,
                Type = type,
                //Description = description,
                NamingConvention = namingConvention,
                MixDbDatabaseId = mixDatabaseContextId
            };

            // Add custom columns
            foreach (var column in columns)
            {
                AddColumnToViewModel(dbViewModel, column);
            }

            // Save database
            var result = await dbViewModel.SaveAsync();
            if (result <= 0)
            {
                _logger.LogError("Failed to create database: {Name}", systemName);
                return null;
            }

            // Migrate the database schema
            await _mixDbStructureService.MigrateDatabase(systemName);

            return dbViewModel;
        }

        /// <summary>
        /// Add a column to a database
        /// </summary>
        public async Task<bool> AddColumn(
            MixDbTableViewModel database,
            ColumnInfo column)
        {
            if (database == null)
            {
                _logger.LogError("Cannot add column to null database");
                return false;
            }

            // Check if column already exists
            if (database.Columns.Any(c => c.SystemName == column.Name))
            {
                _logger.LogWarning("Column with name {Name} already exists", column.Name);
                return false;
            }

            // Add column to database
            var newCol = ParseColumnToViewModel(database, column);
            await newCol.SaveAsync();
            // Migrate database schema
            await _mixDbStructureService.AddColumn(newCol);

            return true;
        }

        /// <summary>
        /// UpdateMixDbData a column in a database
        /// </summary>
        public async Task<bool> UpdateColumn(
            MixDbTableViewModel database,
            string columnSystemName,
            string newDisplayName = null,
            MixDataType? newDataType = null,
            bool? isRequired = null,
            string newDescription = null,
            string newDefaultValue = null)
        {
            if (database == null)
            {
                _logger.LogError("Cannot update column in null database");
                return false;
            }

            // Find column in database
            var columnViewModel = database.Columns
                .FirstOrDefault(c => c.SystemName == columnSystemName);

            if (columnViewModel == null)
            {
                _logger.LogWarning("Column {Name} not found in database {Database}",
                    columnSystemName, database.SystemName);
                return false;
            }

            // Ensure we're not modifying system columns in a dangerous way
            if ((columnSystemName == "id" ||
                 columnSystemName == "created_by" ||
                 columnSystemName == "created_date_time" ||
                 columnSystemName == "last_modified") &&
                (newDataType.HasValue || isRequired.HasValue))
            {
                _logger.LogWarning("Cannot modify data type or required status of system column {Name}",
                    columnSystemName);
                return false;
            }

            // UpdateMixDbData column properties
            bool dataTypeChanged = false;

            if (!string.IsNullOrEmpty(newDisplayName))
            {
                columnViewModel.DisplayName = newDisplayName;
            }

            if (newDataType.HasValue)
            {
                columnViewModel.DataType = newDataType.Value;
                dataTypeChanged = true;
            }

            if (isRequired.HasValue)
            {
                columnViewModel.ColumnConfigurations.IsRequire = isRequired.Value;
            }

            if (newDescription != null) // Allow empty string to clear description
            {
                // MixDbTableName columns might not have a direct Description property
                // We might need to store this in metadata or another related field
                _logger.LogInformation("Would update description for column {ColumnName} to: {Description}",
                    columnSystemName, newDescription);
            }

            if (newDefaultValue != null) // Allow empty string to clear default value
            {
                columnViewModel.DefaultValue = newDefaultValue;
            }

            // Migrate database schema if data type changed
            if (dataTypeChanged)
            {
                await _mixDbStructureService.AlterColumn(columnViewModel, false);
            }

            return true;
        }

        /// <summary>
        /// DeleteMixDbData a column from a database
        /// </summary>
        public async Task<bool> DeleteColumn(
            MixDbTableViewModel database,
            string columnSystemName)
        {
            if (database == null)
            {
                _logger.LogError("Cannot delete column from null database");
                return false;
            }

            // Find column in view model
            var columnViewModel = database.Columns
                .FirstOrDefault(c => c.SystemName == columnSystemName);

            if (columnViewModel == null)
            {
                _logger.LogWarning("Column {Name} not found in database {Database}",
                    columnSystemName, database.SystemName);
                return false;
            }

            // Ensure we're not deleting system columns
            if (columnSystemName == "id" ||
                columnSystemName == "created_by" ||
                columnSystemName == "created_date_time" ||
                columnSystemName == "last_modified" ||
                columnSystemName == "priority" ||
                columnSystemName == "status")
            {
                _logger.LogWarning("Cannot delete system column {Name}", columnSystemName);
                return false;
            }

            // Mark column for deletion
            columnViewModel.IsDeleted = true;

            // Save changes
            var result = await database.SaveAsync();
            if (result <= 0)
            {
                _logger.LogError("Failed to delete column {Name} from database {Database}",
                    columnSystemName, database.SystemName);
                return false;
            }

            // Migrate database schema
            await _mixDbStructureService.MigrateDatabase(database.SystemName);

            return true;
        }

        /// <summary>
        /// Generate a system name from a display name
        /// </summary>
        public static string GenerateSystemName(string displayName, MixDatabaseNamingConvention namingConvention)
        {
            string prefix = "mix_";
            string name = displayName
                .ToLower()
                .Replace(" ", "_")
                .Replace("-", "_")
                .Replace(".", "_")
                .Replace(",", "_");

            name = Regex.Replace(name, @"[^a-z0-9_]", "");

            // Apply additional formatting based on naming convention if needed
            // Currently we just use snake_case for all

            return prefix + name;
        }

        /// <summary>
        /// Format a string as display name (Title Case)
        /// </summary>
        public static string FormatDisplayName(string name)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                name.Replace("_", " ").Replace("-", " ")
            );
        }

        /// <summary>
        /// Add a column to the database view model
        /// </summary>
        private void AddColumnToViewModel(MixDbTableViewModel db, ColumnInfo column)
        {
            db.Columns.Add(ParseColumnToViewModel(db, column));
        }

        private MixdbColumnViewModel ParseColumnToViewModel(MixDbTableViewModel db, ColumnInfo column)
        {
            string displayName = FormatDisplayName(column.Name);
            return new MixdbColumnViewModel(_cmsUow)
            {
                MixDbTableId = db.Id,
                MixDbTableName = db.SystemName,
                DataType = column.DataType,
                DisplayName = displayName,
                SystemName = column.Name,
                ColumnConfigurations = new Mix.Shared.Models.ColumnConfigurations
                {
                    IsRequire = column.IsRequired,
                    IsEncrypt = false,
                    IsUnique = column.Name.Equals("id", StringComparison.OrdinalIgnoreCase)
                }
            };
        }

        public async Task CreateRelationship(
            string sourceTableName,
            string destTableName,
            string displayName,
            string? propertyName,
            string? sourceColumnName,
            string? destinateColumnName,
            MixDbTableRelationshipType relationshipType,
            CancellationToken cancellationToken = default)
        {
            var sourceTable = await GetDatabaseBySystemName(sourceTableName);
            if (sourceTable is null)
            {
                throw new Exception($"Invalid Table {sourceTableName}");
            }
            var destTable = await GetDatabaseBySystemName(destTableName);
            if (sourceTable is null)
            {
                throw new Exception($"Invalid Table {destTableName}");
            }
            var relationship = new MixDbTableRelationshipViewModel(_cmsUow)
            {
                DisplayName = displayName,
                SourceTableName = sourceTableName,
                DestinateTableName = destTableName,
                PropertyName = propertyName ?? displayName.ToSEOString('_'),
                SourceColumnName = sourceColumnName ?? $"{sourceTableName}_id",
                DestinateColumnName = destinateColumnName,
                Type = relationshipType,
                ParentId = sourceTable.Id,
                ChildId = destTable.Id
            };
            await relationship.SaveAsync(cancellationToken);

        }
    }
}