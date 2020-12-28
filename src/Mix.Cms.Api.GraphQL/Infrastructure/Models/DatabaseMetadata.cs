using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Mix.Cms.Api.GraphQL.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace Mix.Cms.Api.GraphQL.Infrastructure.Models
{
    public sealed class DatabaseMetadata : IDatabaseMetadata
    {
        private readonly DbContext _dbContext;
        private readonly ITableNameLookup _tableNameLookup; 
        private string _databaseName;
        private IEnumerable<TableMetadata> _tables; 
        public DatabaseMetadata(DbContext dbContext, ITableNameLookup tableNameLookup)
        {
            _dbContext = dbContext;
            _tableNameLookup = tableNameLookup; 
            _databaseName = _dbContext.Database.GetDbConnection().Database; 
            if (_tables == null)
                ReloadMetadata();
        }
        public IEnumerable<TableMetadata> GetTableMetadatas()
        {
            if (_tables == null)
                return new List<TableMetadata>(); return _tables;
        }
        public void ReloadMetadata()
        {
            _tables = FetchTableMetaData();
        }
        private IReadOnlyList<TableMetadata> FetchTableMetaData()
        {
            var metaTables = new List<TableMetadata>(); 
            foreach (var entityType in _dbContext.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                metaTables.Add(new TableMetadata
                {
                    TableName = tableName,
                    AssemblyFullName = entityType.ClrType.FullName,
                    Columns = GetColumnsMetadata(entityType)
                }); 
                _tableNameLookup.InsertKeyName(tableName);
            }

            return metaTables;
        }
        private IReadOnlyList<ColumnMetadata> GetColumnsMetadata(IEntityType entityType)
        {
            var tableColumns = new List<ColumnMetadata>(); foreach (var propertyType in entityType.GetProperties())
            {
                tableColumns.Add(new ColumnMetadata
                {
                    ColumnName = propertyType.Name,
                    DataType = propertyType.GetColumnType()
                });
            }
            return tableColumns;
        }
    }
}
