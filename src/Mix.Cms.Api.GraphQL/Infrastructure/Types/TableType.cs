using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Api.GraphQL.Infrastructure
{
    public class TableType : ObjectGraphType<object>
    {
        public QueryArguments TableArgs
        {
            get; set;
        }
        private IDictionary<string, Type> _databaseTypeToSystemType;
        protected IDictionary<string, Type> DatabaseTypeToSystemType
        {
            get
            {
                if (_databaseTypeToSystemType == null)
                {
                    _databaseTypeToSystemType = new Dictionary<string, Type> {
                    { "uniqueidentifier", typeof(String) },
                    { "char", typeof(String) },
                    { "nvarchar(250)", typeof(String) },
                    { "nvarchar(50)", typeof(String) },
                    { "text", typeof(String) },
                    { "int", typeof(int) },
                    { "decimal", typeof(decimal) },
                    { "bit", typeof(bool) }
                };
                }
                return _databaseTypeToSystemType;
            }
        }
        public TableType(TableMetadata tableMetadata)
        {
            Name = tableMetadata.TableName;
            foreach (var tableColumn in tableMetadata.Columns)
            {
                InitGraphTableColumn(tableColumn);
            }
        }
        private void InitGraphTableColumn(ColumnMetadata columnMetadata)
        {
            var graphQLType = (ResolveColumnMetaType(columnMetadata.DataType)).GetGraphTypeFromType(true);
            var columnField = Field(
                graphQLType,
                columnMetadata.ColumnName
            ); 
            columnField.Resolver = new NameFieldResolver();
            FillArgs(columnMetadata.ColumnName);
        }
        private void FillArgs(string columnName)
        {
            if (TableArgs == null)
            {
                TableArgs = new QueryArguments(
                    new QueryArgument<StringGraphType>()
                    {
                        Name = columnName
                    }
                );
            }
            else
            {
                TableArgs.Add(new QueryArgument<StringGraphType> { Name = columnName });
            }
            TableArgs.Add(new QueryArgument<IdGraphType> { Name = "id" });
            TableArgs.Add(new QueryArgument<IntGraphType> { Name = "first" });
            TableArgs.Add(new QueryArgument<IntGraphType> { Name = "offset" });
        }
        private Type ResolveColumnMetaType(string dbType)
        {
            if (DatabaseTypeToSystemType.ContainsKey(dbType))
                return DatabaseTypeToSystemType[dbType]; return typeof(String);
        }
    }
}
